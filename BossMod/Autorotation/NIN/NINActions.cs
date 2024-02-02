using System;
using System.Runtime.CompilerServices;
using Dalamud.Game.ClientState.JobGauge.Types;
using FFXIVClientStructs.FFXIV.Common.Lua;

namespace BossMod.NIN
{
    class Actions : CommonActions
    {
        public const int AutoActionST = AutoActionFirstCustom + 0;
        public const int AutoActionAOE = AutoActionFirstCustom + 1;

        private Rotation.State _state;
        private Rotation.Strategy _strategy;

        public Actions(Autorotation autorot, Actor player)
            : base(autorot, player, Definitions.UnlockQuests, Definitions.SupportedActions)
        {
            _state = new(autorot.Cooldowns);
            _strategy = new();

            SupportedSpell(AID.SpinningEdge).PlaceholderForAuto = AutoActionST;
        }

        public override CommonRotation.PlayerState GetState()
        {
            return _state;
        }

        public override CommonRotation.Strategy GetStrategy()
        {
            return _strategy;
        }

        public override void Dispose() { }

        protected override NextAction CalculateAutomaticGCD()
        {
            if (AutoAction < AutoActionAIFight)
                return new();

            var aid = Rotation.GetNextBestGCD(_state, _strategy);
            return MakeResult(aid, Autorot.PrimaryTarget);
        }

        protected override NextAction CalculateAutomaticOGCD(float deadline)
        {
            if (AutoAction < AutoActionAIFight)
                return new();

            ActionID res = new();
            if (_state.CanWeave(deadline - _state.OGCDSlotLength)) // first ogcd slot
                res = Rotation.GetNextBestOGCD(
                    _state,
                    _strategy,
                    deadline - _state.OGCDSlotLength,
                    false
                );
            if (!res && _state.CanWeave(deadline)) // second/only ogcd slot
                res = Rotation.GetNextBestOGCD(_state, _strategy, deadline, true);
            return MakeResult(res, Autorot.PrimaryTarget);
        }

        protected override void UpdateInternalState(int autoAction)
        {
            FillCommonPlayerState(_state);
            FillCommonStrategy(_strategy, CommonDefinitions.IDPotionDex);

            var gauge = Service.JobGauges.Get<NINGauge>();
            _state.HutonLeft = gauge.HutonTimer / 1000f;
            _state.Ninki = gauge.Ninki;

            // bypass pending status check for mudras because it takes 3 seconds to finalize, so 3-action mudras time out
            var stat = Player.FindStatus(SID.Mudra, Player.InstanceID);
            if (stat == null)
                _state.Mudra = (0, 0);
            else
                _state.Mudra = (StatusDuration(stat.Value.ExpireAt), stat.Value.Extra & 0xFF);

            _state.TenChiJin = StatusDetails(Player, SID.TenChiJin, Player.InstanceID);
            _state.Bunshin = StatusDetails(Player, SID.Bunshin, Player.InstanceID);
            _state.RaijuReady = StatusDetails(Player, SID.RaijuReady, Player.InstanceID);

            _state.KassatsuLeft = StatusDetails(Player, SID.Kassatsu, Player.InstanceID).Left;
            _state.SuitonLeft = StatusDetails(Player, SID.Suiton, Player.InstanceID).Left;
            _state.DotonLeft = StatusDetails(Player, SID.Doton, Player.InstanceID).Left;
            _state.MeisuiLeft = StatusDetails(Player, SID.Meisui, Player.InstanceID).Left;
            _state.HiddenLeft = StatusDetails(Player, SID.Hidden, Player.InstanceID).Left;
            _state.KamaitachiLeft = StatusDetails(
                Player,
                SID.PhantomKamaitachiReady,
                Player.InstanceID
            ).Left;
            _state.TargetMugLeft = StatusDetails(
                Autorot.PrimaryTarget,
                SID.VulnerabilityUp,
                Player.InstanceID
            ).Left;
            _state.TargetTrickLeft = StatusDetails(
                Autorot.PrimaryTarget,
                SID.TrickAttack,
                Player.InstanceID
            ).Left;
        }

        protected override void QueueAIActions() { }
    }
}
