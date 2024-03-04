namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    public class RDMConfig : ConfigNode
    {
        [PropertyDisplay("Execute optimal rotations on Jolt (ST) or Scatter/Impact (AOE)")]
        public bool FullRotation = true;

        [PropertyDisplay("Replace Verraise with Swiftcast/Jolt/Vercure if no instant-cast status is active")]
        public bool SmartRaise = true;
    }
}
