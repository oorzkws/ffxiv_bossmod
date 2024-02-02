using System;

namespace BossMod.BLU
{
    class Actions : CommonActions
    {
        public const int AutoActionST = AutoActionFirstCustom + 0;
        public const int AutoActionAOE = AutoActionFirstCustom + 1;
        public const int AutoMimicD = AutoActionFirstCustom + 2;
        public const int AutoMimicH = AutoActionFirstCustom + 3;
        public const int AutoMimicT = AutoActionFirstCustom + 4;

        private Rotation.State _state;
        private Rotation.Strategy _strategy;
        private BLUConfig _config;

        public Actions(Autorotation autorot, Actor player)
            : base(autorot, player, Definitions.UnlockQuests, Definitions.SupportedActions)
        {
            _config = Service.Config.Get<BLUConfig>();
            _state = new(autorot.Cooldowns);
            _strategy = new();

            _config.Modified += OnConfigModified;
            OnConfigModified(null, EventArgs.Empty);
        }

        public override void Dispose()
        {
            _config.Modified -= OnConfigModified;
        }

        public override CommonRotation.PlayerState GetState() => _state;

        public override CommonRotation.Strategy GetStrategy() => _strategy;

        protected override void QueueAIActions() { }

        protected override NextAction CalculateAutomaticGCD()
        {
            if (Autorot.PrimaryTarget == null || AutoAction < AutoActionAIFight)
                return new();

            var aid = Rotation.GetNextBestGCD(_state, _strategy);
            return MakeResult(aid, Autorot.PrimaryTarget);
        }

        protected override NextAction CalculateAutomaticOGCD(float deadline)
        {
            return new();
        }

        protected override unsafe void UpdateInternalState(int autoAction)
        {
            FillCommonPlayerState(_state);
            FillCommonStrategy(_strategy, CommonDefinitions.IDPotionInt);

            for (var i = 0; i < 24; i++)
                _state.BLUSlots[i] = (AID)
                    FFXIVClientStructs.FFXIV.Client.Game.ActionManager.Instance()->GetActiveBlueMageActionInSlot(i);

            _state.TargetMortalFlame = Autorot.PrimaryTarget?.FindStatus(SID.MortalFlame) != null;
            var bomExpire = Autorot.PrimaryTarget?.FindStatus(SID.BreathOfMagic)?.ExpireAt;
            _state.TargetBoMLeft = bomExpire == null ? 0f : StatusDuration(bomExpire.Value);
            _state.TargetDropsyLeft = StatusDetails(Autorot.PrimaryTarget, SID.Dropsy, Player.InstanceID).Left;
            _state.TargetSlowLeft = StatusDetails(Autorot.PrimaryTarget, SID.Slow, Player.InstanceID).Left;
            _state.TargetBindLeft = StatusDetails(Autorot.PrimaryTarget, SID.Bind, Player.InstanceID).Left;
            _state.TargetLightheadedLeft = StatusDetails(Autorot.PrimaryTarget, SID.Lightheaded, Player.InstanceID).Left;
            _state.TargetBegrimedLeft = StatusDetails(Autorot.PrimaryTarget, SID.Begrimed, Player.InstanceID).Left;

            _state.SurpanakhasFury = StatusDetails(Player, SID.SurpanakhasFury, Player.InstanceID);
            _state.WaxingLeft = StatusDetails(Player, SID.WaxingNocturne, Player.InstanceID).Left;
            _state.HarmonizedLeft = StatusDetails(Player, SID.Harmonized, Player.InstanceID).Left;
            _state.TinglingLeft = StatusDetails(Player, SID.Tingling, Player.InstanceID).Left;
            _state.BoostLeft = StatusDetails(Player, SID.Boost, Player.InstanceID).Left;
            _state.BrushWithDeathLeft = StatusDetails(Player, SID.BrushWithDeath, Player.InstanceID).Left;
            _state.ToadOilLeft = StatusDetails(Player, SID.ToadOil, Player.InstanceID).Left;
            _state.VeilLeft = StatusDetails(Player, SID.VeilOfTheWhorl, Player.InstanceID).Left;
            _state.ApokalypsisLeft = StatusDetails(Player, SID.Apokalypsis, Player.InstanceID).Left;
            _state.FlurryLeft = StatusDetails(Player, SID.PhantomFlurry, Player.InstanceID).Left;
        }

        private void OnConfigModified(object? sender, EventArgs args)
        {
            SupportedSpell(AID.SonicBoom).PlaceholderForAuto =
                SupportedSpell(AID.WaterCannon).PlaceholderForAuto =
                SupportedSpell(AID.ChocoMeteor).PlaceholderForAuto =
                    _config.FullRotation ? AutoActionST : AutoActionNone;
        }
    }
}
