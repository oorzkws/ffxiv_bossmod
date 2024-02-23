using System;
using System.Linq;
using Dalamud.Game.ClientState.JobGauge.Enums;

namespace BossMod.AST
{
    public static class Rotation
    {
        public enum Card : byte
        {
            None = 0,
            Balance = 1,
            Bole = 2,
            Arrow = 3,
            Spear = 4,
            Ewer = 5,
            Spire = 6
        }

        public enum CrownCard : byte
        {
            None = 0,
            Lord = 112,
            Lady = 128
        }

        public class State : CommonRotation.PlayerState
        {
            public float SwiftcastLeft; // 10s max
            public float LightspeedLeft; // 15s max

            public float StarLeft; // 20s max
            public float MacrocosmosLeft; // 15s max
            public float HoroscopeLeft; // 10s max
            public float HoroHeliosLeft; // 30s max
            public float AstrodyneLeft; // 15s max
            public float DivinationLeft; // 15s max

            public float TargetCombustLeft; // 30s max

            public Card DrawnCard;
            public CrownCard DrawnCrownCard;
            public SealType[] Seals = [];

            public bool IsClarified;

            public bool HasCard => DrawnCard > 0;
            public bool HasCrown => DrawnCrownCard > 0;

            // "ok" = can we play card without overcapping seals or getting a triple seal which results in a shitty astrodyne
            public bool IsCardGood =>
                SealCount switch
                {
                    0 => true,
                    1 => NextSeal != Seals[0],
                    2 => NextSeal != Seals[0] || NextSeal != Seals[1],
                    _ => false
                };

            public State(float[] cooldowns)
                : base(cooldowns) { }

            public SealType NextSeal =>
                DrawnCard switch
                {
                    Card.Balance or Card.Bole => SealType.SUN,
                    Card.Arrow or Card.Ewer => SealType.MOON,
                    Card.Spear or Card.Spire => SealType.CELESTIAL,
                    _ => SealType.NONE,
                };
            public int SealCount => Seals.Count(x => x > 0);

            public AID BestPlay =>
                DrawnCard switch
                {
                    Card.Arrow => AID.TheArrow,
                    Card.Balance => AID.TheBalance,
                    Card.Bole => AID.TheBole,
                    Card.Ewer => AID.TheEwer,
                    Card.Spear => AID.TheSpear,
                    Card.Spire => AID.TheSpire,
                    _ => AID.Play
                };

            public AID BestCrown =>
                DrawnCrownCard switch
                {
                    CrownCard.Lord => AID.LordOfCrowns,
                    CrownCard.Lady => AID.LadyOfCrowns,
                    _ => AID.MinorArcana
                };

            public AID BestStar => StarLeft > 0 ? AID.StellarDetonation : AID.EarthlyStar;
            public AID BestCosmos => MacrocosmosLeft > 0 ? AID.Microcosmos : AID.Macrocosmos;
            public AID BestHoroscope => HoroscopeLeft > 0 || HoroHeliosLeft > 0 ? AID.HoroscopeEnd : AID.Horoscope;

            public AID BestMalefic =>
                Unlocked(AID.FallMalefic)
                    ? AID.FallMalefic
                    : Unlocked(AID.MaleficIV)
                        ? AID.MaleficIV
                        : Unlocked(AID.MaleficIII)
                            ? AID.MaleficIII
                            : Unlocked(AID.MaleficII)
                                ? AID.MaleficII
                                : AID.Malefic;
            public AID BestCombust =>
                Unlocked(AID.CombustIII)
                    ? AID.CombustIII
                    : Unlocked(AID.CombustII)
                        ? AID.CombustII
                        : AID.Combust;

            public AID BestGravity => Unlocked(AID.GravityII) ? AID.GravityII : AID.Gravity;
            public SID ExpectedCombust =>
                Unlocked(AID.CombustIII)
                    ? SID.CombustIII
                    : Unlocked(AID.CombustII)
                        ? SID.CombustII
                        : SID.Combust;

            public bool Unlocked(AID aid) => Definitions.Unlocked(aid, Level, UnlockProgress);

            public bool Unlocked(TraitID tid) => Definitions.Unlocked(tid, Level, UnlockProgress);

            public override string ToString()
            {
                return $"Draw={CD(CDGroup.Draw)}, Seals=[{Seals[0]},{Seals[1]},{Seals[2]}]";
            }
        }

        public class Strategy : CommonRotation.Strategy
        {
            public int NumGravityTargets; // 5y around target
            public int NumBigAOETargets; // 20y around self, for microcosmos and lord of crowns
            public int NumStarTargets; // 20y around earthly star location

            public bool HasRaidplan => RaidBuffsIn < 1000;

            public OffensiveAbilityUse DivinationUse;
            public OffensiveAbilityUse LightspeedUse;
            public OffensiveAbilityUse CardUse;

            public void ApplyStrategyOverrides(uint[] overrides)
            {
                if (overrides.Length >= 3)
                {
                    DivinationUse = (OffensiveAbilityUse)overrides[0];
                    LightspeedUse = (OffensiveAbilityUse)overrides[1];
                    CardUse = (OffensiveAbilityUse)overrides[2];
                }
                else
                {
                    DivinationUse = LightspeedUse = CardUse = OffensiveAbilityUse.Automatic;
                }
            }

