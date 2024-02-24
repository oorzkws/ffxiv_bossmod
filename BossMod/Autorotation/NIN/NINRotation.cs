using System;
using System.Text;

namespace BossMod.NIN
{
    public static class Rotation
    {
        const float HUTON_REFRESH = 28.0f;

        public class State(float[] cooldowns) : CommonRotation.PlayerState(cooldowns)
        {
            public float KassatsuLeft;
            public float HutonLeft;
            public float SuitonLeft;
            public float DotonLeft;
            public float TargetMugLeft;
            public float TargetTrickLeft;
            public float MeisuiLeft;
            public bool Hidden;
            public float KamaitachiLeft;
            public float TrueNorthLeft;

            public (float Left, int Combo) TenChiJin;
            public (float Left, int Combo) Mudra;
            public (float Left, int Stacks) Bunshin;
            public (float Left, int Stacks) RaijuReady;

            public bool InCombo => Mudra.Left > 0;

            public byte Ninki;

            public AID ComboLastMove => ComboTimeLeft > GCD ? (AID)ComboLastAction : AID.None;

            public bool Unlocked(AID aid) => Definitions.Unlocked(aid, Level, UnlockProgress);

            public bool Unlocked(TraitID tid) => Definitions.Unlocked(tid, Level, UnlockProgress);

            public float NextMudraCD => KassatsuLeft > 0 ? 0 : MathF.Max(CD(CDGroup.Ten) - 20, 0);

            public AID BestTen => TCJAdjust[0];
            public AID BestChi => TCJAdjust[1];
            public AID BestJin => TCJAdjust[2];

            public AID CurrentNinjutsu => Combos.GetCurrentNinjutsu(Mudra.Combo, KassatsuLeft > 0);

            public AID TCJEnder =>
                TCJAdjust[0] == AID.None
                    ? TCJAdjust[1] == AID.None
                        ? TCJAdjust[2]
                        : TCJAdjust[1]
                    : TCJAdjust[0];

            private AID[] TCJAdjust
            {
                get
                {
                    if (TenChiJin.Left > 0)
                        return TenChiJin.Combo switch
                        {
                            0 => [AID.FumaTen, AID.FumaChi, AID.FumaJin],
                            1 => [AID.None, AID.TCJRaiton, AID.TCJHyoton],
                            2 => [AID.TCJKaton, AID.None, AID.TCJHyoton],
                            3 => [AID.TCJKaton, AID.TCJRaiton, AID.None],
                            6 or 9 => [AID.None, AID.None, AID.TCJSuiton],
                            7 or 13 => [AID.None, AID.TCJDoton, AID.None],
                            11 or 14 => [AID.TCJHuton, AID.None, AID.None],
                            _ => [AID.None, AID.None, AID.None]
                        };
                    else if (InCombo || KassatsuLeft > 0)
                        return [AID.Ten2, AID.Chi2, AID.Jin2];
                    else
                        return [AID.Ten, AID.Chi, AID.Jin];
                }
            }

            public uint CurrentComboLength =>
                Mudra.Combo switch
                {
                    <= 0 => 0,
                    < 4 => 1,
                    < 16 => 2,
                    _ => 3
                };

            // we might use trick on an add or something, after which assassinate should still be used even if we switch targets (because it's a waste of a cd otherwise)
            // (ideally shouldn't be using trick on adds but whatever)
            // the actual duration of trick attack is 15.77s
            public bool IsTrickActive => TargetTrickLeft > 0 || CD(CDGroup.TrickAttack) >= 44.23f;
            public bool UseSuitonInOpener => Unlocked(AID.Meisui) || !Unlocked(AID.TenChiJin);

            private static readonly string[] MudraNames = ["", "Ten", "Chi", "Jin"];

            public static string ShowMudra(int combo)
            {
                var mudraDescription = new StringBuilder("[");
                if (combo > 0)
                    mudraDescription.Append(MudraNames[combo & 3]);
                if (combo > 4)
                    mudraDescription.Append($",{MudraNames[(combo >> 2) & 3]}");
                if (combo > 16)
                    mudraDescription.Append($",{MudraNames[(combo >> 4) & 3]}");
                mudraDescription.Append(']');
                return mudraDescription.ToString();
            }

