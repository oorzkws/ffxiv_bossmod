using System;

namespace BossMod.RDM
{
    public static class Rotation
    {
        public class State(WorldState ws) : CommonRotation.PlayerState(ws)
        {
            public float DualcastLeft; // 15s max
            public float SwiftcastLeft; // 10s max
            public float AccelerationLeft; // 20s max
            public float ManaficationLeft; // 15s max
            public float VerstoneReadyLeft; // 30s max
            public float VerfireReadyLeft; // 30s max
            public byte WhiteMana; // 100 max
            public byte BlackMana; // 100 max
            public byte ManaStacks; // 6 max

            public byte MinMana => Math.Min(WhiteMana, BlackMana);
            public float InstantCastLeft => MathF.Max(DualcastLeft, SwiftcastLeft);

            public AID BestRiposte => MinMana >= 20 ? AID.EnchantedRiposte : AID.Riposte;
            public AID BestZwerchhau => MinMana >= 15 ? AID.EnchantedZwerchhau : AID.Zwerchhau;
            public AID BestRedoublement => MinMana >= 15 ? AID.EnchantedRedoublement : AID.Redoublement;
            public AID BestMoulinet => MinMana >= 20 ? AID.EnchantedMoulinet : AID.Moulinet;
            public AID BestReprise => MinMana >= 5 ? AID.EnchantedReprise : AID.Reprise;

            public AID BestJolt =>
                ResolutionReady
                    ? AID.Resolution
                    : ScorchReady
                        ? AID.Scorch
                        : Unlocked(AID.JoltII)
                            ? AID.JoltII
                            : AID.Jolt;
            public AID BestScatter =>
                ResolutionReady
                    ? AID.Resolution
                    : ScorchReady
                        ? AID.Scorch
                        : Unlocked(AID.Impact)
                            ? AID.Impact
                            : AID.Scatter;

            public AID BestAero2 => ManaStacks == 3 ? AID.Verholy : AID.VeraeroII;
            public AID BestAero =>
                ManaStacks == 3
                    ? AID.Verholy
                    : Unlocked(AID.VeraeroIII)
                        ? AID.VeraeroIII
                        : AID.Veraero;
            public AID BestThunder2 => ManaStacks == 3 ? AID.Verflare : AID.VerthunderII;
            public AID BestThunder =>
                ManaStacks == 3
                    ? AID.Verflare
                    : Unlocked(AID.VerthunderIII)
                        ? AID.VerthunderIII
                        : AID.Verthunder;

            public bool ScorchReady => Unlocked(AID.Scorch) && ComboLastMove is AID.Verflare or AID.Verholy;
            public bool ResolutionReady => Unlocked(AID.Resolution) && ComboLastMove is AID.Scorch;

            public bool CanMelee => RangeToTarget <= 3;

            public bool InMeleeCombo => ComboLastMove == AID.Zwerchhau && MinMana >= 15 || ComboLastMove == AID.Riposte && MinMana >= 30;

            public bool Unlocked(AID aid) => Definitions.Unlocked(aid, Level, UnlockProgress);

            public bool Unlocked(TraitID tid) => Definitions.Unlocked(tid, Level, UnlockProgress);

            public AID ComboLastMove => (AID)ComboLastAction;

            public float GetCastTime(AID aid)
            {
                if (SwiftcastLeft > GCD || DualcastLeft > GCD || AccelerationLeft > GCD && aid.IsAccelerated())
                    return 0f;

                return Definitions.SupportedActions[ActionID.MakeSpell(aid)].CastTime * SpellGCDTime / 2.5f;
            }

            public float GetSlidecastTime(AID aid) => MathF.Max(0, GetCastTime(aid) - 0.5f);

            public override string ToString()
            {
                return $"Combo={ComboLastMove}";
            }
        }

        public class Strategy : CommonRotation.Strategy
        {
            public int NumAOETargets; // 25y/5y radius
            public int NumManastackAOETargets; // 25/5y radius. tracked separately since we use these GCDs, but not AOE GCDs, on bosses
            public int NumC6Targets; // 25y/6y radius
            public int NumMoulinetTargets; // 8y/120deg cone
            public int NumResolutionTargets; // 25y/4y rect

            public float ActualFightEndIn => FightEndIn == 0 ? 10000f : FightEndIn;

            public void ApplyStrategyOverrides(uint[] overrides) { }
        }

        private static bool CanCast(State state, Strategy strategy, float castTime)
        {
            var castEndIn = state.GCD + castTime;
            var moveOk = castTime == 0 || strategy.ForceMovementIn > castEndIn;

            return moveOk && strategy.ActualFightEndIn > castEndIn;
        }

        private static bool CanCast(State state, Strategy strategy, AID action) =>
            state.Unlocked(action) && CanCast(state, strategy, state.GetSlidecastTime(action));

