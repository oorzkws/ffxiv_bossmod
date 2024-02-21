using System;

namespace BossMod.DRK
{
    public static class Rotation
    {
        // 200 MP regen per tick in combat
        const int MP_OVERCAP_THRESHOLD = 9800;

        public class State(float[] cooldowns) : CommonRotation.PlayerState(cooldowns)
        {
            public bool HaveDarkArts;
            public byte Blood; // max 100
            public float DarksideLeft; // max 60s
            public float ShadowLeft; // max 20s
            public float SaltedEarthLeft; // max 15s
            public (float Left, int Stacks) Delirium;
            public (float Left, int Stacks) BloodWeapon;

            public AID BestFlood => Unlocked(AID.FloodOfShadow) ? AID.FloodOfShadow : AID.FloodOfDarkness;
            public AID BestEdge => Unlocked(AID.EdgeOfShadow) ? AID.EdgeOfShadow : AID.EdgeOfDarkness;

            public AID BestSalt =>
                Unlocked(AID.SaltAndDarkness) && SaltedEarthLeft > 0 ? AID.SaltAndDarkness : AID.SaltedEarth;

            public AID ComboLastMove => (AID)ComboLastAction;

            public int ImminentBloodGain =>
                (BloodWeapon.Stacks > 0 ? 10 : 0) + (ComboLastMove is AID.SyphonStrike or AID.Unleash ? 20 : 0);

            public bool Unlocked(AID aid) => Definitions.Unlocked(aid, Level, UnlockProgress);

            public bool Unlocked(TraitID tid) => Definitions.Unlocked(tid, Level, UnlockProgress);

            public override string ToString()
            {
                return $"DS={DarksideLeft:f3}, B={Blood}, MP={CurMP}, Shadow={ShadowLeft:f3}, D={Delirium.Stacks}/{Delirium.Left:f3}, BW={BloodWeapon.Stacks}/{BloodWeapon.Left}, GCD={GCD:f3}";
            }
        }

        public class Strategy : CommonRotation.Strategy
        {
            public int NumAOETargets;
            public int NumFloodTargets;
            public int NumSHBTargets;
            public int NumSaltTargets;
            public int NumDrainTargets;

            public enum MPUse : uint
            {
                // use Edge or Flood of Darkness/Shadow if
                // 1. darkside gauge is empty
                // 2. in raid buff window
                // 3. over ~9000 MP (prevent overcap)
                Automatic = 0,

                // same as above, save MP for TBN
                AutomaticTBN = 1,

                // spend MP as soon as possible
                Force = 2,

                // same as above, save MP for TBN
                ForceTBN = 3,

                // only spend to prevent overcap
                PreventOvercap = 4,

                // don't use it at all
                Delay = 5,
            }

            public MPUse MPStrategy;
            public OffensiveAbilityUse BloodUse;
            public OffensiveAbilityUse BloodWeaponUse;
            public OffensiveAbilityUse DeliriumUse;
            public OffensiveAbilityUse SaltedEarthUse;
            public OffensiveAbilityUse CarveUse;
            public OffensiveAbilityUse ShadowUse;

            public enum PlungeUse : uint
            {
                // use during 2min burst if in melee range
                Automatic = 0,

                // don't use
                Delay = 1,

                // use on cooldown
                Force = 2,

                // use on cooldown, save one charge
                ForceReserve = 3,

                // use if not in melee range
                GapCloser = 4,
            }

            public PlungeUse PlungeStrategy;

            public void ApplyStrategyOverrides(uint[] overrides, bool defaultToAutoTBN)
            {
                if (overrides.Length >= 8)
                {
                    MPStrategy = (MPUse)overrides[0];
                    BloodUse = (OffensiveAbilityUse)overrides[1];
                    BloodWeaponUse = (OffensiveAbilityUse)overrides[2];
                    DeliriumUse = (OffensiveAbilityUse)overrides[3];
                    SaltedEarthUse = (OffensiveAbilityUse)overrides[4];
                    CarveUse = (OffensiveAbilityUse)overrides[5];
                    ShadowUse = (OffensiveAbilityUse)overrides[6];
                    PlungeStrategy = (PlungeUse)overrides[7];
                }
                else
                {
                    MPStrategy = defaultToAutoTBN ? MPUse.AutomaticTBN : MPUse.Automatic;
                    BloodUse = OffensiveAbilityUse.Automatic;
                    BloodWeaponUse = OffensiveAbilityUse.Automatic;
                    DeliriumUse = OffensiveAbilityUse.Automatic;
                    SaltedEarthUse = OffensiveAbilityUse.Automatic;
                    CarveUse = OffensiveAbilityUse.Automatic;
                    ShadowUse = OffensiveAbilityUse.Automatic;
                    PlungeStrategy = PlungeUse.Automatic;
                }
            }

