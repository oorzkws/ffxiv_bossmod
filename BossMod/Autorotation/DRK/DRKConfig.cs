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

        [PropertyDisplay("Reserve MP for The Blackest Night during general (non-planned) combat")]
        public bool AutomaticTBNFallback = true;

        [PropertyDisplay("Use Plunge as a damage ability")]
        public bool AutoPlunge = true;
    }
}