            public override string ToString()
            {
                return $"M={ShowMudra(Mudra.Combo)}/{Mudra.Left:f2} (CD {CD(CDGroup.Ten)}), B={Bunshin.Stacks}/{Bunshin.Left}, K={KassatsuLeft}, Trick={TargetTrickLeft}, Mug={TargetMugLeft}, PotCD={PotionCD:f3}, GCD={GCD:f3}, ALock={AnimationLock:f3}+{AnimationLockDelay:f3}, lvl={Level}/{UnlockProgress}";
            }
        }

        public class Strategy : CommonRotation.Strategy
        {
            public enum TrueNorthUse : uint
            {
                Automatic = 0,

                [PropertyDisplay("Delay", 0x800000ff)]
                Delay = 1,

                [PropertyDisplay("Force", 0x8000ff00)]
                Force = 2,
            }

            public enum NinjutsuUse : uint
            {
                Automatic = 0, // use to prevent overcap, use all charges in trick window

                [PropertyDisplay("Do not use", 0x800000ff)]
                Delay = 1, // use none

                [PropertyDisplay("Use all charges", 0x8000ff00)]
                Force = 2, // use all

                [PropertyDisplay("Place Doton if not already active, otherwise do not use")]
                Doton = 3,
            }

            public enum TCJUse : uint
            {
                // use after trick unless we're level 70 or 71, then use before trick
                Automatic = 0,

                [PropertyDisplay("Do not automatically use")]
                Delay = 1,

                [PropertyDisplay("Use ASAP")]
                Force = 2,
            }

            public enum PKUse : uint
            {
                Automatic = 0, // use it bby, its damage

                [PropertyDisplay("Do not automatically use")]
                Delay = 1,

                [PropertyDisplay("Use ASAP")]
                Force = 2,

                [PropertyDisplay("Only use if outside melee range")]
                UseOutsideMelee = 3
            }

            public TrueNorthUse TrueNorthStrategy;
            public NinjutsuUse NinjutsuStrategy;
            public TCJUse TCJStrategy;
            public PKUse PKStrategy;

            public bool AutoHide;
            public bool AutoUnhide;
            public bool AllowDashRaiju;

            public int NumPointBlankAOETargets;
            public int NumKatonTargets;
            public int NumFrogTargets;
            public int NumTargetsInDoton;

            // not equivalent to "num aoe targets >= 3"; for AI mode, we want to avoid using ST buff actions on trash mobs even if they're alone (i.e. quest targets)
            // not only because suiton -> trick is slow, but because those charges could more profitably be spent on raiton
            public bool UseAOERotation;

            public void ApplyStrategyOverrides(uint[] overrides) { }

            public override string ToString()
            {
                return $"AOE={NumPointBlankAOETargets}/Katon {NumKatonTargets}/Frog {NumFrogTargets}/Doton {NumTargetsInDoton}";
            }
        }

