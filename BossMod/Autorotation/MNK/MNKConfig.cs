namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    public class MNKConfig : ConfigNode
    {
        public enum FormShiftBehavior : uint
        {
            [PropertyDisplay("Never")]
            Never = 0,

            [PropertyDisplay("Out of combat")]
            OutOfCombat = 1,

            [PropertyDisplay("If no targetable enemies are nearby")]
            NoTargets = 2
        }

        [PropertyDisplay("Execute optimal rotations on Bootshine (ST) or Arm of the Destroyer (AOE)")]
        public bool FullRotation = true;

        [PropertyDisplay("Execute filler rotation (no automatic buff usage) on True Strike")]
        public bool FillerRotation = true;

        [PropertyDisplay("Execute form-specific aoe GCD on Four-point Fury")]
        public bool AOECombos = true;

        [PropertyDisplay("Automatic mouseover targeting for Thunderclap")]
        public bool SmartThunderclap = true;

        [PropertyDisplay("Delay Thunderclap if already in melee range of target")]
        public bool PreventCloseDash = true;

        [PropertyDisplay("Automatic Form Shift")]
        public FormShiftBehavior AutoFormShift = FormShiftBehavior.NoTargets;
    }
}
