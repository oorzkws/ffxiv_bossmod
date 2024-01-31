using System;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace BossMod.MCH
{
    class Actions : CommonActions
    {
        public const int AutoActionST = AutoActionFirstCustom + 0;
        public const int AutoActionAOE = AutoActionFirstCustom + 1;

        private Rotation.State _state;
        private Rotation.Strategy _strategy;
        private MCHConfig _config;

        public Actions(Autorotation autorot, Actor player)
            : base(autorot, player, Definitions.UnlockQuests, Definitions.SupportedActions)
        {
            _config = Service.Config.Get<MCHConfig>();
            _state = new(autorot.Cooldowns);
            _strategy = new();

            _config.Modified += OnConfigModified;
            OnConfigModified(null, EventArgs.Empty);
        }

        public override CommonRotation.PlayerState GetState() => _state;

        public override CommonRotation.Strategy GetStrategy() => _strategy;

        protected override NextAction CalculateAutomaticOGCD(float deadline)
        {
            if (Autorot.PrimaryTarget == null || AutoAction < AutoActionAIFight)
                return new();

            ActionID res = new();
            if (_state.CanWeave(deadline - _state.OGCDSlotLength)) // first ogcd slot
                res = Rotation.GetNextBestOGCD(_state, _strategy, deadline - _state.OGCDSlotLength);
            if (!res && _state.CanWeave(deadline)) // second/only ogcd slot
                res = Rotation.GetNextBestOGCD(_state, _strategy, deadline);
            return MakeResult(res, Autorot.PrimaryTarget);
        }

        protected override NextAction CalculateAutomaticGCD()
        {
            if (Autorot.PrimaryTarget == null || AutoAction < AutoActionAIFight)
                return new();

            return MakeResult(ActionID.MakeSpell(Rotation.GetNextBestGCD(_state, _strategy)), Autorot.PrimaryTarget);
        }

        protected override void QueueAIActions() { }

        public override void Dispose()
        {
            _config.Modified -= OnConfigModified;
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

            _strategy.NumAOETargets =
                autoAction == AutoActionST || Autorot.PrimaryTarget == null ? 0 : NumConeTargets(Autorot.PrimaryTarget, 12);
            _strategy.NumFlamethrowerTargets =
                autoAction == AutoActionST || Autorot.PrimaryTarget == null ? 0 : NumConeTargets(Autorot.PrimaryTarget, 8);
            _strategy.NumChainsawTargets = Autorot.PrimaryTarget == null ? 0 : NumChainsawTargets(Autorot.PrimaryTarget);
            _strategy.NumRicochetTargets =
                Autorot.PrimaryTarget == null
                    ? 0
                    : Autorot.Hints.NumPriorityTargetsInAOECircle(Autorot.PrimaryTarget.Position, 5);
        }

        private int NumChainsawTargets(Actor target) =>
            Autorot.Hints.NumPriorityTargetsInAOERect(Player.Position, (target.Position - Player.Position).Normalized(), 25, 2);

        private int NumConeTargets(Actor target, float range) =>
            Autorot.Hints.NumPriorityTargetsInAOECone(Player.Position, range, (target.Position - Player.Position).Normalized(), 45.Degrees());

        public override Targeting SelectBetterTarget(AIHints.Enemy initial)
        {
            var newBest = initial;
            if (_state.ReassembleLeft > _state.GCD && _state.Unlocked(AID.ChainSaw) && _state.CD(CDGroup.ChainSaw) == 0)
                newBest = FindBetterTargetBy(initial, 25, x => NumChainsawTargets(x.Actor)).Target;
            else
                newBest = FindBetterTargetBy(initial, 12, x => NumConeTargets(x.Actor, 12)).Target;

            return new(newBest, newBest.StayAtLongRange ? 25 : 12);
        }

        private void UpdatePlayerState()
        {
            FillCommonPlayerState(_state);

            var gauge = Service.JobGauges.Get<MCHGauge>();
            _state.Heat = gauge.Heat;
            _state.Battery = gauge.Battery;
            _state.OverheatLeft = gauge.OverheatTimeRemaining / 1000f;
            _state.IsOverheated = gauge.IsOverheated;
            _state.HasMinion = gauge.IsRobotActive;

            _state.ReassembleLeft = StatusDetails(Player, SID.Reassembled, Player.InstanceID).Left;
            _state.WildfireLeft = StatusDetails(Player, SID.WildfireActive, Player.InstanceID).Left;
        }

        private void OnConfigModified(object? sender, EventArgs args)
        {
            SupportedSpell(AID.SplitShot).PlaceholderForAuto = SupportedSpell(AID.HeatedSplitShot).PlaceholderForAuto =
                _config.FullRotation ? AutoActionST : AutoActionNone;
            SupportedSpell(AID.SpreadShot).PlaceholderForAuto = SupportedSpell(AID.Scattergun).PlaceholderForAuto =
                _config.FullRotation ? AutoActionAOE : AutoActionNone;
        }
    }
}
