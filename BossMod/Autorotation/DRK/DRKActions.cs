using System;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace BossMod.DRK
{
    class Actions : TankActions
    {
        public const int AutoActionST = AutoActionFirstCustom + 0;
        public const int AutoActionAOE = AutoActionFirstCustom + 1;

        private WPos _saltedEarthPosition;

        private DRKConfig _config;
        private Rotation.State _state;
        private Rotation.Strategy _strategy;

        public Actions(Autorotation autorot, Actor player)
            : base(autorot, player, Definitions.UnlockQuests, Definitions.SupportedActions)
        {
            _config = Service.Config.Get<DRKConfig>();
            _state = new(autorot.Cooldowns);
            _strategy = new();

            SupportedSpell(AID.SaltedEarth).TransformAction = () => ActionID.MakeSpell(_state.BestSalt);
            SupportedSpell(AID.Oblation).Condition = (tar) => tar?.FindStatus((uint)SID.Oblation) == null;
            SupportedSpell(AID.Grit).TransformAction = SupportedSpell(AID.ReleaseGrit).TransformAction = () => ActionID.MakeSpell(_state.HaveTankStance ? AID.ReleaseGrit : AID.Grit);

            _config.Modified += OnConfigModified;
            OnConfigModified(null, EventArgs.Empty);
        }

        public override void Dispose()
        {
            _config.Modified -= OnConfigModified;
        }

        public override CommonRotation.PlayerState GetState() => _state;

        public override CommonRotation.Strategy GetStrategy() => _strategy;

        protected override void UpdateInternalState(int autoAction)
        {
            base.UpdateInternalState(autoAction);
            UpdatePlayerState();
            FillCommonStrategy(_strategy, CommonDefinitions.IDPotionStr);
            _strategy.ApplyStrategyOverrides(
                Autorot
                    .Bossmods.ActiveModule?.PlanExecution
                    ?.ActiveStrategyOverrides(Autorot.Bossmods.ActiveModule.StateMachine) ?? new uint[0],
                _config.AutomaticTBNFallback
            );

            if (!_config.AutoPlunge)
                _strategy.PlungeStrategy = Rotation.Strategy.PlungeUse.Delay;

            _strategy.NumSaltTargets = Autorot.Hints.NumPriorityTargetsInAOECircle(
                _saltedEarthPosition == default ? Player.Position : _saltedEarthPosition,
                5
            );
            _strategy.NumAOETargets =
                autoAction == AutoActionST ? 0 : Autorot.Hints.NumPriorityTargetsInAOECircle(Player.Position, 5);
            _strategy.NumSHBTargets =
                Autorot.PrimaryTarget == null
                    ? 0
                    : Autorot.Hints.NumPriorityTargetsInAOERect(
                        Player.Position,
                        (Autorot.PrimaryTarget.Position - Player.Position).Normalized(),
                        10,
                        2
                    );
            _strategy.NumFloodTargets =
                (autoAction == AutoActionST && _state.Unlocked(AID.EdgeOfDarkness)) ? 0 : _strategy.NumSHBTargets;
            _strategy.NumDrainTargets =
                autoAction == AutoActionST || Autorot.PrimaryTarget == null
                    ? 0
                    : Autorot.Hints.NumPriorityTargetsInAOECircle(Autorot.PrimaryTarget.Position, 5);

            /*
                        var timeSlice = _strategy.CombatTimer % 120f;
                        _state.RaidBuffsLeft = 0;
                        _strategy.RaidBuffsIn = 10000;
                        if (timeSlice >= 27.8f) {
                            _strategy.RaidBuffsIn = 127.8f - timeSlice;
                        } else if (timeSlice >= 7.8f) {
                            _state.RaidBuffsLeft = 27.8f - timeSlice;
                        } else {
                            _strategy.RaidBuffsIn = 7.8f - timeSlice;
                        }
                        */
        }

        private void UpdatePlayerState()
        {
            FillCommonPlayerState(_state);
            if (_state.AnimationLockDelay < 0.1f)
                _state.AnimationLockDelay = 0.1f; // TODO: reconsider; we generally don't want triple weaves or extra-late proc weaves

            _state.HaveTankStance = Player.FindStatus(SID.Grit) != null;

            var gauge = Service.JobGauges.Get<DRKGauge>();
            _state.Blood = gauge.Blood;
            _state.ShadowLeft = gauge.ShadowTimeRemaining / 1000f;
            _state.DarksideLeft = gauge.DarksideTimeRemaining / 1000f;
            _state.HaveDarkArts = gauge.HasDarkArts;
            _state.Delirium = StatusDetails(Player, SID.Delirium, Player.InstanceID);
            _state.BloodWeapon = StatusDetails(Player, SID.BloodWeapon, Player.InstanceID);
            _state.SaltedEarthLeft = StatusDetails(Player, SID.SaltedEarth, Player.InstanceID).Left;
            if (_state.SaltedEarthLeft == 0)
                _saltedEarthPosition = default;
        }

        protected override void OnActionSucceeded(ActorCastEvent ev)
        {
            if (ev.Action.ID == (uint)AID.SaltedEarth)
                _saltedEarthPosition = Player.Position;
        }

        protected override void QueueAIActions() { }

        protected override NextAction CalculateAutomaticGCD()
        {
            if (AutoAction < AutoActionAIFight)
                return new();

            var aid = Rotation.GetNextBestGCD(_state, _strategy);
            return MakeResult(aid, Autorot.PrimaryTarget);
        }

        protected override NextAction CalculateAutomaticOGCD(float deadline)
        {
            if (Autorot.PrimaryTarget == null || AutoAction < AutoActionAIFight)
                return new();

            ActionID res = new();
            if (_state.CanWeave(deadline - _state.OGCDSlotLength)) // first ogcd slot
                res = Rotation.GetNextBestOGCD(_state, _strategy, deadline - _state.OGCDSlotLength, false);
            if (!res && _state.CanWeave(deadline)) // second/only ogcd slot
                res = Rotation.GetNextBestOGCD(_state, _strategy, deadline, true);
            return MakeResult(res, Autorot.PrimaryTarget);
        }

        private void OnConfigModified(object? sender, EventArgs args)
        {
            SupportedSpell(AID.HardSlash).PlaceholderForAuto = _config.FullRotation ? AutoActionST : AutoActionNone;
            SupportedSpell(AID.Unleash).PlaceholderForAuto = _config.FullRotation ? AutoActionAOE : AutoActionNone;

            SupportedSpell(AID.Shirk).TransformTarget = _config.SmartTargetShirk ? SmartTargetCoTank : null;
            SupportedSpell(AID.TheBlackestNight).TransformTarget = SupportedSpell(AID.Oblation).TransformTarget =
                _config.SmartTargetFriendly ? SmartTargetFriendlyOrSelf : null;
        }
    }
}
