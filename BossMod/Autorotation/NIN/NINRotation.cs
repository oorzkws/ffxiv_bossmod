using System;
using System.Linq;
using System.Text;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace BossMod.NIN
{
    public static class Rotation
    {
        public class State(float[] cooldowns) : CommonRotation.PlayerState(cooldowns)
        {
            public float KassatsuLeft;
            public float HutonLeft;
            public float SuitonLeft;
            public float DotonLeft;
            public float TargetMugLeft;
            public float TargetTrickLeft;
            public float MeisuiLeft;
            public float HiddenLeft;
            public float KamaitachiLeft;

            public (float Left, int Combo) TenChiJin;
            public (float Left, int Combo) Mudra;
            public (float Left, int Stacks) Bunshin;
            public (float Left, int Stacks) RaijuReady;

            public byte Ninki;

            public AID ComboLastMove => (AID)ComboLastAction;

            public bool Unlocked(AID aid) => Definitions.Unlocked(aid, Level, UnlockProgress);

            public bool Unlocked(TraitID tid) => Definitions.Unlocked(tid, Level, UnlockProgress);

            public float NextMudraCD => KassatsuLeft > 0 ? 0 : MathF.Max(CD(CDGroup.Ten) - 20, 0);

            private static readonly string[] MudraNames = ["", "Ten", "Chi", "Jin"];

            public static string ShowMudra(int combo)
            {
                var m1 = combo & 3;
                var m2 = (combo >> 2) & 3;
                var m3 = (combo >> 4) & 3;
                var mudraDescription = new StringBuilder("[");
                if (m1 > 0)
                    mudraDescription.Append(MudraNames[m1]);
                if (m2 > 0)
                    mudraDescription.Append($",{MudraNames[m2]}");
                if (m3 > 0)
                    mudraDescription.Append($",{MudraNames[m3]}");
                mudraDescription.Append(']');
                return mudraDescription.ToString();
            }

            public override string ToString()
            {
                return $"M={ShowMudra(Mudra.Combo)},{Mudra.Left:f3}";
            }
        }

        public class Strategy : CommonRotation.Strategy { }

        public static AID GetNextBestGCD(State state, Strategy strategy)
        {
            AID act;

            if (strategy.CombatTimer > -100 && strategy.CombatTimer < 0)
            {
                if (
                    strategy.CombatTimer > -9.5
                    && strategy.CombatTimer < -6
                    && state.HutonLeft < 50
                    && DoNinjutsu(state, AID.Huton, out act)
                )
                    return act;

                if (strategy.CombatTimer > -6 && DoNinjutsu(state, AID.Suiton, out act))
                {
                    // delay suiton
                    if (act == AID.Suiton && strategy.CombatTimer < -1)
                        return AID.None;

                    return act;
                }

                return AID.None;
            }

            if (!state.TargetingEnemy)
                return AID.None;

            if (state.TargetMugLeft > state.GCD && state.KamaitachiLeft > state.GCD)
                return AID.PhantomKamaitachi;

            if (state.TenChiJin.Left > 0)
                return state.TenChiJin.Combo switch
                {
                    0 => AID.FumaTen,
                    1 => AID.TCJRaiton,
                    _ => AID.TCJSuiton
                };

            if (state.RaijuReady.Left > state.GCD)
                return state.RangeToTarget > 3 ? AID.ForkedRaiju : AID.FleetingRaiju;

            if (
                state.TargetTrickLeft > state.GCD
                && state.Bunshin.Stacks < 5
                && state.KassatsuLeft > state.GCD
                && DoNinjutsu(state, AID.HyoshoRanryu, out act)
            )
                return act;

            if (state.KassatsuLeft == 0 && DoNinjutsu(state, AID.Raiton, out act))
                return act;

            if (state.ComboLastMove == AID.GustSlash)
                return AID.AeolianEdge;

            if (state.ComboLastMove == AID.SpinningEdge)
                return AID.GustSlash;

            return AID.SpinningEdge;
        }

        public static ActionID GetNextBestOGCD(
            State state,
            Strategy strategy,
            float deadline,
            bool lastSlot
        )
        {
            // don't use anything during ninjutsu as it breaks combo
            if (state.Mudra.Left > 0 || state.TenChiJin.Left > 0)
                return new();

            if (
                state.HutonLeft > 0
                && state.CD(CDGroup.Ten) > 0
                && state.CanWeave(CDGroup.Hide, 0.6f, deadline)
                && strategy.CombatTimer <= 0
            )
                return ActionID.MakeSpell(AID.Hide);

            if (
                strategy.CombatTimer > -1
                && state.CanWeave(CDGroup.Kassatsu, 0.6f, deadline)
                && state.Unlocked(AID.Kassatsu)
            )
                return ActionID.MakeSpell(AID.Kassatsu);

            if (state.TargetMugLeft > 0 && state.TargetTrickLeft > 0)
            {
                if (state.CanWeave(CDGroup.DreamWithinADream, 0.6f, deadline))
                    return ActionID.MakeSpell(AID.DreamWithinADream);

                if (
                    state.RaijuReady.Left > state.GCD
                    && state.CanWeave(CDGroup.TenChiJin, 0.6f, deadline)
                )
                    return ActionID.MakeSpell(AID.TenChiJin);

                if (state.SuitonLeft > state.GCD && state.CanWeave(CDGroup.Meisui, 0.6f, deadline))
                    return ActionID.MakeSpell(AID.Meisui);

                if (
                    state.CD(CDGroup.Meisui) > 0
                    && state.Ninki >= 50
                    && state.CanWeave(CDGroup.HellfrogMedium, 0.6f, deadline)
                )
                    return ActionID.MakeSpell(AID.Bhavacakra);
            }

            if (state.ComboLastMove == AID.GustSlash)
            {
                if (
                    state.Ninki >= 50
                    && state.TargetMugLeft > 0
                    && state.CanWeave(CDGroup.Bunshin, 0.6f, deadline)
                )
                    return ActionID.MakeSpell(AID.Bunshin);

                if (
                    state.Bunshin.Left > 0
                    && lastSlot
                    && state.TargetMugLeft > 0
                    && state.KamaitachiLeft == 0
                    && state.CanWeave(CDGroup.TrickAttack, 0.6f, deadline)
                )
                    return ActionID.MakeSpell(AID.TrickAttack);

                if (state.CanWeave(CDGroup.Mug, 0.6f, deadline))
                    return ActionID.MakeSpell(AID.Mug);
            }

            return new();
        }

        private static bool DoNinjutsu(State state, AID ninjutsu, out AID act)
        {
            act = AID.None;

            if (
                !state.Unlocked(ninjutsu)
                || (state.Mudra.Left == 0 && state.NextMudraCD > state.GCD)
            )
                return false;

            var combo = state.Mudra.Combo;
            var (Start, Continue, End) = GetComboStates(ninjutsu);
            if (combo == 0)
            {
                act = Start;
                return true;
            }

            if (End.Contains(combo))
            {
                act = ninjutsu;
                return true;
            }

            var comboAction = Continue.FirstOrDefault(x => x.Item1 == state.Mudra.Combo).Item2;
            if (comboAction == AID.None)
            {
                if (state.Mudra.Combo == 13)
                    act = AID.Hyoton;
                else if (state.Mudra.Combo == 6)
                    act = AID.Katon;
                else
                    act = AID.RabbitMedium;
                return true;
            }

            act = comboAction;
            return true;
        }

        private static (AID Start, (int, AID)[] Continue, int[] End) GetComboStates(AID ninjutsu)
        {
            return ninjutsu switch
            {
                AID.FumaShuriken => (AID.Jin, [], [1, 2, 3]),
                AID.Katon or AID.GokaMekkyaku => (AID.Chi, [(2, AID.Ten2), (3, AID.Ten2)], [6, 7]),
                AID.Raiton => (AID.Ten, [(1, AID.Chi2), (3, AID.Chi2),], [9, 11]),
                AID.Hyoton
                or AID.HyoshoRanryu
                    => (AID.Ten, [(1, AID.Jin2), (2, AID.Jin2)], [13, 14]),
                AID.Huton
                    => (
                        AID.Jin,
                        [(3, AID.Chi2), (11, AID.Ten2), (2, AID.Jin2), (14, AID.Ten2)],
                        [27, 30]
                    ),
                AID.Doton
                    => (
                        AID.Jin,
                        [(3, AID.Ten2), (7, AID.Chi2), (1, AID.Jin2), (13, AID.Chi2)],
                        [39, 45]
                    ),
                AID.Suiton
                    => (
                        AID.Chi,
                        [(2, AID.Ten2), (6, AID.Jin2), (1, AID.Chi2), (9, AID.Jin2)],
                        [54, 57]
                    ),
                _ => throw new ArgumentException($"Action {ninjutsu} is not a ninjutsu")
            };
        }
    }
}