            public override string ToString()
            {
                var savetbn = (MPStrategy is MPUse.AutomaticTBN or MPUse.ForceTBN) ? "Reserve" : "None";
                return $"AOE={NumAOETargets}/SE {NumSaltTargets}/SH {NumSHBTargets}/FL {NumFloodTargets}/DR {NumDrainTargets}, TBN={savetbn}";
            }
        }

        public static AID GetNextBestGCD(State state, Strategy strategy)
        {
            if (strategy.CombatTimer > -100 && strategy.CombatTimer < -0.7f)
                return AID.None;

            if (CanBlood(state, strategy))
            {
                if (strategy.NumAOETargets >= 3 && state.Unlocked(AID.Quietus))
                    return AID.Quietus;

                if (state.TargetingEnemy)
                    return AID.Bloodspiller;
            }

            if (strategy.NumAOETargets >= 2 && state.Unlocked(AID.Unleash))
            {
                if (state.ComboLastMove == AID.Unleash && state.Unlocked(AID.StalwartSoul))
                    return AID.StalwartSoul;

                return AID.Unleash;
            }
            else if (state.TargetingEnemy)
            {
                if (state.ComboLastMove == AID.SyphonStrike && state.Unlocked(AID.Souleater))
                    return AID.Souleater;

                if (state.ComboLastMove == AID.HardSlash && state.Unlocked(AID.SyphonStrike))
                    return AID.SyphonStrike;

                return AID.HardSlash;
            }

            return AID.None;
        }

        public static ActionID GetNextBestOGCD(State state, Strategy strategy, float deadline, bool lastSlot)
        {
            if (strategy.CombatTimer > -100 && strategy.CombatTimer < 0)
            {
                if (strategy.CombatTimer > -4 && state.BloodWeapon.Left == 0 && state.Unlocked(AID.BloodWeapon))
                    return ActionID.MakeSpell(AID.BloodWeapon);

                return new();
            }

            var bestEdge = AID.None;
            if (state.Unlocked(AID.EdgeOfDarkness))
                bestEdge = strategy.NumFloodTargets >= 3 ? state.BestFlood : state.BestEdge;
            else if (strategy.NumFloodTargets > 0)
                bestEdge = AID.FloodOfDarkness;

            var canEdge =
                bestEdge != AID.None
                && CanEdge(state, strategy)
                && state.Unlocked(AID.FloodOfDarkness)
                && state.CanWeave(CDGroup.FloodOfDarkness, 0.6f, deadline);

            var useBurst = ShouldUseBurst(state, strategy);

            if (canEdge && state.DarksideLeft < state.GCD)
                return ActionID.MakeSpell(bestEdge);

            if (
                state.Unlocked(AID.BloodWeapon)
                && state.CanWeave(CDGroup.BloodWeapon, 0.6f, deadline)
                && ShouldUseBloodWeapon(state, strategy)
            )
            {
                return ActionID.MakeSpell(AID.BloodWeapon);
            }

            if (
                state.Unlocked(AID.Delirium)
                && state.CanWeave(CDGroup.Delirium, 0.6f, deadline)
                && ShouldUseDelirium(state, strategy)
            )
                return ActionID.MakeSpell(AID.Delirium);

            if (
                state.Unlocked(AID.LivingShadow)
                && state.Blood >= 50
                && state.CanWeave(CDGroup.LivingShadow, 0.6f, deadline)
                && strategy.ShadowUse != CommonRotation.Strategy.OffensiveAbilityUse.Delay
            )
                return ActionID.MakeSpell(AID.LivingShadow);

            // TODO fix this dollar store raid buff alignment
            if (strategy.CombatTimer < state.AttackGCDTime * 3)
                return new();

            // intentionally checking for full plunge charges
            if (
                state.Unlocked(AID.Plunge)
                && ShouldUsePlunge(state, strategy)
                && state.CD(CDGroup.AbyssalDrain) > 0
                && state.CanWeave(CDGroup.Plunge, 0.6f, deadline)
            )
                return ActionID.MakeSpell(AID.Plunge);

            if (canEdge && lastSlot)
                return ActionID.MakeSpell(bestEdge);

            if (
                state.Unlocked(AID.SaltedEarth)
                && state.CanWeave(CDGroup.SaltedEarth, 0.6f, deadline)
                && strategy.NumSaltTargets > 0
            )
                return ActionID.MakeSpell(AID.SaltedEarth);

            if (
                state.Unlocked(AID.Shadowbringer)
                && useBurst
                && state.CanWeave(CDGroup.Shadowbringer, 0.6f, deadline)
                && strategy.NumSHBTargets > 0
            )
                return ActionID.MakeSpell(AID.Shadowbringer);

            if (state.CanWeave(CDGroup.AbyssalDrain, 0.6f, deadline) && state.Unlocked(AID.AbyssalDrain))
            {
                if (strategy.NumDrainTargets >= 3 || !state.Unlocked(AID.CarveAndSpit))
                    return ActionID.MakeSpell(AID.AbyssalDrain);

                return ActionID.MakeSpell(AID.CarveAndSpit);
            }

            if (
                state.Unlocked(AID.Shadowbringer)
                && useBurst
                && state.CanWeave(state.CD(CDGroup.Shadowbringer) - 60, 0.6f, deadline)
                && strategy.NumSHBTargets > 0
            )
                return ActionID.MakeSpell(AID.Shadowbringer);

            if (
                state.SaltedEarthLeft > state.AnimationLock
                && state.Unlocked(AID.SaltAndDarkness)
                && state.CanWeave(CDGroup.SaltAndDarkness, 0.6f, deadline)
                && strategy.NumSaltTargets > 0
            )
                return ActionID.MakeSpell(AID.SaltAndDarkness);

            if (canEdge)
                return ActionID.MakeSpell(bestEdge);

            if (
                state.Unlocked(AID.Plunge)
                && ShouldUsePlunge(state, strategy)
                && state.CanWeave(state.CD(CDGroup.Plunge) - 30, 0.6f, deadline)
            )
                return ActionID.MakeSpell(AID.Plunge);

            return new();
        }

