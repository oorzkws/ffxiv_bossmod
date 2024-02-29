using System.Linq;
using static BossMod.CommonActions;

namespace BossMod.SGE
{
    public static class Rotation
    {
        // full state needed for determining next action
        public class State : CommonRotation.PlayerState
        {
            public int Gall; // 3 max
            public float NextGall; // 20 max
            public int Sting; // 3 max
            public bool Eukrasia;

            public float SwiftcastLeft; // 0 if buff not up, max 10
            public float TargetDotLeft; // i am not typing that shit out. max 30

            public AID BestDosis =>
                Eukrasia
                    ? FindUnlocked(AID.EukrasianDosisIII, AID.EukrasianDosisII, AID.EukrasianDosis)
                    : FindUnlocked(AID.DosisIII, AID.DosisII, AID.Dosis);
            public AID BestPhlegma => FindUnlocked(AID.PhlegmaIII, AID.PhlegmaII, AID.Phlegma);
            public AID BestDyskrasia => FindUnlocked(AID.DyskrasiaII, AID.Dyskrasia);
            public AID BestToxikon => FindUnlocked(AID.ToxikonII, AID.Toxikon);

            public SID ExpectedEudosis =>
                Unlocked(AID.EukrasianDosisIII)
                    ? SID.EukrasianDosisIII
                    : Unlocked(AID.EukrasianDosisII)
                        ? SID.EukrasianDosisII
                        : SID.EukrasianDosis;

            public CDGroup PhlegmaCD =>
                Unlocked(AID.PhlegmaIII)
                    ? CDGroup.PhlegmaIII
                    : Unlocked(AID.PhlegmaII)
                        ? CDGroup.PhlegmaII
                        : CDGroup.Phlegma;

            public State(float[] cooldowns)
                : base(cooldowns) { }

            public bool Unlocked(AID aid) => Definitions.Unlocked(aid, Level, UnlockProgress);

            public bool Unlocked(TraitID tid) => Definitions.Unlocked(tid, Level, UnlockProgress);

            private AID FindUnlocked(params AID[] actions)
            {
                var act = actions.FirstOrDefault(Unlocked);
                return act == AID.None ? actions.Last() : act;
            }

            public override string ToString()
            {
                return $"E={Eukrasia}, S={Sting}, G={Gall}/{NextGall}, RB={RaidBuffsLeft:f1}, PotCD={PotionCD:f1}, GCD={GCD:f3}, ALock={AnimationLock:f3}+{AnimationLockDelay:f3}, lvl={Level}/{UnlockProgress}";
            }
        }

        // strategy configuration
        public class Strategy : CommonRotation.Strategy
        {
            public int NumDyskrasiaTargets; // 5y around self
            public int NumToxikonTargets; // 5y around target
            public int NumPhlegmaTargets; // 5y around target
            public int NumPneumaTargets; // 25y/4y rect

            public override string ToString()
            {
                return $"AOE={NumDyskrasiaTargets}/{NumToxikonTargets}, no-dots={ForbidDOTs}, movement-in={ForceMovementIn:f3}";
            }
        }

        public static bool CanCast(State state, Strategy strategy, float castTime) =>
            state.SwiftcastLeft > state.GCD || strategy.ForceMovementIn >= state.GCD + castTime;

        public static bool RefreshDOT(State state, float timeLeft) => timeLeft < state.GCD + 3.0f; // TODO: tweak threshold so that we don't overwrite or miss ticks...

        public static AID GetNextBestGCD(State state, Strategy strategy)
        {
            var canCast = CanCast(state, strategy, 1.5f);

            if (strategy.NumDyskrasiaTargets > 1 && state.Unlocked(state.BestDyskrasia))
                return state.BestDyskrasia;

            if (!canCast && state.Unlocked(AID.Toxikon) && state.Sting > 0 && strategy.NumToxikonTargets > 0)
                return state.BestToxikon;

            if (strategy.NumPneumaTargets >= 3 && state.Unlocked(AID.Pneuma) && state.CD(CDGroup.Pneuma) <= state.GCD)
                return AID.Pneuma;

            if (!state.TargetingEnemy && state.Unlocked(AID.Eukrasia))
                return state.Eukrasia ? AID.None : AID.Eukrasia;

            if (strategy.CombatTimer > -100 && strategy.CombatTimer < 0)
                return AID.None;

            if (RefreshDOT(state, state.TargetDotLeft) && state.Unlocked(AID.Eukrasia))
                return state.Eukrasia ? state.BestDosis : AID.Eukrasia;

            if (
                state.RangeToTarget <= 6
                && state.CD(state.PhlegmaCD) <= 40
                && ShouldUseBurst(state, strategy)
                && strategy.NumPhlegmaTargets > 0
            )
                return state.BestPhlegma;

            if (canCast)
                return state.BestDosis;

            if (strategy.NumDyskrasiaTargets > 0 && state.Unlocked(state.BestDyskrasia))
                return state.BestDyskrasia;

            return AID.None;
        }

        private static bool ShouldUseBurst(State state, Strategy strategy)
        {
            return state.RaidBuffsLeft > state.GCD || strategy.RaidBuffsIn > 9000;
        }

        public static ActionID GetNextBestOGCD(State state, Strategy strategy, float deadline)
        {
            if (
                state.CurMP <= 7000
                && state.Unlocked(AID.LucidDreaming)
                && state.CanWeave(CDGroup.LucidDreaming, 0.6f, deadline)
            )
                return ActionID.MakeSpell(AID.LucidDreaming);

            if (
                state.Unlocked(AID.Rhizomata)
                && state.Gall < 2
                && state.NextGall > 10
                && state.CanWeave(CDGroup.Rhizomata, 0.6f, deadline)
            )
                return ActionID.MakeSpell(AID.Rhizomata);

            return new();
        }
    }
}
