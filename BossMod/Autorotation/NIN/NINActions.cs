using System;
using System.Runtime.InteropServices;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Objects.Types;

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

        private bool _mudraDebounce;

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

            var aid = Rotation.GetNextBestGCD(_state, _strategy);
            return MakeResult(aid, Autorot.PrimaryTarget);
        }

        protected override NextAction CalculateAutomaticOGCD(float deadline)
        {
            if (_strategy.CombatTimer < 0 && _strategy.AutoUnhide && _state.Hidden)
                return StatusOff(SID.Hidden);

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

            FillStrategyPositionals(
                _strategy,
                Rotation.GetNextPositional(_state, _strategy),
                _state.TrueNorthLeft > _state.GCD
            );

            var isSTMode =
                autoAction == AutoActionST || (autoAction == AutoActionAIFight && IsBoss(Autorot.PrimaryTarget));

            _strategy.NumPointBlankAOETargets = isSTMode
                ? 0
                : Autorot.Hints.NumPriorityTargetsInAOECircle(Player.Position, 5);
            _strategy.NumKatonTargets =
                isSTMode || Autorot.PrimaryTarget == null ? 0 : NumKatonTargets(Autorot.PrimaryTarget);
            _strategy.NumFrogTargets =
                isSTMode || Autorot.PrimaryTarget == null
                    ? 0
                    : Autorot.Hints.NumPriorityTargetsInAOECircle(Autorot.PrimaryTarget.Position, 6);
            _strategy.NumTargetsInDoton =
                _state.DotonLeft > 0 ? Autorot.Hints.NumPriorityTargetsInAOECircle(_lastDotonPos, 5) : 0;
            _strategy.UseAOERotation = !isSTMode;
        }

        private void UpdatePlayerState()
        {
            FillCommonPlayerState(_state);

            var gauge = Service.JobGauges.Get<NINGauge>();
            _state.HutonLeft = gauge.HutonTimer / 1000f;
            _state.Ninki = gauge.Ninki;

            _state.Mudra = StatusDetails(Player, SID.Mudra, Player.InstanceID, 0);
            if (_state.Mudra == (0, 0) && _mudraDebounce)
                // combo param doesn't matter here, it is updated to the correct value long before next GCD
                _state.Mudra.Left = 1000;
            else
                _mudraDebounce = false;

            _state.TenChiJin = StatusDetails(Player, SID.TenChiJin, Player.InstanceID);
            _state.Bunshin = StatusDetails(Player, SID.Bunshin, Player.InstanceID);
            _state.RaijuReady = StatusDetails(Player, SID.RaijuReady, Player.InstanceID);

            _state.KassatsuLeft = StatusDetails(Player, SID.Kassatsu, Player.InstanceID).Left;
            _state.SuitonLeft = StatusDetails(Player, SID.Suiton, Player.InstanceID).Left;
            _state.DotonLeft = StatusDetails(Player, SID.Doton, Player.InstanceID).Left;
            _state.MeisuiLeft = StatusDetails(Player, SID.Meisui, Player.InstanceID).Left;
            _state.Hidden = StatusDetails(Player, SID.Hidden, Player.InstanceID).Stacks > 0;
            _state.KamaitachiLeft = StatusDetails(Player, SID.PhantomKamaitachiReady, Player.InstanceID).Left;

            var mugAny = Autorot.PrimaryTarget?.FindStatus((uint)SID.VulnerabilityUp);
            if (mugAny != null)
                _state.TargetMugLeft = StatusDuration(mugAny.Value.ExpireAt);
            else
                _state.TargetMugLeft = 0;
            _state.TargetTrickLeft = StatusDetails(Autorot.PrimaryTarget, SID.TrickAttack, Player.InstanceID).Left;

            _state.TrueNorthLeft = StatusDetails(Player, SID.TrueNorth, Player.InstanceID).Left;
        }

        protected override void OnActionExecuted(ClientActionRequest request)
        {
            // prevent instant auto hide if the user inputs a mudra manually
            // the mudra CD (which is how we check for missing charges) increases before the status is applied, even the pending one
            if ((AID)request.Action.ID is AID.Ten or AID.Chi or AID.Jin)
                _mudraDebounce = true;

            base.OnActionExecuted(request);
        }

        protected override void OnActionSucceeded(ActorCastEvent ev)
        {
            // TODO there must be a better way to do this. does doton spawn an object we can look for?
            if (ev.Action.ID is (uint)AID.Doton or (uint)AID.TCJDoton)
                _lastDotonPos = Player.Position;

            base.OnActionSucceeded(ev);
        }

        public override Targeting SelectBetterTarget(AIHints.Enemy initial)
        {
            float neededRange;

            if (_state.HutonLeft == 0 && _state.Unlocked(AID.Huraijin))
                neededRange = 3;
            else if (Rotation.ShouldUseDamageNinjutsu(_state, _strategy) && _state.CD(CDGroup.Ten) <= 20)
                neededRange = 20;
            else
                neededRange = 3;

            (var newBest, var tars) = FindBetterTargetBy(initial, 20, e => NumKatonTargets(e.Actor));

            return new(newBest, neededRange);
        }

        private int NumKatonTargets(Actor actor) => Autorot.Hints.NumPriorityTargetsInAOECircle(actor.Position, 6);

        protected override void QueueAIActions()
        {
            bool useAIActions = !_state.InCombo;
            if (_state.Unlocked(AID.SecondWind))
                SimulateManualActionForAI(
                    ActionID.MakeSpell(AID.SecondWind),
                    Player,
                    Player.InCombat && Player.HP.Cur < Player.HP.Max * 0.5f && useAIActions
                );
            if (_state.Unlocked(AID.Bloodbath))
                SimulateManualActionForAI(
                    ActionID.MakeSpell(AID.Bloodbath),
                    Player,
                    Player.InCombat && Player.HP.Cur < Player.HP.Max * 0.8f && useAIActions
                );
            if (_state.Unlocked(AID.ShadeShift))
                SimulateManualActionForAI(
                    ActionID.MakeSpell(AID.ShadeShift),
                    Player,
                    Player.InCombat && Player.HP.Cur < Player.HP.Max * 0.8f && useAIActions
                );
            if (_state.Unlocked(AID.Ten))
                SimulateManualActionForAI(
                    ActionID.MakeSpell(AID.Hide),
                    Player,
                    !Player.InCombat && _state.CD(CDGroup.Ten) > 0 && useAIActions
                );
        }

        private void OnConfigModified(object? sender, EventArgs args)
        {
            SupportedSpell(AID.SpinningEdge).PlaceholderForAuto = _config.FullRotation ? AutoActionST : AutoActionNone;
            SupportedSpell(AID.DeathBlossom).PlaceholderForAuto = _config.FullRotation ? AutoActionAOE : AutoActionNone;

            _strategy.AutoHide = _config.AutoHide;
            _strategy.AutoUnhide = _config.AutoUnhide;
        }

        private static bool IsBoss(Actor? tar)
        {
            if (tar == null)
                return false;
            var tarObject = Service.ObjectTable[tar.SpawnIndex] as BattleChara;
            if (tarObject == null)
                return false;
            // striking dummy
            if (tarObject.NameId == 541)
                return true;
            return Service.LuminaRow<Lumina.Excel.GeneratedSheets.BNpcBase>(tarObject.DataId)?.Rank is 1 or 2 or 6;
        }
    }
}