        private static bool CanEdge(State state, Strategy strategy)
        {
            if (state.HaveDarkArts)
                return true;

            var minimumMP = 3000;
            if (
                state.Unlocked(AID.TheBlackestNight)
                && strategy.MPStrategy is Strategy.MPUse.AutomaticTBN or Strategy.MPUse.ForceTBN
            )
                minimumMP = 6000;

            switch (strategy.MPStrategy)
            {
                case Strategy.MPUse.Automatic:
                case Strategy.MPUse.AutomaticTBN:
                    if (ShouldUseBurst(state, strategy) || state.DarksideLeft < state.GCD)
                        return state.CurMP >= minimumMP;
                    return state.CurMP >= MP_OVERCAP_THRESHOLD;
                case Strategy.MPUse.Force:
                case Strategy.MPUse.ForceTBN:
                    return state.CurMP >= minimumMP;
                case Strategy.MPUse.PreventOvercap:
                    return state.CurMP >= MP_OVERCAP_THRESHOLD;
                case Strategy.MPUse.Delay:
                    return false;
                default:
                    return false;
            }
        }

        private static bool CanBlood(State state, Strategy strategy)
        {
            if (state.Blood < 50 && state.Delirium.Left == 0)
                return false;

            var waitForShadow =
                state.Unlocked(AID.LivingShadow) && state.CD(CDGroup.LivingShadow) <= state.AttackGCDTime * 4;

            switch (strategy.BloodUse)
            {
                case CommonRotation.Strategy.OffensiveAbilityUse.Force:
                    return true;
                case CommonRotation.Strategy.OffensiveAbilityUse.Delay:
                    return false;
                case CommonRotation.Strategy.OffensiveAbilityUse.Automatic:
                    if (ShouldUseBurst(state, strategy) && !waitForShadow)
                        return true;
                    if (
                        MathF.Min(state.CD(CDGroup.BloodWeapon), state.CD(CDGroup.Delirium))
                            < state.GCD + state.AttackGCDTime
                        && !waitForShadow
                    )
                        return true;
                    return state.Blood + state.ImminentBloodGain > 100;
                default:
                    return false;
            }
        }

        private static bool ShouldUseBurst(State state, Strategy strategy) =>
            state.RaidBuffsLeft > state.AnimationLock || strategy.RaidBuffsIn > 9000;

        private static bool ShouldUseBloodWeapon(State state, Strategy strategy) =>
            strategy.BloodWeaponUse switch
            {
                CommonRotation.Strategy.OffensiveAbilityUse.Automatic => state.Blood < 50 || state.Delirium.Left > 0,
                CommonRotation.Strategy.OffensiveAbilityUse.Force => true,
                _ => false,
            };

        private static bool ShouldUseDelirium(State state, Strategy strategy) =>
            strategy.DeliriumUse switch
            {
                CommonRotation.Strategy.OffensiveAbilityUse.Automatic => state.Blood < 50 || state.BloodWeapon.Left > 0,
                CommonRotation.Strategy.OffensiveAbilityUse.Force => true,
                _ => false,
            };

        private static bool ShouldUsePlunge(State state, Strategy strategy)
        {
            var inMelee = state.RangeToTarget <= 3;

            return strategy.PlungeStrategy switch
            {
                Strategy.PlungeUse.Automatic => inMelee && ShouldUseBurst(state, strategy),
                Strategy.PlungeUse.Delay => false,
                Strategy.PlungeUse.Force => inMelee,
                Strategy.PlungeUse.ForceReserve => inMelee && state.CD(CDGroup.Plunge) == 0, // plunge only has 2 charges
                Strategy.PlungeUse.GapCloser => !inMelee,
                _ => false,
            };
        }
    }
}
