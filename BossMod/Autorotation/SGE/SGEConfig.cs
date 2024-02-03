namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class SGEConfig : ConfigNode
    {
        public enum RaiseBehavior {
            [PropertyDisplay("Unchanged")]
            None = 0,
            [PropertyDisplay("Smart target (mouseover target, otherwise most valuable dead party member)")]
            SmartManual = 1,
            [PropertyDisplay("Automatic")]
            Auto = 2,
            [PropertyDisplay("Automatic, allowing slowcast")]
            AutoSlow = 3
        }

        [PropertyDisplay("Execute optimal damage rotation on Dosis (ST) or Dyskrasia (AOE)")]
        public bool FullRotation = true;

        [PropertyDisplay("Use mouseover targeting for friendly spells")]
        public bool MouseoverFriendly = true;

        // icarus can target enemies
        [PropertyDisplay("Use mouseover targeting for Icarus")]
        public bool MouseoverIcarus = true;

        [PropertyDisplay("Automatic Esuna")]
        public bool AutoEsuna = true;

        [PropertyDisplay("Raise behavior")]
        public RaiseBehavior AutoRaise = RaiseBehavior.None;

        [PropertyDisplay("Automatically choose Kardia target")]
        public bool AutoKardia = true;

        [PropertyDisplay("Automatically use Druochole (ST heal) to prevent Addersgall overcap")]
        public bool PreventGallOvercap = true;
    }
}