            public override string ToString()
            {
                return $"AOE={NumGravityTargets}/{NumBigAOETargets}, Star={NumStarTargets}, C={CombatTimer:f2}";
            }
        }

        public static bool CanCast(State state, Strategy strategy, float castTime)
        {
            if (state.LightspeedLeft > state.GCD)
                castTime -= 2.5f;
            
            return MathF.Max(0, castTime) <= strategy.ForceMovementIn;
        }

        public static AID GetNextBestGCD(State state, Strategy strategy)
        {
            if (!state.TargetingEnemy) return AID.None;

            if (strategy.CombatTimer < 0)
            {
                if (strategy.CombatTimer > -1.5)
                    return AID.FallMalefic;

                return AID.None;
            }

            var canCast = CanCast(state, strategy, state.SpellGCDTime * 0.6f);

            if (canCast && state.Unlocked(AID.Gravity) && strategy.NumGravityTargets >= 2)
                return state.BestGravity;

            if (state.TargetCombustLeft <= state.SpellGCDTime && state.Unlocked(AID.Combust))
                return state.BestCombust;

            if (canCast)
                return state.BestMalefic;

            return AID.None;
        }

        public static ActionID GetNextBestOGCD(State state, Strategy strategy, float deadline, bool lastSlot)
        {
            if (ShouldDraw(state, strategy) && state.CanWeave(state.CD(CDGroup.Draw) - 30, 0.6f, deadline))
                return ActionID.MakeSpell(AID.Draw);

            if (
                state.CurMP <= 7000
                && state.Unlocked(AID.LucidDreaming)
                && state.CanWeave(CDGroup.LucidDreaming, 0.6f, deadline)
            )
                return ActionID.MakeSpell(AID.LucidDreaming);

            if (
                state.HasCard
                && !state.IsCardGood
                && state.IsClarified
                && state.CanWeave(CDGroup.Redraw, 0.6f, deadline)
            )
                return ActionID.MakeSpell(AID.Redraw);

            if (strategy.CombatTimer < 0)
            {
                if (
                    strategy.CombatTimer > -1
                    && state.Unlocked(AID.Lightspeed)
                    && state.CanWeave(CDGroup.Lightspeed, 0.6f, deadline)
                )
                    return ActionID.MakeSpell(AID.Lightspeed);

                return new();
            }

            if (ShouldLightspeed(state, strategy) && state.CanWeave(CDGroup.Lightspeed, 0.6f, deadline))
                return ActionID.MakeSpell(AID.Lightspeed);

            if (ShouldDivination(state, strategy) && state.CanWeave(CDGroup.Divination, 0.6f, deadline))
                return ActionID.MakeSpell(AID.Divination);

            if (ShouldPlay(state, strategy) && state.CanWeave(CDGroup.Play, 0.6f, deadline))
                return ActionID.MakeSpell(state.BestPlay);

            if (state.SealCount == 3 && state.CanWeave(CDGroup.Astrodyne, 0.6f, deadline))
                return ActionID.MakeSpell(AID.Astrodyne);

            if (
                (state.SealCount == 3 || state.AstrodyneLeft > 0)
                && !state.HasCrown
                && state.Unlocked(AID.MinorArcana)
                && state.CanWeave(CDGroup.MinorArcana, 0.6f, deadline)
            )
                return ActionID.MakeSpell(AID.MinorArcana);

            if (
                state.AstrodyneLeft > 0
                && state.DrawnCrownCard is CrownCard.Lord
                && state.CanWeave(CDGroup.LadyOfCrowns, 0.6f, deadline)
            )
                return ActionID.MakeSpell(AID.LordOfCrowns);

            return new();
        }

        private static bool ShouldPlay(State state, Strategy strategy)
        {
            if (!state.HasCard || !state.IsCardGood || !state.TargetingEnemy)
                return false;

            if (!strategy.HasRaidplan)
                return true;

            if (strategy.CombatTimer % 120 < 20)
                return true;
            else if (strategy.CombatTimer % 60 < 20)
                return state.SealCount == 0;

            return false;
        }

        private static bool ShouldDraw(State state, Strategy strategy) =>
            state.Unlocked(AID.Draw)
            && !state.HasCard
            && strategy.CardUse != CommonRotation.Strategy.OffensiveAbilityUse.Delay;

        private static bool ShouldLightspeed(State state, Strategy strategy)
        {
            if (
                !state.Unlocked(AID.Lightspeed)
                || strategy.LightspeedUse == CommonRotation.Strategy.OffensiveAbilityUse.Delay
                || !state.TargetingEnemy
            )
                return false;
            if (
                !state.Unlocked(AID.Divination)
                || strategy.LightspeedUse == CommonRotation.Strategy.OffensiveAbilityUse.Force
            )
                return true;

            return state.CD(CDGroup.Divination) < 5 || state.DivinationLeft > 0;
        }

        private static bool ShouldDivination(State state, Strategy strategy) =>
            strategy.DivinationUse != CommonRotation.Strategy.OffensiveAbilityUse.Delay
            && state.Unlocked(AID.Divination)
            && state.TargetingEnemy
            && strategy.CombatTimer >= state.SpellGCDTime * 2;
    }
}