        public static AID GetNextBestGCD(State state, Strategy strategy)
        {
            AID act;

            if (strategy.CombatTimer < 0)
            {
                if (strategy.CombatTimer < -100 && state.HutonLeft < 5 && PerformNinjutsu(state, AID.Huton, out act))
                    return act;

                if (
                    strategy.CombatTimer > -9.5
                    && strategy.CombatTimer < -6
                    && state.HutonLeft < HUTON_REFRESH
                    && PerformNinjutsu(state, AID.Huton, out act)
                )
                    return act;

                // levels 45-69: suiton in opener to enable trick attack
                // levels 72-90: same as above, then use TCJ to get another suiton to enable Meisui
                // levels 70-71: we have TCJ, but the only usable TCJ finishers are Doton (AOE only) and Suiton, so
                // instead, use Raiton in opener, then immediately cast TCJ to get Suiton, then use Trick
                if (state.UseSuitonInOpener)
                {
                    if (strategy.CombatTimer > -6 && PerformNinjutsu(state, AID.Suiton, out act))
                    {
                        // delay suiton
                        if (act == AID.Suiton && strategy.CombatTimer < -1)
                            return AID.None;

                        return act;
                    }
                }
                else
                {
                    if (strategy.CombatTimer > -5.5 && PerformNinjutsu(state, AID.Raiton, out act))
                    {
                        if (act == AID.Raiton && strategy.CombatTimer < -1)
                            return AID.None;

                        return act;
                    }
                }

                if (strategy.CombatTimer > -100)
                    return AID.None;
            }

            if (!HaveTarget(state, strategy))
                return AID.None;

            if (state.TenChiJin.Left > 0)
                return NextTCJAction(state, strategy);

            // emergency huton refresh
            // this block needs to be moved down because it tends to interrupt other things
            if (state.HutonLeft == 0 && state.Mudra.Left == 0 && state.TenChiJin.Left == 0)
                if (state.Unlocked(AID.Huraijin))
                    return AID.Huraijin;
                else if (state.KassatsuLeft == 0 && PerformNinjutsu(state, AID.Huton, out act))
                    return act;

            // low level huton refresh
            if (
                !state.Unlocked(AID.ArmorCrush)
                && state.HutonLeft < 5
                && state.KassatsuLeft == 0
                && PerformNinjutsu(state, AID.Huton, out act)
            )
                return act;

            // spending charges on suiton + trick on dungeon packs is generally a potency loss
            if (ShouldUseSuiton(state, strategy) && PerformNinjutsu(state, AID.Suiton, out act))
                return act;

            if (ShouldUseDamageNinjutsu(state, strategy))
            {
                if (!state.Unlocked(AID.Chi))
                {
                    // level <35, raiton is not unlocked yet
                    if (PerformNinjutsu(state, AID.FumaShuriken, out act))
                        return act;
                }
                else if (
                    // if kassatsu expires this will hopefully fall through to case 4 below where we use regular katon
                    // since katon's combo start is raiton's combo end it's not possible to be in a state where we can
                    // choose which one we want to use
                    state.KassatsuLeft > state.GCD
                    && state.Unlocked(AID.GokaMekkyaku)
                    && strategy.NumKatonTargets >= 3
                    && PerformNinjutsu(state, AID.GokaMekkyaku, out act)
                )
                {
                    return act;
                }
                else if (
                    // wait until trick application to use hyosho
                    // extra check here that kassatsu won't expire within 2 mudra casts. unlike goka, which downgrades
                    // to katon (which serves the same purpose), hyosho downgrades to hyoton which is generally worthless
                    state.KassatsuLeft
                        > state.GCD + (2 - state.CurrentComboLength)
                    && PerformNinjutsu(state, AID.HyoshoRanryu, out act)
                )
                {
                    return act;
                }
                else
                {
                    if (strategy.NumKatonTargets >= 3 && PerformNinjutsu(state, AID.Katon, out act))
                        return act;

                    if (PerformNinjutsu(state, AID.Raiton, out act))
                        return act;
                }
            }

            if (state.KamaitachiLeft > state.GCD && ShouldUsePK(state, strategy))
                return AID.PhantomKamaitachi;

            if (state.RaijuReady.Left > state.GCD)
            {
                if (state.RangeToTarget <= 3)
                    return AID.FleetingRaiju;
                else if (strategy.AllowDashRaiju)
                    return AID.ForkedRaiju;
            }

            if (strategy.NumPointBlankAOETargets >= 3 && state.Unlocked(AID.DeathBlossom))
            {
                if (state.ComboLastMove == AID.DeathBlossom && state.Unlocked(AID.HakkeMujinsatsu))
                    return AID.HakkeMujinsatsu;

                return AID.DeathBlossom;
            }
            else
            {
                if (state.ComboLastMove == AID.GustSlash && state.Unlocked(AID.AeolianEdge))
                    // TODO: flank armor crush is 420 vs flank aeolian 380
                    // also don't need to refresh armor crush at exactly 29s if we aren't on flank, since 30s is a long
                    // time and aeolian is higher potency
                    return ShouldUseCrush(state, strategy, state.GCD) ? AID.ArmorCrush : AID.AeolianEdge;

                if (state.ComboLastMove == AID.SpinningEdge && state.Unlocked(AID.GustSlash))
                    return AID.GustSlash;

                return AID.SpinningEdge;
            }
        }

