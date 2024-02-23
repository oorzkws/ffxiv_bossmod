namespace BossMod.AST
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class ASTConfig : ConfigNode
    {
        public enum RaiseBehavior
        {
            [PropertyDisplay("Unchanged")]
            None = 0,

            [PropertyDisplay("Smart target (mouseover target, otherwise most valuable dead party member)")]
            SmartManual = 1,

            [PropertyDisplay("Automatic")]
            Auto = 2,

            [PropertyDisplay("Automatic, allowing slowcast")]
            AutoSlow = 3
        }

        [PropertyDisplay("Execute optimal rotation on Malefic (ST) or Gravity (AOE)")]
        public bool FullRotation = true;

        [PropertyDisplay("Mouseover targeting for friendly actions")]
        public bool Mouseover = true;

        [PropertyDisplay("Automatic Esuna")]
        public bool AutoEsuna = true;

        [PropertyDisplay("Raise behavior")]
        public RaiseBehavior AutoRaise = RaiseBehavior.None;

        [PropertyDisplay("Smart targeting for cards")]
        public bool SmartCard = true;

        [PropertyDisplay("Automatically draw and play cards")]
        public bool AutoCard = true;
    }
}
