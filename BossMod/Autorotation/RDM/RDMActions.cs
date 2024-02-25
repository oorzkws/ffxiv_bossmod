using System;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace BossMod.RDM
{
    class Actions : CommonActions
    {
        public const int AutoActionST = AutoActionFirstCustom + 0;
        public const int AutoActionAOE = AutoActionFirstCustom + 1;

        private readonly Rotation.State _state;
        private readonly Rotation.Strategy _strategy;
        private readonly RDMConfig _config;

        public Actions(Autorotation autorot, Actor player)
            : base(autorot, player, Definitions.UnlockQuests, Definitions.SupportedActions)
        {
            _config = Service.Config.Get<RDMConfig>();
            _state = new(autorot.Cooldowns);
            _strategy = new();

            SupportedSpell(AID.Riposte).TransformAction = () => ActionID.MakeSpell(_state.BestRiposte);
            SupportedSpell(AID.Zwerchhau).TransformAction = () => ActionID.MakeSpell(_state.BestZwerchhau);
            SupportedSpell(AID.Redoublement).TransformAction = () => ActionID.MakeSpell(_state.BestRedoublement);
            SupportedSpell(AID.Moulinet).TransformAction = () => ActionID.MakeSpell(_state.BestMoulinet);
            SupportedSpell(AID.Reprise).TransformAction = () => ActionID.MakeSpell(_state.BestReprise);
            SupportedSpell(AID.Veraero).TransformAction = SupportedSpell(AID.VeraeroIII).TransformAction = () =>
                ActionID.MakeSpell(_state.BestAero);
            SupportedSpell(AID.VeraeroII).TransformAction = () => ActionID.MakeSpell(_state.BestAero2);
            SupportedSpell(AID.Verthunder).TransformAction = SupportedSpell(AID.VerthunderIII).TransformAction = () =>
                ActionID.MakeSpell(_state.BestThunder);
            SupportedSpell(AID.VerthunderII).TransformAction = () => ActionID.MakeSpell(_state.BestThunder2);
            SupportedSpell(AID.Jolt).TransformAction = () => ActionID.MakeSpell(_state.BestJolt);
            SupportedSpell(AID.Scatter).TransformAction = () => ActionID.MakeSpell(_state.BestScatter);

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
            FillCommonStrategy(_strategy, CommonDefinitions.IDPotionInt);
            _strategy.ApplyStrategyOverrides(
                Autorot
                    .Bossmods.ActiveModule?.PlanExecution
                    ?.ActiveStrategyOverrides(Autorot.Bossmods.ActiveModule.StateMachine) ?? []
            );

            _strategy.NumManastackAOETargets = NumCircleTargets(Autorot.PrimaryTarget, 5);
            _strategy.NumAOETargets = autoAction == AutoActionST ? 0 : _strategy.NumManastackAOETargets;
            _strategy.NumC6Targets = NumCircleTargets(Autorot.PrimaryTarget, 6);
            _strategy.NumMoulinetTargets = autoAction == AutoActionST ? 0 : NumMoulinetTargets(Autorot.PrimaryTarget);
            _strategy.NumResolutionTargets = NumResolutionTargets(Autorot.PrimaryTarget);
        }

        private void UpdatePlayerState()
        {
            FillCommonPlayerState(_state);

            var gauge = Service.JobGauges.Get<RDMGauge>();

            _state.DualcastLeft = StatusDetails(Player, SID.Dualcast, Player.InstanceID).Left;
            _state.SwiftcastLeft = StatusDetails(Player, SID.Swiftcast, Player.InstanceID).Left;
            _state.AccelerationLeft = StatusDetails(Player, SID.Acceleration, Player.InstanceID).Left;
            _state.ManaficationLeft = StatusDetails(Player, SID.Manafication, Player.InstanceID).Left;
            _state.VerstoneReadyLeft = StatusDetails(Player, SID.VerstoneReady, Player.InstanceID).Left;
            _state.VerfireReadyLeft = StatusDetails(Player, SID.VerfireReady, Player.InstanceID).Left;
            _state.WhiteMana = gauge.WhiteMana;
            _state.BlackMana = gauge.BlackMana;
            _state.ManaStacks = gauge.ManaStacks;
        }

        public override Targeting SelectBetterTarget(AIHints.Enemy initial)
        {
            var tar = initial;
            var range = initial.StayAtLongRange ? 25 : 15;

            if (_state.ResolutionReady)
            {
                tar = FindBetterTargetBy(initial, 25, (e) => NumResolutionTargets(e.Actor)).Target;
                range = tar.StayAtLongRange ? 25 : 15;
            }
            else if (_state.MinMana >= 60 - (_state.ManaStacks * 20) && _strategy.NumMoulinetTargets >= 3)
            {
                tar = FindBetterTargetBy(initial, 8, (e) => NumMoulinetTargets(e.Actor)).Target;
                range = 8;
            }
            else if (_state.ManaStacks == 3 || _state.ScorchReady)
            {
                tar = FindBetterTargetBy(initial, 25, (e) => NumCircleTargets(e.Actor, 5)).Target;
                range = tar.StayAtLongRange ? 25 : 15;
            }

            return new(tar, range);
        }

        protected override void QueueAIActions() { }

        private void OnConfigModified(object? sender, EventArgs args)
        {
            SupportedSpell(AID.Jolt).PlaceholderForAuto = SupportedSpell(AID.JoltII).PlaceholderForAuto =
                _config.FullRotation ? AutoActionST : AutoActionNone;
            SupportedSpell(AID.Scatter).PlaceholderForAuto = SupportedSpell(AID.Impact).PlaceholderForAuto =
                _config.FullRotation ? AutoActionAOE : AutoActionNone;
        }

        private int NumCircleTargets(Actor? primary, float radius) =>
            primary == null ? 0 : Autorot.Hints.NumPriorityTargetsInAOECircle(primary.Position, radius);

        private int NumMoulinetTargets(Actor? primary) =>
            primary == null
                ? 0
                : Autorot.Hints.NumPriorityTargetsInAOECone(
                    Player.Position,
                    8,
                    (primary.Position - Player.Position).Normalized(),
                    60.Degrees()
                );

        private int NumResolutionTargets(Actor? primary) =>
            primary == null
                ? 0
                : Autorot.Hints.NumPriorityTargetsInAOERect(
                    Player.Position,
                    (primary.Position - Player.Position).Normalized(),
                    25,
                    2
                );
    }
}
