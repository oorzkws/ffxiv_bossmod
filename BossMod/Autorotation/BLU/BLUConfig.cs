namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class BLUConfig : ConfigNode
    {
        [PropertyDisplay("Execute optimal rotations on Sonic Boom/Water Cannon/Choco Meteor")]
        public bool FullRotation = true;
    }
}
