namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class NINConfig : ConfigNode
    {
        [PropertyDisplay("Execute optimal rotations on Spinning Edge (ST) or Deathblossom (AOE)")]
        public bool FullRotation = true;

        [PropertyDisplay("Automatically Hide out of combat to restore mudra charges")]
        public bool AutoHide = true;

        [PropertyDisplay("Automatically cancel Hide")]
        public bool AutoUnhide = true;

        [PropertyDisplay("Press Hakke Mujinsatsu to cast Doton")]
        public bool OneButtonDoton = true;
    }
}
