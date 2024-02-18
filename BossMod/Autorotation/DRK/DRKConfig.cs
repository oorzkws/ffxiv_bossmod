namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class DRKConfig : ConfigNode
    {
        [PropertyDisplay("Execute optimal rotations on Hard Slash (ST) or Unleash (AOE)")]
        public bool FullRotation = true;

        [PropertyDisplay("Mouseover targeting for TBN and Oblation")]
        public bool SmartTargetFriendly = true;

        [PropertyDisplay("Auto target for Shirk")]
        public bool SmartTargetShirk = true;
    }
}
