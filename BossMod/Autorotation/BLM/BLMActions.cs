using System;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace BossMod.BLM
{
    class Actions : CommonActions
    {
        public const int AutoActionST = AutoActionFirstCustom + 0;
        public const int AutoActionAOE = AutoActionFirstCustom + 1;
        public const int AutoActionFiller = AutoActionFirstCustom + 2;

        private BLMConfig _config;
        private Rotation.State _state;
        private Rotation.Strategy _strategy;
        private DateTime _lastManaTick;
        private uint _prevMP;

        public Actions(Autorotation autorot, Actor player)
            : base(autorot, player, Definitions.UnlockQuests, Definitions.SupportedActions)
        {
            _config = Service.Config.Get<BLMConfig>();
            _state = new(autorot.Cooldowns);
            _strategy = new();
            _prevMP = player.CurMP;

            SupportedSpell(AID.Triplecast).Condition = _ => _state.TriplecastLeft == 0;
            SupportedSpell(AID.Sharpcast).Condition = _ => _state.SharpcastLeft == 0;
            SupportedSpell(AID.Manafont).Condition = _ => _state.CurMP <= 7000;

            _config.Modified += OnConfigModified;
            OnConfigModified(null, EventArgs.Empty);
        }

        public override void Dispose()
        {
            _config.Modified -= OnConfigModified;
        }

        public override CommonRotation.PlayerState GetState() => _state;

        public override CommonRotation.Strategy GetStrategy() => _strategy;

        public override Targeting SelectBetterTarget(AIHints.Enemy initial)
        {
            // TODO: multidot?..
            var bestTarget = initial;
            if (_state.Unlocked(AID.Blizzard2))
                // if multiple targets result in the same AOE count, prioritize by HP
                bestTarget = FindBetterTargetBy(
                    initial,
                    25,
                    e => NumTargetsHitByAOE(e.Actor) * 1000000 + (int)e.Actor.HP.Cur
                ).Target;
            return new(bestTarget, bestTarget.StayAtLongRange ? 25 : 15);
        }

        protected override void UpdateInternalState(int autoAction)
        {
            UpdatePlayerState();
            FillCommonStrategy(_strategy, CommonDefinitions.IDPotionInt);
            _strategy.UseAOERotation =
                Autorot.PrimaryTarget != null
                && autoAction != AutoActionST
                && _state.Unlocked(AID.Blizzard2)
                && NumTargetsHitByAOE(Autorot.PrimaryTarget) >= 3;

            _strategy.ApplyStrategyOverrides(
                Autorot
                    .Bossmods.ActiveModule?.PlanExecution
                    ?.ActiveStrategyOverrides(Autorot.Bossmods.ActiveModule.StateMachine) ?? new uint[0]
            );

            if (autoAction == AutoActionFiller)
            {
                _strategy.LeylinesStrategy = CommonRotation.Strategy.OffensiveAbilityUse.Delay;
                _strategy.TriplecastStrategy = CommonRotation.Strategy.OffensiveAbilityUse.Delay;
            }
        }

        protected override void QueueAIActions()
        {
            if (_state.Unlocked(AID.Transpose))
                SimulateManualActionForAI(
                    ActionID.MakeSpell(AID.Transpose),
                    Player,
                    !Player.InCombat
                        && (
                            _state.ElementalLevel > 0 && _state.CurMP < 10000
                            || _state.ElementalLevel < 0
                                && _state.CurMP == 10000
                                && _state.UmbralHearts == _state.MaxHearts
                                && _state.FirestarterLeft > 0
                            || _state.ElementalLevel != 0 && _state.ElementalLeft < 3
                        )
                );
            if (_state.Unlocked(AID.UmbralSoul))
                SimulateManualActionForAI(
                    ActionID.MakeSpell(AID.UmbralSoul),
                    Player,
                    !Player.InCombat && _state.ElementalLevel < 0 && _state.UmbralHearts < _state.MaxHearts
                );
            if (_state.Unlocked(AID.Manaward))
                SimulateManualActionForAI(
                    ActionID.MakeSpell(AID.Manaward),
                    Player,
                    Player.HP.Cur < Player.HP.Max * 0.8f
                );
            if (_state.Unlocked(AID.Sharpcast))
                SimulateManualActionForAI(
                    ActionID.MakeSpell(AID.Sharpcast),
                    Player,
                    !Player.InCombat && _state.SharpcastLeft == 0
                );
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

            if (res.ID == (uint)AID.LeyLines)
                return new NextAction(res, null, Player.PosRot.XYZ(), ActionSource.Automatic);

            return MakeResult(res, Autorot.PrimaryTarget);
        }

        private void UpdatePlayerState()
        {
            FillCommonPlayerState(_state);

            var gauge = Service.JobGauges.Get<BLMGauge>();

            // track mana ticks
            if (_prevMP < Player.CurMP && !gauge.InAstralFire)
            {
                var expectedTick = Rotation.MPTick(-gauge.UmbralIceStacks);
                if (Player.CurMP - _prevMP == expectedTick)
                {
                    _lastManaTick = Autorot.WorldState.CurrentTime;
                }
            }
            _prevMP = Player.CurMP;
            _state.TimeToManaTick =
                3
                - (
                    _lastManaTick != default
                        ? (float)(Autorot.WorldState.CurrentTime - _lastManaTick).TotalSeconds % 3
                        : 0
                );

            _state.ElementalLevel = gauge.InAstralFire ? gauge.AstralFireStacks : -gauge.UmbralIceStacks;
            _state.ElementalLeft = gauge.ElementTimeRemaining * 0.001f;
            _state.EnochianTimer = gauge.EnochianTimer * 0.001f;
            _state.UmbralHearts = gauge.UmbralHearts;
            _state.Polyglot = gauge.PolyglotStacks;
            _state.Paradox = gauge.IsParadoxActive;

            _state.TriplecastLeft = StatusDetails(Player, SID.Triplecast, Player.InstanceID).Left;
            _state.SwiftcastLeft = Math.Max(
                StatusDetails(Player, SID.Swiftcast, Player.InstanceID).Left,
                // Lost Chainspell
                StatusDetails(Player, 2560, Player.InstanceID).Left
            );
            _state.SharpcastLeft = StatusDetails(Player, SID.Sharpcast, Player.InstanceID).Left;
            _state.ThundercloudLeft = StatusDetails(Player, SID.Thundercloud, Player.InstanceID).Left;
            _state.FirestarterLeft = StatusDetails(Player, SID.Firestarter, Player.InstanceID, 0).Left;

            _state.TargetThunderLeft = Math.Max(
                StatusDetails(Autorot.PrimaryTarget, _state.ExpectedThunder1, Player.InstanceID).Left,
                StatusDetails(Autorot.PrimaryTarget, _state.ExpectedThunder2, Player.InstanceID).Left
            );

            _state.LeyLinesLeft = StatusDetails(Player, SID.LeyLines, Player.InstanceID).Left;
            _state.InLeyLines = StatusDetails(Player, SID.CircleOfPower, Player.InstanceID).Left > 0;

            _state.FontOfMagicLeft = StatusDetails(Player, SID.LostFontOfMagic, Player.InstanceID).Left;
            _state.MagicBurstLeft = StatusDetails(Player, SID.MagicBurst, Player.InstanceID).Left;
            _state.LucidDreamingLeft = StatusDetails(Player, SID.LucidDreaming, Player.InstanceID).Left;
        }

        private void OnConfigModified(object? sender, EventArgs args)
        {
            // placeholders
            SupportedSpell(AID.Fire1).PlaceholderForAuto = SupportedSpell(AID.Fire4).PlaceholderForAuto =
                _config.FullRotation ? AutoActionST : AutoActionNone;
            SupportedSpell(AID.Fire2).PlaceholderForAuto = _config.FullRotation ? AutoActionAOE : AutoActionNone;
            SupportedSpell(AID.Blizzard1).PlaceholderForAuto = SupportedSpell(AID.Blizzard4).PlaceholderForAuto =
                _config.FullRotation ? AutoActionFiller : AutoActionNone;

            // smart targets
            SupportedSpell(AID.AetherialManipulation).TransformTarget = _config.SmartDash
                ? (tar) => Autorot.SecondaryTarget ?? tar
                : null;

            _strategy.AutoRefresh = _config.AutoIceRefresh;
        }

        private int NumTargetsHitByAOE(Actor primary) =>
            Autorot.Hints.NumPriorityTargetsInAOECircle(primary.Position, 5);
    }
}