        public static ActionID GetNextBestOGCD(State state, Strategy strategy, float deadline)
        {
            // don't use anything during ninjutsu as it breaks combo
            // TODO does this need to be a condition on all of our ogcd action definitions to prevent queueing?
            if (state.InCombo || state.TenChiJin.Left > 0)
                return new();

            if (
                state.CD(CDGroup.Ten) > 0
                && strategy.AutoHide
                && state.CanWeave(CDGroup.Hide, 0.6f, deadline)
                && strategy.CombatTimer < 0
            )
                return ActionID.MakeSpell(AID.Hide);

            // early TCJ, levels 70-71 only
            if (
                strategy.CombatTimer > -1
                && !state.UseSuitonInOpener
                && state.CanWeave(CDGroup.TenChiJin, 0.6f, deadline)
                && state.RangeToTarget <= 20
                && strategy.ForceMovementIn > deadline + 3
                && (!strategy.UseAOERotation || strategy.NumKatonTargets >= 3)
                && state.KassatsuLeft == 0
                && state.Unlocked(AID.TenChiJin)
            )
                return ActionID.MakeSpell(AID.TenChiJin);

            // otherwise, first weave is kassatsu
            if (
                strategy.CombatTimer > -1
                && !ShouldUseSuiton(state, strategy)
                && state.CanWeave(CDGroup.Kassatsu, 0.6f, deadline)
                && state.RangeToTarget <= 20
                && state.Unlocked(AID.Kassatsu)
            )
                return ActionID.MakeSpell(AID.Kassatsu);

            if (state.IsTrickActive || strategy.UseAOERotation)
            {
                // these two have a different cdgroup for some reason
                if (state.Unlocked(AID.DreamWithinADream))
                {
                    if (state.CanWeave(CDGroup.DreamWithinADream, 0.6f, deadline))
                        return ActionID.MakeSpell(AID.DreamWithinADream);
                }
                else if (state.Unlocked(AID.Assassinate) && state.CanWeave(CDGroup.Assassinate, 0.6f, deadline))
                {
                    return ActionID.MakeSpell(AID.Assassinate);
                }

                if (
                    ShouldUseTCJ(state, strategy)
                    && strategy.ForceMovementIn > deadline + 3
                    && state.CanWeave(CDGroup.TenChiJin, 0.6f, deadline)
                )
                    return ActionID.MakeSpell(AID.TenChiJin);

                if (
                    state.SuitonLeft > state.GCD
                    && state.Unlocked(AID.Meisui)
                    && state.Ninki <= 50
                    && state.CanWeave(CDGroup.Meisui, 0.6f, deadline)
                )
                    return ActionID.MakeSpell(AID.Meisui);
            }

            if (ShouldUseBunshin(state, strategy) && state.CanWeave(CDGroup.Bunshin, 0.6f, deadline))
                return ActionID.MakeSpell(AID.Bunshin);

            if (ShouldUseBhava(state, strategy) && state.CanWeave(CDGroup.HellfrogMedium, 0.6f, deadline))
            {
                if (!state.Unlocked(AID.Bhavacakra) || strategy.NumFrogTargets >= (state.MeisuiLeft > deadline ? 4 : 3))
                    return ActionID.MakeSpell(AID.HellfrogMedium);

                return ActionID.MakeSpell(AID.Bhavacakra);
            }

            if (ShouldUseMug(state, strategy) && state.CanWeave(CDGroup.Mug, 0.6f, deadline))
                return ActionID.MakeSpell(AID.Mug);

            if (
                ShouldUseTrick(state, strategy)
                && state.CanWeave(CDGroup.TrickAttack, 0.6f, deadline)
                && state.GCD < 0.800
            )
                return ActionID.MakeSpell(AID.TrickAttack);

            return new();
        }