        public static AID GetNextBestGCD(State state, Strategy strategy)
        {
            if (strategy.CombatTimer < 0)
            {
                if (strategy.CombatTimer > -state.GetCastTime(state.BestThunder))
                    return state.BestThunder;

                if (strategy.CombatTimer > -100)
                    return AID.None;
            }

            if (state.ResolutionReady)
                return AID.Resolution;

            if (state.ScorchReady)
                return AID.Scorch;

            if (state.ManaStacks == 3)
                return state.WhiteMana > state.BlackMana ? AID.Verflare : AID.Verholy;

            if (state.ComboLastMove == AID.Zwerchhau && state.MinMana >= 15)
                return state.BestRedoublement;

            if (state.ComboLastMove == AID.Riposte && state.MinMana >= 30)
                return state.BestZwerchhau;

            if (strategy.NumMoulinetTargets >= 3)
            {
                if (state.MinMana >= 60 - (state.ManaStacks * 20))
                    return state.BestMoulinet;
            }
            else
            {
                if (state.MinMana >= 50 - (state.ManaStacks * 15) && state.CanMelee && state.DualcastLeft == 0)
                    return state.BestRiposte;
            }

            var canStone = state.VerstoneReadyLeft >= state.GetSlidecastTime(AID.Verstone);
            var canFire = state.VerfireReadyLeft >= state.GetSlidecastTime(AID.Verfire);

            if (state.DualcastLeft > state.GCD)
            {
                if (strategy.NumAOETargets >= 2)
                    return state.BestScatter;

                if (canStone == canFire)
                    return state.BlackMana > state.WhiteMana ? state.BestAero : state.BestThunder;

                // prevent overwriting procs
                if (canStone)
                    return state.BestThunder;

                if (canFire)
                    return state.BestAero;
            }

            if (state.SwiftcastLeft > state.GCD || state.AccelerationLeft > state.GCD)
            {
                if (strategy.NumAOETargets >= 2)
                    return state.BestScatter;

                return state.WhiteMana > state.BlackMana ? state.BestAero : state.BestThunder;
            }

            var fastCast = state.GetSlidecastTime(AID.Jolt);
            var canCast = CanCast(state, strategy, fastCast);

            if (canCast)
            {
                if (strategy.NumAOETargets >= 3)
                    return state.BlackMana > state.WhiteMana ? AID.VeraeroII : AID.VerthunderII;

                if (canStone && canFire)
                    return state.BlackMana > state.WhiteMana ? AID.Verstone : AID.Verfire;
                else if (canStone)
                    return AID.Verstone;
                else if (canFire)
                    return AID.Verfire;

                return state.BestJolt;
            }

            return AID.None;
        }

        public static ActionID GetNextBestOGCD(State state, Strategy strategy, float deadline)
        {
            if (strategy.CombatTimer < 0)
                return new();

            if (state.CanWeave(CDGroup.Swiftcast, 0.6f, deadline))
                return ActionID.MakeSpell(AID.Swiftcast);

            if (state.AccelerationLeft < state.GCD && state.CanWeave(CDGroup.Acceleration, 0.6f, deadline))
                return ActionID.MakeSpell(AID.Acceleration);

            if (state.AccelerationLeft < state.GCD && state.InstantCastLeft < state.GCD)
            {
                if (state.CanWeave(CDGroup.Embolden, 0.6f, deadline))
                    return ActionID.MakeSpell(AID.Embolden);

                if (state.CanWeave(CDGroup.Manafication, 0.6f, deadline))
                    return ActionID.MakeSpell(AID.Manafication);
            }

            if (state.CD(CDGroup.Manafication) > 0)
            {
                if (state.CanWeave(CDGroup.Fleche, 0.6f, deadline))
                    return ActionID.MakeSpell(AID.Fleche);

                if (state.CanWeave(CDGroup.ContreSixte, 0.6f, deadline))
                    return ActionID.MakeSpell(AID.ContreSixte);

                if (state.CanMelee)
                {
                    if (state.CanWeave(CDGroup.CorpsACorps, 0.6f, deadline))
                        return ActionID.MakeSpell(AID.CorpsACorps);

                    if (state.CanWeave(CDGroup.Engagement, 0.6f, deadline))
                        return ActionID.MakeSpell(AID.Engagement);

                    if (state.CanWeave(state.CD(CDGroup.CorpsACorps) - 35, 0.6f, deadline))
                        return ActionID.MakeSpell(AID.CorpsACorps);

                    if (state.CanWeave(state.CD(CDGroup.Engagement) - 35, 0.6f, deadline))
                        return ActionID.MakeSpell(AID.Engagement);
                }
            }

            if (state.CurMP <= 7000 && state.CanWeave(CDGroup.LucidDreaming, 0.6f, deadline))
                return ActionID.MakeSpell(AID.LucidDreaming);

            return new();
        }
    }
}
