using System;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace BossMod.NIN
{
    class Actions : CommonActions
    {
        public const int AutoActionST = AutoActionFirstCustom + 0;
        public const int AutoActionAOE = AutoActionFirstCustom + 1;

        private NINConfig _config;
        private Rotation.State _state;
        private Rotation.Strategy _strategy;

        private WPos _lastDotonPos;

        public Actions(Autorotation autorot, Actor player)
            : base(autorot, player, Definitions.UnlockQuests, Definitions.SupportedActions)
        {
            _config = Service.Config.Get<NINConfig>();
            _state = new(autorot.Cooldowns);
            _strategy = new();

            SupportedSpell(AID.Ten).TransformAction = SupportedSpell(AID.Ten2).TransformAction = () =>
                ActionID.MakeSpell(_state.BestTen);
            SupportedSpell(AID.Chi).TransformAction = SupportedSpell(AID.Chi2).TransformAction = () =>
                ActionID.MakeSpell(_state.BestChi);
            SupportedSpell(AID.Jin).TransformAction = SupportedSpell(AID.Jin2).TransformAction = () =>
                ActionID.MakeSpell(_state.BestJin);
            SupportedSpell(AID.Ninjutsu).TransformAction = () => ActionID.MakeSpell(_state.CurrentNinjutsu);

            _config.Modified += OnConfigModified;
            OnConfigModified(null, EventArgs.Empty);
        }

        public override CommonRotation.PlayerState GetState() => _state;

        public override CommonRotation.Strategy GetStrategy() => _strategy;

        public override void Dispose()
        {
            _config.Modified -= OnConfigModified;
        }

        protected override NextAction CalculateAutomaticGCD()
        {
            if (AutoAction < AutoActionAIFight)
                return new();

            if (_strategy.CombatTimer < 0 && _strategy.AutoUnhide && _state.Hidden)
                StatusOff((uint)SID.Hidden);

            var aid = Rotation.GetNextBestGCD(_state, _strategy);

            return MakeResult(aid, Autorot.PrimaryTarget);
        }

        private unsafe void StatusOff(uint sid)
        {
            var obj = Service.ObjectTable[Player.SpawnIndex] as Dalamud.Game.ClientState.Objects.Types.BattleChara;
            if (obj == null)
                return;
            var man = (FFXIVClientStructs.FFXIV.Client.Game.StatusManager*)obj.StatusList.Address;
            var stat = man->GetStatusIndex(sid);
            if (stat < 0)
                return;
            man->RemoveStatus(stat);
        }

        protected override NextAction CalculateAutomaticOGCD(float deadline)
        {
            if (AutoAction < AutoActionAIFight)
                return new();

            ActionID res = new();
            if (_state.CanWeave(deadline - _state.OGCDSlotLength)) // first ogcd slot
                res = Rotation.GetNextBestOGCD(_state, _strategy, deadline - _state.OGCDSlotLength);
            if (!res && _state.CanWeave(deadline)) // second/only ogcd slot
                res = Rotation.GetNextBestOGCD(_state, _strategy, deadline);

            return MakeResult(res, Autorot.PrimaryTarget);
        }

        protected override void UpdateInternalState(int autoAction)
        {
            UpdatePlayerState();
            FillCommonStrategy(_strategy, CommonDefinitions.IDPotionDex);
            _strategy.ApplyStrategyOverrides(
                Autorot
                    .Bossmods.ActiveModule?.PlanExecution
                    ?.ActiveStrategyOverrides(Autorot.Bossmods.ActiveModule.StateMachine) ?? new uint[0]
            );

            _strategy.NumPointBlankAOETargets =
                autoAction == AutoActionST ? 0 : Autorot.Hints.NumPriorityTargetsInAOECircle(Player.Position, 5);
            _strategy.NumKatonTargets =
                autoAction == AutoActionST || Autorot.PrimaryTarget == null
                    ? 0
                    : Autorot.Hints.NumPriorityTargetsInAOECircle(Autorot.PrimaryTarget.Position, 5);
            _strategy.NumFrogTargets =
                autoAction == AutoActionST || Autorot.PrimaryTarget == null
                    ? 0
                    : Autorot.Hints.NumPriorityTargetsInAOECircle(Autorot.PrimaryTarget.Position, 6);
            _strategy.NumTargetsInDoton =
                _state.DotonLeft > 0 ? Autorot.Hints.NumPriorityTargetsInAOECircle(_lastDotonPos, 5) : 0;
            _strategy.UseAOERotation = autoAction == AutoActionAOE;
        }

        private void UpdatePlayerState()
        {
            FillCommonPlayerState(_state);

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
            _state.Hidden = StatusDetails(Player, SID.Hidden, Player.InstanceID).Stacks > 0;
            _state.KamaitachiLeft = StatusDetails(Player, SID.PhantomKamaitachiReady, Player.InstanceID).Left;
            _state.TargetMugLeft = StatusDetails(Autorot.PrimaryTarget, SID.VulnerabilityUp, Player.InstanceID).Left;
            _state.TargetTrickLeft = StatusDetails(Autorot.PrimaryTarget, SID.TrickAttack, Player.InstanceID).Left;
        }

        protected override void OnActionSucceeded(ActorCastEvent ev)
        {
            // TODO there must be a better way to do this. does doton spawn an object we can look for?
            if (ev.Action.ID == (uint)AID.Doton)
                _lastDotonPos = Player.Position;

            base.OnActionSucceeded(ev);
        }

        protected override void QueueAIActions()
        {
            if (_state.Unlocked(AID.SecondWind))
                SimulateManualActionForAI(
                    ActionID.MakeSpell(AID.SecondWind),
                    Player,
                    Player.InCombat && Player.HP.Cur < Player.HP.Max * 0.5f
                );
            if (_state.Unlocked(AID.Bloodbath))
                SimulateManualActionForAI(
                    ActionID.MakeSpell(AID.Bloodbath),
                    Player,
                    Player.InCombat && Player.HP.Cur < Player.HP.Max * 0.8f
                );
            if (_state.Unlocked(AID.ShadeShift))
                SimulateManualActionForAI(
                    ActionID.MakeSpell(AID.ShadeShift),
                    Player,
                    Player.InCombat && Player.HP.Cur < Player.HP.Max * 0.8f
                );
        }

        private void OnConfigModified(object? sender, EventArgs args)
        {
            SupportedSpell(AID.SpinningEdge).PlaceholderForAuto = _config.FullRotation ? AutoActionST : AutoActionNone;
            SupportedSpell(AID.DeathBlossom).PlaceholderForAuto = _config.FullRotation ? AutoActionAOE : AutoActionNone;

            _strategy.AutoHide = _config.AutoHide;
            _strategy.AutoUnhide = _config.AutoUnhide;
        }
    }
}
