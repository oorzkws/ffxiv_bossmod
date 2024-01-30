namespace BossMod.MCH
{
    public static class Rotation
    {
        public class State : CommonRotation.PlayerState
        {
            public int Heat; // 100 max
            public int Battery; // 100 max
            public float OverheatLeft; // 10s max
            public float ReassembleLeft; // 5s max
            public float WildfireLeft; // 10s max
            public bool IsOverheated;
            public bool HasMinion;

            public State(float[] cooldowns)
                : base(cooldowns) { }

            public AID ComboLastMove => (AID)ComboLastAction;

            public AID BestSplitShot => Unlocked(AID.HeatedSplitShot) ? AID.HeatedSplitShot : AID.SplitShot;
            public AID BestSlugShot => Unlocked(AID.HeatedSlugShot) ? AID.HeatedSlugShot : AID.SlugShot;
            public AID BestCleanShot => Unlocked(AID.HeatedCleanShot) ? AID.HeatedCleanShot : AID.CleanShot;

            public bool Unlocked(AID aid) => Definitions.Unlocked(aid, Level, UnlockProgress);

            public bool Unlocked(TraitID tid) => Definitions.Unlocked(tid, Level, UnlockProgress);

            public override string ToString()
            {
                return $"RB={RaidBuffsLeft:f1}, PotCD={PotionCD:f1}, act={ComboLastMove}, GCD={GCD:f3}, ALock={AnimationLock:f3}+{AnimationLockDelay:f3}, lvl={Level}/{UnlockProgress}";
            }
        }

        public class Strategy : CommonRotation.Strategy
        {
            public void ApplyStrategyOverrides(uint[] overrides) { }
        }

        public static AID GetNextBestGCD(State state, Strategy strategy)
        {
            if (state.IsOverheated)
                return AID.HeatBlast;

            var canHotShot = state.Unlocked(AID.HotShot) && state.CD(CDGroup.HotShot) <= state.GCD;

            if (state.ReassembleLeft > state.GCD)
            {
                if (state.Unlocked(AID.AirAnchor) && state.CD(CDGroup.AirAnchor) <= state.GCD)
                    return AID.AirAnchor;

                if (state.Unlocked(AID.ChainSaw) && state.CD(CDGroup.ChainSaw) <= state.GCD)
                    return AID.ChainSaw;

                if (state.Unlocked(AID.CleanShot) && state.ComboLastMove == AID.SlugShot)
                    return AID.CleanShot;

                if (canHotShot)
                    return AID.HotShot;
            }

            if (!state.Unlocked(AID.Reassemble) && canHotShot)
                return AID.HotShot;

            if (
                state.Unlocked(AID.Drill)
                && state.CD(CDGroup.Drill) <= state.GCD
                && (state.CD(CDGroup.Ricochet) > 0 || state.CD(CDGroup.GaussRound) > 0)
            )
                return AID.Drill;

            if (state.ComboLastMove == AID.SlugShot && state.Unlocked(AID.CleanShot))
                return state.BestCleanShot;

            if (state.ComboLastMove == AID.SplitShot && state.Unlocked(AID.SlugShot))
                return state.BestSlugShot;

            return state.BestSplitShot;
        }

        public static ActionID GetNextBestOGCD(State state, Strategy strategy, float deadline)
        {
            // check for full charges
            if (state.Unlocked(AID.GaussRound) && state.CanWeave(CDGroup.GaussRound, 0.6f, deadline))
                return ActionID.MakeSpell(AID.GaussRound);

            if (state.Unlocked(AID.Ricochet) && state.CanWeave(CDGroup.Ricochet, 0.6f, deadline))
                return ActionID.MakeSpell(AID.Ricochet);

            if (state.CD(CDGroup.Drill) > 0 && state.CanWeave(CDGroup.BarrelStabilizer, 0.6f, deadline))
                return ActionID.MakeSpell(AID.BarrelStabilizer);

            if (ShouldUseBurst(state, strategy, deadline))
            {
                if (
                    ShouldReassemble(state, strategy)
                    && state.CanWeave(state.CD(CDGroup.Reassemble) - 55, 0.6f, deadline)
                // && (state.CD(CDGroup.AirAnchor) <= state.GCD || state.CD(CDGroup.ChainSaw) <= state.GCD)
                )
                    return ActionID.MakeSpell(AID.Reassemble);

                if (
                    state.CD(CDGroup.AirAnchor) > 0
                    && state.CanWeave(CDGroup.Wildfire, 0.6f, deadline)
                    && state.WildfireLeft == 0
                )
                    return ActionID.MakeSpell(AID.Wildfire);

                if (
                    state.WildfireLeft > 0
                    && state.Battery >= 50
                    && !state.HasMinion
                    && state.CanWeave(CDGroup.RookAutoturret, 0.6f, deadline)
                )
                    return ActionID.MakeSpell(AID.AutomatonQueen);

                if (
                    state.CD(CDGroup.Wildfire) > 0
                    && state.CD(CDGroup.AirAnchor) > 0
                    && state.CD(CDGroup.ChainSaw) > 0
                    && state.Heat >= 50
                    && !state.IsOverheated
                    && state.CanWeave(CDGroup.Hypercharge, 0.6f, deadline)
                )
                    return ActionID.MakeSpell(AID.Hypercharge);

                var rcd = state.CD(CDGroup.Ricochet) - 60;
                var grcd = state.CD(CDGroup.GaussRound) - 60;
                var canRcd = state.Unlocked(AID.Ricochet) && state.CanWeave(rcd, 0.6f, deadline);
                var canGrcd = state.Unlocked(AID.GaussRound) && state.CanWeave(grcd, 0.6f, deadline);

                if (canRcd && canGrcd)
                    return ActionID.MakeSpell(rcd > grcd ? AID.GaussRound : AID.Ricochet);
                else if (canRcd)
                    return ActionID.MakeSpell(AID.Ricochet);
                else if (canGrcd)
                    return ActionID.MakeSpell(AID.GaussRound);
            }

            return new();
        }

        private static bool ShouldReassemble(State state, Strategy strategy)
        {
            if (state.ReassembleLeft > 0)
                return false;

            return state.Level switch
            {
                < 26 => state.CD(CDGroup.HotShot) <= state.GCD,
                26 => state.ComboLastMove == AID.SlugShot,
                _ => false
            };
        }

        private static bool ShouldUseBurst(State state, Strategy strategy, float deadline) =>
            state.RaidBuffsLeft >= deadline || strategy.RaidBuffsIn > 9000;
    }
}