        public static (Positional, bool) GetNextPositional(State state, Strategy strategy)
        {
            if (strategy.NumPointBlankAOETargets >= 3 || !state.Unlocked(AID.TrickAttack))
                return (Positional.Any, false);

            if (state.Hidden)
                return (Positional.Rear, false);

            if (!state.Unlocked(AID.AeolianEdge))
                return (Positional.Any, false);

            var gcdsInAdvance = state.ComboLastMove switch
            {
                AID.GustSlash => 0,
                AID.SpinningEdge => 1,
                _ => 2
            };

            return (
                ShouldUseCrush(state, strategy, state.GCD + (state.AttackGCDTime * gcdsInAdvance))
                    ? Positional.Flank
                    : Positional.Rear,
                gcdsInAdvance == 0
            );
        }

        private static bool PerformNinjutsu(State state, AID ninjutsu, out AID act)
        {
            act = AID.None;

            if (!state.Unlocked(ninjutsu) || (!state.InCombo && state.NextMudraCD > state.GCD))
                return false;

            // katon and raiton are our two most used filler ninjutsu and they can both start with jin, but there's a 10 level span where
            // jin isn't unlocked yet
            var jinDowngrade = AID.Jin;
            if (!state.Unlocked(AID.Jin))
            {
                if (ninjutsu == AID.Katon)
                    jinDowngrade = AID.Chi;
                else if (ninjutsu == AID.Raiton)
                    jinDowngrade = AID.Ten;
            }

            var kass = state.KassatsuLeft > state.GCD;

            act = Combos.GetNextAction(ninjutsu, state.Mudra.Combo) switch
            {
                AID.Hyoton => kass && state.Unlocked(AID.HyoshoRanryu) ? AID.HyoshoRanryu : AID.Hyoton,
                AID.Katon => kass && state.Unlocked(AID.GokaMekkyaku) ? AID.GokaMekkyaku : AID.Katon,
                AID.Ten => kass ? AID.Ten2 : AID.Ten,
                AID.Chi => kass ? AID.Chi2 : AID.Chi,
                AID.Jin => kass ? AID.Jin2 : jinDowngrade,
                var x => x
            };

            if (act == AID.None)
            {
                // TODO implement throttling, this prints every frame which is annoying
                Service.Log($"error - broken combo: wanted {ninjutsu} (combo state: {state.Mudra.Combo})");
                act = state.CurrentNinjutsu;
            }

            return true;
        }

        // TODO this function is still kind of bad at recovering in cases where a ninjutsu condition changed while we were mid cast
        public static bool ShouldUseDamageNinjutsu(State state, Strategy strategy)
        {
            // if a conditional flipped while we were in the middle of a combo, finish the combo anyway
            // this can happen if, for example:
            // * trick expires while casting raiton
            // * kassatsu expires during hyosho and it becomes hyoton, which we don't want to use
            if (
                state.Mudra.Left > 0
                || strategy.NinjutsuStrategy == Strategy.NinjutsuUse.Force
                || strategy.NinjutsuStrategy == Strategy.NinjutsuUse.Doton
            )
                return true;

            // target is out of range
            if (state.RangeToTarget > 25 || strategy.NinjutsuStrategy == Strategy.NinjutsuUse.Delay)
                return false;

            // when fighting packs in dungeons, or if we don't have access to suiton, use all mudra charges
            if (strategy.UseAOERotation || !state.Unlocked(AID.Suiton))
                return true;

            // spam raiton in trick windows
            if (state.IsTrickActive && state.Bunshin.Stacks < 5)
                return true;

            // for opener. TODO this is a hack, figure out a better condition for it, something to do with ninjutsu CD
            if (state.TargetMugLeft > state.GCD && state.CD(CDGroup.TenChiJin) > 0)
                return true;

            // prevent charge overcap (TODO: make sure we are saving for trick) (note that the first mudra use increases the cooldown by 20s)
            if (state.CD(CDGroup.Ten) < (state.InCombo ? 25 : 5))
                return true;

            if (state.KassatsuLeft > state.GCD)
            {
                // kassatsu is running out
                if (state.KassatsuLeft < state.GCD + 3)
                    return true;
            }

            return false;
        }

        private static bool ShouldUseMug(State state, Strategy strategy)
        {
            if (!state.Unlocked(AID.Mug) || state.TargetMugLeft > 0)
                return false;

            if (strategy.CombatTimer < 10)
                return state.ComboLastMove == AID.GustSlash;

            return true;
        }

