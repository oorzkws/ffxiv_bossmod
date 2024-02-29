using System;
using System.Linq;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace BossMod.AST
{
    class Actions : HealerActions
    {
        public const int AutoActionST = AutoActionFirstCustom + 0;
        public const int AutoActionAOE = AutoActionFirstCustom + 1;
        public const int AutoActionFiller = AutoActionFirstCustom + 1;

        private Rotation.State _state;
        private Rotation.Strategy _strategy;
        private ASTConfig _config;

        private WPos _starPos;
        private Actor? _autoRaiseTarget;

        public Actions(Autorotation autorot, Actor player)
            : base(autorot, player, Definitions.UnlockQuests, Definitions.SupportedActions)
        {
            _config = Service.Config.Get<ASTConfig>();
            _state = new(autorot.Cooldowns);
            _strategy = new();

            SupportedSpell(AID.Play).TransformAction = () => ActionID.MakeSpell(_state.BestPlay);
            SupportedSpell(AID.MinorArcana).TransformAction = () => ActionID.MakeSpell(_state.BestCrown);
            SupportedSpell(AID.EarthlyStar).TransformAction = () => ActionID.MakeSpell(_state.BestStar);
            SupportedSpell(AID.Macrocosmos).TransformAction = () => ActionID.MakeSpell(_state.BestCosmos);
            SupportedSpell(AID.Horoscope).TransformAction = () => ActionID.MakeSpell(_state.BestHoroscope);

            _config.Modified += OnConfigModified;
            OnConfigModified(null, EventArgs.Empty);
        }

        public override CommonRotation.PlayerState GetState() => _state;

        public override CommonRotation.Strategy GetStrategy() => _strategy;

        public override void Dispose()
        {
            _config.Modified -= OnConfigModified;
        }

        public override Targeting SelectBetterTarget(AIHints.Enemy initial)
        {
            // TODO: multidot?..
            var bestTarget = initial;
            if (_state.Unlocked(AID.Gravity))
                // if multiple targets result in the same AOE count, prioritize by HP
                bestTarget = FindBetterTargetBy(
                    initial,
                    25,
                    e =>
                        Autorot.Hints.NumPriorityTargetsInAOECircle(e.Actor.Position, 5) * 1000000 + (int)e.Actor.HP.Cur
                ).Target;
            return new(bestTarget, bestTarget.StayAtLongRange ? 25 : 15);
        }

        protected override NextAction CalculateAutomaticGCD()
        {
            if (_config.AutoEsuna && Rotation.CanCast(_state, _strategy, 1))
            {
                var esunaTarget = FindEsunaTarget();
                if (esunaTarget != null)
                    return MakeResult(AID.Esuna, esunaTarget);
            }

            if (_autoRaiseTarget != null && _state.Unlocked(AID.Ascend))
            {
                if (_config.AutoRaise == ASTConfig.RaiseBehavior.Auto && _state.SwiftcastLeft > _state.GCD)
                    return MakeResult(AID.Ascend, _autoRaiseTarget);

                if (_config.AutoRaise == ASTConfig.RaiseBehavior.AutoSlow)
                    return MakeResult(AID.Ascend, _autoRaiseTarget);
            }

            var aid = Rotation.GetNextBestGCD(_state, _strategy);
            return MakeResult(aid, Autorot.PrimaryTarget);
        }

        protected override NextAction CalculateAutomaticOGCD(float deadline)
        {
            if (AutoAction < AutoActionAIFight)
                return new();

            if (
                _autoRaiseTarget != null
                && _state.SwiftcastLeft == 0
                && _state.CanWeave(CDGroup.Swiftcast, 0.6f, deadline)
            )
                return MakeResult(ActionID.MakeSpell(AID.Swiftcast), Player);

            ActionID res = new();
            if (_state.CanWeave(deadline - _state.OGCDSlotLength)) // first ogcd slot
                res = Rotation.GetNextBestOGCD(_state, _strategy, deadline - _state.OGCDSlotLength);
            if (!res && _state.CanWeave(deadline)) // second/only ogcd slot
                res = Rotation.GetNextBestOGCD(_state, _strategy, deadline);

            var tarOverride = Autorot.PrimaryTarget;
            if (res.ID is 4401 or 4402 or 4403)
                tarOverride = SmartCardTarget(tarOverride, MeleePriority);
            else if (res.ID is 4404 or 4405 or 4406)
                tarOverride = SmartCardTarget(tarOverride, RangedPriority);

            return MakeResult(res, tarOverride);
        }

        protected override void QueueAIActions()
        {
            if (_state.Unlocked(AID.Draw))
            {
                SimulateManualActionForAI(ActionID.MakeSpell(AID.Draw), Player, !Player.InCombat && !_state.HasCard);
                SimulateManualActionForAI(
                    ActionID.MakeSpell(AID.Redraw),
                    Player,
                    !Player.InCombat && _state.HasCard && _state.Seals.Any(x => x == _state.NextSeal)
                );
            }
        }

        protected override void UpdateInternalState(int autoAction)
        {
            base.UpdateInternalState(autoAction);
            UpdatePlayerState();
            FillCommonStrategy(_strategy, CommonDefinitions.IDPotionMnd);

            _strategy.ApplyStrategyOverrides(
                Autorot
                    .Bossmods.ActiveModule?.PlanExecution
                    ?.ActiveStrategyOverrides(Autorot.Bossmods.ActiveModule.StateMachine) ?? new uint[0]
            );

            _strategy.NumBigAOETargets = Autorot.Hints.NumPriorityTargetsInAOECircle(Player.Position, 20);
            _strategy.NumGravityTargets =
                autoAction == AutoActionST || Autorot.PrimaryTarget == null
                    ? 0
                    : Autorot.Hints.NumPriorityTargetsInAOECircle(Autorot.PrimaryTarget.Position, 5);
            _strategy.NumStarTargets =
                _starPos == default ? 0 : Autorot.Hints.NumPriorityTargetsInAOECircle(_starPos, 20);

            if (autoAction == AutoActionFiller)
            {
                _strategy.DivinationUse = _strategy.LightspeedUse = CommonRotation.Strategy.OffensiveAbilityUse.Delay;
            }
        }

        protected override void OnActionSucceeded(ActorCastEvent ev)
        {
            if (ev.Action.ID is (uint)AID.EarthlyStar)
                _starPos = ev.TargetXZ;

            base.OnActionSucceeded(ev);
        }

        private void UpdatePlayerState()
        {
            FillCommonPlayerState(_state);

            var gauge = Service.JobGauges.Get<ASTGauge>();
            _state.DrawnCard = (Rotation.Card)gauge.DrawnCard;
            _state.DrawnCrownCard = (Rotation.CrownCard)gauge.DrawnCrownCard;
            _state.Seals = gauge.Seals;

            _state.SwiftcastLeft = StatusDetails(Player, SID.Swiftcast, Player.InstanceID).Left;
            _state.LightspeedLeft = StatusDetails(Player, SID.Lightspeed, Player.InstanceID).Left;

            _state.IsClarified = Player.FindStatus(SID.ClarifyingDraw) != null;

            _state.StarLeft = StatusDetails(Player, SID.EarthlyDominance, Player.InstanceID).Left;
            if (_state.StarLeft > 0)
                _state.StarLeft += 10;
            else
                _state.StarLeft = StatusDetails(Player, SID.GiantDominance, Player.InstanceID).Left;
            if (_state.StarLeft == 0)
                _starPos = default;

            _state.MacrocosmosLeft = StatusDetails(Player, SID.Macrocosmos, Player.InstanceID).Left;
            _state.HoroscopeLeft = StatusDetails(Player, SID.Horoscope, Player.InstanceID).Left;
            _state.HoroHeliosLeft = StatusDetails(Player, SID.HoroscopeHelios, Player.InstanceID).Left;
            _state.AstrodyneLeft = StatusDetails(Player, SID.HarmonyOfSpirit, Player.InstanceID).Left;
            _state.DivinationLeft = StatusDetails(Player, SID.Divination, Player.InstanceID).Left;

            _state.TargetCombustLeft = StatusDetails(
                Autorot.PrimaryTarget,
                _state.ExpectedCombust,
                Player.InstanceID
            ).Left;

            _autoRaiseTarget = _config.AutoRaise is ASTConfig.RaiseBehavior.Auto or ASTConfig.RaiseBehavior.AutoSlow
                ? FindRaiseTarget()
                : null;
        }

        private void OnConfigModified(object? sender, EventArgs args)
        {
            SupportedSpell(AID.Malefic).PlaceholderForAuto =
                SupportedSpell(AID.MaleficII).PlaceholderForAuto =
                SupportedSpell(AID.MaleficIII).PlaceholderForAuto =
                SupportedSpell(AID.MaleficIV).PlaceholderForAuto =
                SupportedSpell(AID.FallMalefic).PlaceholderForAuto =
                    _config.FullRotation ? AutoActionST : AutoActionNone;
            SupportedSpell(AID.Gravity).PlaceholderForAuto = SupportedSpell(AID.GravityII).PlaceholderForAuto =
                _config.FullRotation ? AutoActionAOE : AutoActionNone;
            SupportedSpell(AID.Combust).PlaceholderForAuto =
                SupportedSpell(AID.CombustII).PlaceholderForAuto =
                SupportedSpell(AID.CombustIII).PlaceholderForAuto =
                    _config.FullRotation ? AutoActionFiller : AutoActionNone;

            SupportedSpell(AID.Ascend).TransformTarget = SupportedSpell(AID.Synastry).TransformTarget =
                _config.Mouseover ? (tar) => Autorot.SecondaryTarget ?? tar : null;

            SupportedSpell(AID.Benefic).TransformTarget =
                SupportedSpell(AID.BeneficII).TransformTarget =
                SupportedSpell(AID.AspectedBenefic).TransformTarget =
                SupportedSpell(AID.EssentialDignity).TransformTarget =
                SupportedSpell(AID.Exaltation).TransformTarget =
                SupportedSpell(AID.CelestialIntersection).TransformTarget =
                SupportedSpell(AID.Esuna).TransformTarget =
                    _config.Mouseover ? SmartTargetFriendlyOrSelf : null;

            SupportedSpell(AID.TheBalance).TransformTarget =
                SupportedSpell(AID.TheArrow).TransformTarget =
                SupportedSpell(AID.TheSpear).TransformTarget =
                    _config.SmartCard
                        ? (tar) => SmartCardTarget(tar, MeleePriority)
                        : _config.Mouseover
                            ? SmartTargetFriendlyOrSelf
                            : null;

            SupportedSpell(AID.TheBole).TransformTarget =
                SupportedSpell(AID.TheEwer).TransformTarget =
                SupportedSpell(AID.TheSpire).TransformTarget =
                    _config.SmartCard
                        ? (tar) => SmartCardTarget(tar, RangedPriority)
                        : _config.Mouseover
                            ? SmartTargetFriendlyOrSelf
                            : null;

            SupportedSpell(AID.EarthlyStar).TransformPosition = _config.SmartStar switch
            {
                ASTConfig.StarLocation.Target => () => Autorot.PrimaryTarget?.PosRot.XYZ(),
                ASTConfig.StarLocation.Self => () => Player.PosRot.XYZ(),
                _ => null
            };
        }

        // so this function is designed to pick an optimal card target for whatever card you have in a pug environment,
        // or in duty finder where you might not have a normal raid comp (5 ranged in ct roulette lol)
        // in a planned/timed raid environment, the card priority is as follows (taken from #ast_resources on the balance discord)
        //
        // 0m (opener)
        // melee: NIN -> DRK (prepull "free" card only) -> SAM -> MNK -> RPR -> PLD -> DRG
        // ranged: SMN -> DNC -> BLM -> RDM -> BRD -> MCH
        //
        // 1m/5m
        // melee: DRG with pot -> NIN -> SAM -> DRG without pot -> MNK -> RPR
        // ranged: SMN -> BLM -> MCH @ 5m -> BRD -> RDM -> MCH @ 1m -> DNC
        //
        // 2m/4m
        // melee: SAM -> NIN -> DRK -> MNK -> DRG -> RPR
        // ranged: BLM -> DNC -> SMN -> MCH -> BRD -> RDM
        //
        // 3m
        // melee: NIN -> SAM -> MNK -> RPR -> DRG
        // ranged: BLM -> SMN -> MCH -> BRD -> RDM -> DNC
        //
        // 6m
        // melee: SAM -> NIN -> DRK -> MNK -> DRG -> RPR
        // ranged: BLM -> DNC -> SMN -> MCH -> BRD -> RDM
        private Actor? SmartCardTarget(Actor? primaryTarget, Func<Class, int> prioFunc) =>
            SmartTargetFriendly(primaryTarget)
            ?? Autorot.WorldState.Party.WithoutSlot(false, true).MaxBy(act => CardPriority(act, prioFunc));

        private static int CardPriority(Actor act, Func<Class, int> prioFunc)
        {
            var prio = prioFunc(act.Class);

            // overwriting a card on a dps is probably worth more than putting a card on a tank idk i should do research
            if (HasCardBuff(act))
                prio -= 100;

            foreach (var stat in act.Statuses)
            {
                // brink of death (50% less damage give or take)
                if (stat.ID is 44)
                {
                    prio -= 1000;
                    break;
                }
                // weakness (25% less damage)
                else if (stat.ID is 43)
                {
                    prio -= 500;
                    break;
                }
            }

            return prio;
        }

        private static int MeleePriority(Class c) =>
            c switch
            {
                Class.SAM => 100,
                Class.NIN or Class.ROG => 99,
                Class.MNK or Class.PGL => 90,
                Class.RPR => 89,
                Class.DRG or Class.LNC => 88,
                Class.DRK => 60,
                Class.PLD => 50,
                _ => 0
            };

        private static int RangedPriority(Class c) =>
            c switch
            {
                Class.BLM or Class.THM => 100,
                Class.SMN or Class.ACN => 99,
                Class.RDM => 98,
                Class.MCH => 89,
                Class.BRD or Class.ARC => 88,
                Class.DNC => 87,
                _ => 0
            };

        private static bool HasCardBuff(Actor act) =>
            act.Statuses.Any(
                x =>
                    (SID)x.ID
                        is SID.TheArrow
                            or SID.TheBalance
                            or SID.TheBole
                            or SID.TheEwer
                            or SID.TheSpear
                            or SID.TheSpire
            );
    }
}
