namespace BossMod 
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class MCHConfig : ConfigNode
    {
        [PropertyDisplay("Execute optimal rotations on (Heated) Split Shot (ST) or Spread Shot/Scattergun (AOE)")]
        public bool FullRotation = true;
    }
}