        private static bool ShouldUseTrick(State state, Strategy strategy)
        {
            var canTrick = strategy.CombatTimer > 0 ? state.SuitonLeft > 0 : state.Hidden;

            if (!state.Unlocked(AID.TrickAttack) || !canTrick || strategy.UseAOERotation)
                return false;

            if (strategy.CombatTimer < 10)
                return state.CD(CDGroup.Mug) > 0 && (state.CD(CDGroup.Bunshin) > 0 || !state.Unlocked(AID.Bunshin));

            return true;
        }

        private static bool ShouldUseBunshin(State state, Strategy strategy)
        {
            if (!state.Unlocked(AID.Bunshin) || state.Ninki < 50)
                return false;

            if (strategy.CombatTimer < 10 && !strategy.UseAOERotation)
                return state.TargetMugLeft > 0;

            return true;
        }

        private static bool ShouldUseBhava(State state, Strategy strategy)
        {
            if (!state.Unlocked(AID.HellfrogMedium) || state.Ninki < 50)
                return false;

            if (state.IsTrickActive)
                return state.CD(CDGroup.Meisui) > 0 || !state.Unlocked(TraitID.EnhancedMeisui) || state.Ninki > 50;

            // TODO: don't use if bunshin is about to come off cooldown. idk how to do this in non st mode
            return state.Ninki >= (strategy.UseAOERotation ? 50 : 90);
        }

        private static bool ShouldUseSuiton(State state, Strategy strategy)
        {
            if (
                strategy.UseAOERotation
                || !state.Unlocked(AID.Suiton)
                || state.SuitonLeft > 0
                || state.KassatsuLeft > 0
            )
                return false;

            if (
                state.Unlocked(AID.TenChiJin)
                && !state.Unlocked(AID.Meisui)
                // we will use tcj for trick instead of suiton
                && state.CD(CDGroup.TenChiJin) <= state.CD(CDGroup.TrickAttack)
            )
                return false;

            return state.CD(CDGroup.TrickAttack) < 20;
        }

        private static bool ShouldUseTCJ(State state, Strategy strategy)
        {
            if (
                !state.Unlocked(AID.TenChiJin)
                || state.KassatsuLeft > 0
                || state.CD(CDGroup.Ten) <= state.GCD + 3
                || strategy.TCJStrategy == Strategy.TCJUse.Delay
            )
                return false;

            if (strategy.TCJStrategy == Strategy.TCJUse.Force)
                return true;

            // not sure how important this is. it's probably a loss to use single target TCJ on trash pulls right?
            if (strategy.UseAOERotation && strategy.NumKatonTargets < 3)
                return false;

            if (!state.Unlocked(AID.Meisui))
                return state.CD(CDGroup.TrickAttack) < 20;

            return true;
        }

        private static bool ShouldUseCrush(State state, Strategy strategy, float deadline) =>
            state.Unlocked(AID.ArmorCrush) && state.HutonLeft - deadline < HUTON_REFRESH && state.HutonLeft > deadline;

        private static bool ShouldUsePK(State state, Strategy strategy) => strategy.PKStrategy switch
        {
            // todo: do these need to be separated
            Strategy.PKUse.Automatic or Strategy.PKUse.Force => true,
            Strategy.PKUse.UseOutsideMelee => state.TargetingEnemy && state.RangeToTarget > 3,
            _ => false,
        };

        private static bool HaveTarget(State state, Strategy strategy) =>
            state.TargetingEnemy || strategy.NumPointBlankAOETargets > 0;

        private static AID NextTCJAction(State state, Strategy strategy)
        {
            if (strategy.NumKatonTargets >= 3)
            {
                return state.TenChiJin.Combo switch
                {
                    // action 1
                    0 => AID.FumaJin,
                    // action 2
                    1 => AID.TCJHyoton,
                    2 or 3 => AID.TCJKaton,
                    _ => state.TCJEnder
                };
            }
            else
            {
                return state.TenChiJin.Combo switch
                {
                    // action 1
                    0 => AID.FumaTen,
                    // action 2
                    1 or 3 => AID.TCJRaiton,
                    2 => AID.TCJKaton, // only option to avoid finishing on huton
                    _ => state.TCJEnder
                };
            }
        }
    }
}
