using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BossMod.BLU
{
    public static class Rotation
    {
        public class State(float[] cooldowns) : CommonRotation.PlayerState(cooldowns)
        {
            public bool TargetMortalFlame;
            public float TargetBoMLeft; // 60s max
            public float TargetDropsyLeft;
            public float TargetSlowLeft;
            public float TargetBindLeft;
            public float TargetLightheadedLeft;
            public float TargetBegrimedLeft;

            public (float Left, int Stacks) SurpanakhasFury; // 3/3 max
            public int ReproStacks;

            public float WaxingLeft; // 15s max
            public float HarmonizedLeft; // 30s max
            public float TinglingLeft; // 15s max
            public float BoostLeft; // 30s max
            public float BrushWithDeathLeft;
            public float ToadOilLeft;
            public float VeilLeft;
            public float ApokalypsisLeft;
            public float FlurryLeft;

            public AID[] BLUSlots = new AID[24];

            public bool OnSlot(AID act) => BLUSlots.Contains(act);

            public override string ToString()
            {
                var slots = string.Join(", ", BLUSlots.Select(x => x.ToString()));
                return $"actions=[{slots}]";
            }
        }

        public class Strategy : CommonRotation.Strategy { }

        public static AID GetNextBestGCD(State state, Strategy strategy)
        {
            if (!state.TargetMortalFlame && state.OnSlot(AID.MortalFlame))
                return AID.MortalFlame;

            if (
                state.TargetBoMLeft <= state.GCD + state.SpellGCDTime
                && state.OnSlot(AID.BreathOfMagic)
                && state.OnSlot(AID.Bristle)
                && state.RangeToTarget <= 10
                && state.BoostLeft == 0
            )
                return AID.Bristle;

            if (state.TargetBoMLeft <= state.GCD && state.OnSlot(AID.BreathOfMagic) && state.RangeToTarget <= 10)
                return AID.BreathOfMagic;

            if (state.OnSlot(AID.TripleTrident) && state.RangeToTarget <= 3)
            {
                if (
                    state.OnSlot(AID.Whistle)
                    && state.CD(CDGroup.TripleTrident) <= state.GCD + state.SpellGCDTime
                    && state.HarmonizedLeft <= state.GCD + state.SpellGCDTime
                )
                    return AID.Whistle;

                if (state.CD(CDGroup.TripleTrident) <= state.GCD)
                    return AID.TripleTrident;
            }

            return AID.SonicBoom;
        }
    }
}
