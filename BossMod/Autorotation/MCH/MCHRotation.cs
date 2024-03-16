namespace BossMod.MCH
{
    public static class Rotation
    {
        public class State(WorldState ws) : CommonRotation.PlayerState(ws)
        {
            public int Heat; // 100 max
            public int Battery; // 100 max
            public float OverheatLeft; // 10s max
            public float ReassembleLeft; // 5s max
            public float WildfireLeft; // 10s max
            public float PelotonLeft;
            public bool UsingFlamethrower;
            public bool IsOverheated;
            public bool HasMinion;

            public AID ComboLastMove => ComboTimeLeft > GCD ? (AID)ComboLastAction : AID.None;

            public AID BestSplitShot => Unlocked(AID.HeatedSplitShot) ? AID.HeatedSplitShot : AID.SplitShot;
            public AID BestSlugShot => Unlocked(AID.HeatedSlugShot) ? AID.HeatedSlugShot : AID.SlugShot;
            public AID BestCleanShot => Unlocked(AID.HeatedCleanShot) ? AID.HeatedCleanShot : AID.CleanShot;
            public AID BestHotShot => Unlocked(AID.AirAnchor) ? AID.AirAnchor : AID.HotShot;

            public float FullGaussCD => CD(CDGroup.GaussRound) - (Unlocked(TraitID.ChargedActionMastery) ? 0 : 30);
            public float FullRicochetCD => CD(CDGroup.Ricochet) - (Unlocked(TraitID.ChargedActionMastery) ? 0 : 30);

            public bool Unlocked(AID aid) => Definitions.Unlocked(aid, Level, UnlockProgress);

            public bool Unlocked(TraitID tid) => Definitions.Unlocked(tid, Level, UnlockProgress);

            public override string ToString()
            {
                return $"RB={RaidBuffsLeft:f1}, PotCD={PotionCD:f1}, act={ComboLastMove}, GCD={GCD:f3}, ALock={AnimationLock:f3}+{AnimationLockDelay:f3}, lvl={Level}/{UnlockProgress}";
            }
        }

        public class Strategy : CommonRotation.Strategy
        {
            public int NumAOETargets; // 12y/90deg cone for scattergun, bioblaster, auto crossbow
            public int NumFlamethrowerTargets; // 8y/90deg cone
            public int NumChainsawTargets; // 25/4y rect
            public int NumRicochetTargets; // 5y circle around target

            public void ApplyStrategyOverrides(uint[] overrides) { }
        }

        public static AID GetNextBestGCD(State state, Strategy strategy)
        {
            if (FlamethrowerPause(state, strategy))
                return AID.None;

            if (state.IsOverheated)
            {
                if (strategy.NumAOETargets > 2 && state.Unlocked(AID.AutoCrossbow))
                    return AID.AutoCrossbow;

                if (state.Unlocked(AID.HeatBlast))
                    return AID.HeatBlast;
            }

            var canHotShot =
                !state.Unlocked(AID.AirAnchor) && state.Unlocked(AID.HotShot) && state.CD(CDGroup.HotShot) <= state.GCD;

            if (ShouldUseAirAnchor(state, strategy))
                return AID.AirAnchor;

            if (ShouldUseChainsaw(state, strategy))
                return AID.ChainSaw;

            if (state.Unlocked(AID.Bioblaster) && state.CD(CDGroup.Drill) <= state.GCD && strategy.NumAOETargets > 1)
                return AID.Bioblaster;

            if (ShouldUseDrill(state, strategy))
                return AID.Drill;

            if (state.ReassembleLeft > state.GCD)
            {
                if (strategy.NumAOETargets > 3)
                    return AID.Scattergun;

                if (state.Unlocked(AID.CleanShot) && state.ComboLastMove == AID.SlugShot)
                    return AID.CleanShot;

                if (canHotShot)
                    return AID.HotShot;
            }

            if ((!state.Unlocked(AID.Reassemble) || state.ReassembleLeft == 0) && canHotShot)
                return AID.HotShot;

            if (strategy.NumAOETargets > 2)
            {
                if (!state.IsOverheated && strategy.NumFlamethrowerTargets >= 3 && state.Unlocked(AID.Flamethrower) && state.CD(CDGroup.Flamethrower) < state.GCD)
                    return AID.Flamethrower;

                if (state.Unlocked(AID.Scattergun))
                    return AID.Scattergun;

                if (state.Unlocked(AID.SpreadShot))
                    return AID.SpreadShot;
            }

            if (state.ComboLastMove == AID.SlugShot && state.Unlocked(AID.CleanShot))
                return state.BestCleanShot;

            if (state.ComboLastMove == AID.SplitShot && state.Unlocked(AID.SlugShot))
                return state.BestSlugShot;

            return state.BestSplitShot;
        }

        public static ActionID GetNextBestOGCD(State state, Strategy strategy, float deadline)
        {
            if (FlamethrowerPause(state, strategy))
                return new();

            // check for full charges
            if (state.Unlocked(AID.GaussRound) && state.CanWeave(state.FullGaussCD, 0.6f, deadline))
                return ActionID.MakeSpell(AID.GaussRound);

            if (state.Unlocked(AID.Ricochet) && state.CanWeave(state.FullRicochetCD, 0.6f, deadline))
                return ActionID.MakeSpell(AID.Ricochet);

            if (state.CD(CDGroup.Drill) > 0 && state.CanWeave(CDGroup.BarrelStabilizer, 0.6f, deadline))
                return ActionID.MakeSpell(AID.BarrelStabilizer);

            if (ShouldUseBurst(state, strategy, deadline))
            {
                if (
                    ShouldUseReassemble(state, strategy)
                    && state.CanWeave(state.CD(CDGroup.Reassemble) - 55, 0.6f, deadline)
                )
                    return ActionID.MakeSpell(AID.Reassemble);

                if (
                    (!state.Unlocked(AID.AirAnchor) || state.CD(CDGroup.AirAnchor) > 0)
                    && state.CanWeave(CDGroup.Wildfire, 0.6f, deadline)
                    && state.Heat >= 50
                    && state.WildfireLeft == 0
                )
                    return ActionID.MakeSpell(AID.Wildfire);

                if (state.Battery >= 50 && !state.HasMinion && state.CanWeave(CDGroup.RookAutoturret, 0.6f, deadline))
                {
                    if (state.CD(CDGroup.Wildfire) > 0 && state.Unlocked(AID.AutomatonQueen))
                        return ActionID.MakeSpell(AID.AutomatonQueen);

                    if (state.CD(CDGroup.Wildfire) > 0 || !state.Unlocked(AID.Wildfire))
                        return ActionID.MakeSpell(AID.RookAutoturret);
                }

                if (ShouldUseHypercharge(state, strategy) && state.CanWeave(CDGroup.Hypercharge, 0.6f, deadline))
                    return ActionID.MakeSpell(AID.Hypercharge);

                var rcd = state.CD(CDGroup.Ricochet) - 60;
                var grcd = state.CD(CDGroup.GaussRound) - 60;
                var canRcd = state.Unlocked(AID.Ricochet) && state.CanWeave(rcd, 0.6f, deadline);
                var canGrcd = state.Unlocked(AID.GaussRound) && state.CanWeave(grcd, 0.6f, deadline);

                if (state.IsOverheated)
                {
                    if (canRcd && canGrcd)
                        return ActionID.MakeSpell(rcd > grcd ? AID.GaussRound : AID.Ricochet);
                    else if (canRcd)
                        return ActionID.MakeSpell(AID.Ricochet);
                    else if (canGrcd)
                        return ActionID.MakeSpell(AID.GaussRound);
                }
            }

            return new();
        }

        private static bool ShouldUseReassemble(State state, Strategy strategy)
        {
            if (
                state.ReassembleLeft > 0
                || !state.Unlocked(AID.Reassemble)
                || state.RangeToTarget > 25
                || state.OverheatLeft > 0
            )
                return false;

            // scattergun priority
            if (strategy.NumAOETargets > 3 && state.Unlocked(AID.SpreadShot))
                return true;

            var waitOpener = strategy.CombatTimer > 10 || state.ComboLastMove == AID.CleanShot;

            return state.Level switch
            {
                < 26 => state.CD(CDGroup.HotShot) <= state.GCD,
                < 58 => state.ComboLastMove == AID.SlugShot,
                < 76 => state.CD(CDGroup.Drill) <= state.GCD,
                < 90 => waitOpener && state.CD(CDGroup.AirAnchor) <= state.GCD,
                _ => waitOpener && (state.CD(CDGroup.ChainSaw) <= state.GCD || state.CD(CDGroup.AirAnchor) <= state.GCD)
            };
        }

        private static bool ShouldUseHypercharge(State state, Strategy strategy)
        {
            if (state.Heat < 50 || state.IsOverheated || state.RangeToTarget > 25 || state.ReassembleLeft > 0)
                return false;

            // if (state.Heat == 100)
            //     return true;

            var waitForWildfire = state.Unlocked(AID.Wildfire) && state.CD(CDGroup.Wildfire) <= state.GCD;
            var waitForAnchor = state.Unlocked(AID.AirAnchor) && state.CD(CDGroup.AirAnchor) <= state.GCD;
            var waitForSaw = state.Unlocked(AID.ChainSaw) && state.CD(CDGroup.ChainSaw) <= state.GCD;

            if (waitForWildfire || waitForAnchor || waitForSaw)
                return false;

            return true;
        }

        private static bool ShouldUseAirAnchor(State state, Strategy strategy)
        {
            if (
                !state.Unlocked(AID.AirAnchor)
                || state.CD(CDGroup.AirAnchor) > state.GCD
                || (state.CD(CDGroup.Reassemble) < 55 && state.ReassembleLeft == 0)
            )
                return false;

            if (strategy.CombatTimer < 10)
                return state.ReassembleLeft > state.GCD;

            return true;
        }

        private static bool ShouldUseChainsaw(State state, Strategy strategy)
        {
            if (
                !state.Unlocked(AID.ChainSaw)
                || state.CD(CDGroup.ChainSaw) > state.GCD
                || (state.CD(CDGroup.Reassemble) < 55 && state.ReassembleLeft == 0)
            )
                return false;

            if (strategy.CombatTimer < 10)
                return state.ReassembleLeft > state.GCD;

            return true;
        }

        private static bool ShouldUseDrill(State state, Strategy strategy)
        {
            if (!state.Unlocked(AID.Drill) || state.CD(CDGroup.Drill) > state.GCD)
                return false;

            // level >= 76: use reassemble on AA and chainsaw
            if (state.Unlocked(AID.AirAnchor))
                return state.ReassembleLeft == 0 && (state.FullGaussCD > 0 || state.FullRicochetCD > 0);

            var reassembleCharge = state.CD(CDGroup.Reassemble) - 55;
            return reassembleCharge > state.AttackGCDTime;
        }

        private static bool ShouldUseBurst(State state, Strategy strategy, float deadline) =>
            state.RaidBuffsLeft >= deadline || strategy.RaidBuffsIn > 9000;

        private static bool FlamethrowerPause(State state, Strategy strategy) => state.UsingFlamethrower && strategy.NumAOETargets >= 3;
    }
}
