using System.Collections.Generic;

namespace BossMod.AST
{
    public enum AID : uint
    {
        None = 0,

        // GCDs
        Malefic = 3596, // L1, 1.5s cast, range 25, single-target 0/0, targets=hostile, animLock=???
        Benefic = 3594, // L2, 1.5s cast, range 30, single-target 0/0, targets=self/party/friendly, animLock=0.100s
        Combust = 3599, // L4, instant, range 25, single-target 0/0, targets=hostile, animLock=???
        Repose = 16560, // L8, 2.5s cast, range 30, single-target 0/0, targets=hostile, animLock=???
        Esuna = 7568, // L10, 1.0s cast, range 30, single-target 0/0, targets=self/party/friendly, animLock=0.100s
        Helios = 3600, // L10, 1.5s cast, range 0, AOE circle 15/0, targets=self, animLock=???
        Ascend = 3603, // L12, 8.0s cast, range 30, single-target 0/0, targets=party/friendly, animLock=???
        BeneficII = 3610, // L26, 1.5s cast, range 30, single-target 0/0, targets=self/party/friendly, animLock=0.100s
        AspectedBenefic = 3595, // L34, instant, range 30, single-target 0/0, targets=self/party/friendly, animLock=0.600s
        AspectedHelios = 3601, // L42, 1.5s cast, range 0, AOE circle 15/0, targets=self, animLock=0.100s
        Gravity = 3615, // L45, 1.5s cast, range 25, AOE circle 5/0, targets=hostile, animLock=???
        CombustII = 3608, // L46, instant, range 25, single-target 0/0, targets=hostile, animLock=???
        MaleficII = 3598, // L54, 1.5s cast, range 25, single-target 0/0, targets=hostile, animLock=???
        MaleficIII = 7442, // L64, 1.5s cast, range 25, single-target 0/0, targets=hostile, animLock=???
        MaleficIV = 16555, // L72, 1.5s cast, range 25, single-target 0/0, targets=hostile, animLock=???
        CombustIII = 16554, // L72, instant, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        FallMalefic = 25871, // L82, 1.5s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        GravityII = 25872, // L82, 1.5s cast, range 25, AOE circle 5/0, targets=hostile, animLock=0.100s

        // oGCDs
        Lightspeed = 3606, // L6, instant, 120.0s CD (group 18), range 0, single-target 0/0, targets=self, animLock=0.600s
        LucidDreaming = 7562, // L14, instant, 60.0s CD (group 45), range 0, single-target 0/0, targets=self, animLock=0.600s
        EssentialDignity = 3614, // L15, instant, 40.0s CD (group 10) (2 charges), range 30, single-target 0/0, targets=self/party/friendly, animLock=0.600s
        Swiftcast = 7561, // L18, instant, 60.0s CD (group 44), range 0, single-target 0/0, targets=self, animLock=0.600s
        TheEwer = 4405, // L30, instant, 1.0s CD (group 1), range 30, single-target 0/0, targets=self/party, animLock=0.600s
        TheSpire = 4406, // L30, instant, 1.0s CD (group 1), range 30, single-target 0/0, targets=self/party, animLock=0.600s
        Draw = 3590, // L30, instant, 30.0s CD (group 11) (2 charges), range 0, single-target 0/0, targets=self, animLock=0.600s
        TheBole = 4404, // L30, instant, 1.0s CD (group 1), range 30, single-target 0/0, targets=self/party, animLock=0.600s
        Undraw = 9629, // L30, instant, 1.0s CD (group 2), range 0, single-target 0/0, targets=self, animLock=???
        Play = 17055, // L30, instant, 1.0s CD (group 1), range 0, single-target 0/0, targets=self, animLock=???
        TheSpear = 4403, // L30, instant, 1.0s CD (group 1), range 30, single-target 0/0, targets=self/party, animLock=0.600s
        TheArrow = 4402, // L30, instant, 1.0s CD (group 1), range 30, single-target 0/0, targets=self/party, animLock=0.600s
        TheBalance = 4401, // L30, instant, 1.0s CD (group 1), range 30, single-target 0/0, targets=self/party, animLock=0.600s
        Redraw = 3593, // L40, instant, 1.0s CD (group 5), range 0, single-target 0/0, targets=self, animLock=0.600s
        Surecast = 7559, // L44, instant, 120.0s CD (group 48), range 0, single-target 0/0, targets=self, animLock=0.600s
        Rescue = 7571, // L48, instant, 120.0s CD (group 49), range 30, single-target 0/0, targets=party, animLock=0.600s
        Astrodyne = 25870, // L50, instant, 1.0s CD (group 7), range 0, single-target 0/0, targets=self, animLock=0.600s
        Synastry = 3612, // L50, instant, 120.0s CD (group 19), range 30, single-target 0/0, targets=party, animLock=???
        Divination = 16552, // L50, instant, 120.0s CD (group 23), range 0, AOE circle 30/0, targets=self, animLock=0.600s
        CollectiveUnconscious = 3613, // L58, instant, 60.0s CD (group 17), range 0, AOE circle 30/0, targets=self, animLock=???
        CelestialOpposition = 16553, // L60, instant, 60.0s CD (group 16), range 0, AOE circle 15/0, targets=self, animLock=???
        EarthlyStar = 7439, // L62, instant, 60.0s CD (group 13), range 30, Ground circle 20/0, targets=area, animLock=0.600s
        StellarDetonation = 8324, // L62, instant, 3.0s CD (group 6), range 0, AOE circle 20/0, targets=self, animLock=0.600s
        MinorArcana = 7443, // L70, instant, 60.0s CD (group 12), range 0, single-target 0/0, targets=self, animLock=0.600s
        LadyOfCrowns = 7445, // L70, instant, 1.0s CD (group 4), range 0, AOE circle 20/0, targets=self, animLock=0.600s
        LordOfCrowns = 7444, // L70, instant, 1.0s CD (group 4), range 0, AOE circle 20/0, targets=self, animLock=0.600s
        CelestialIntersection = 16556, // L74, instant, 30.0s CD (group 9) (2 charges), range 30, single-target 0/0, targets=self/party, animLock=???
        Horoscope = 16557, // L76, instant, 60.0s CD (group 14), range 0, AOE circle 20/0, targets=self, animLock=0.600s
        HoroscopeEnd = 16558, // L76, instant, 1.0s CD (group 3), range 0, AOE circle 20/0, targets=self, animLock=0.600s
        NeutralSect = 16559, // L80, instant, 120.0s CD (group 21), range 0, single-target 0/0, targets=self, animLock=???
        Exaltation = 25873, // L86, instant, 60.0s CD (group 15), range 30, single-target 0/0, targets=self/party, animLock=???
        Macrocosmos = 25874, // L90, instant, 180.0s CD (group 20), range 0, AOE circle 20/0, targets=self, animLock=0.600s
        Microcosmos = 25875, // L90, instant, 1.0s CD (group 0), range 0, AOE circle 20/0, targets=self, animLock=0.600s
    }

    public enum TraitID : uint
    {
        None = 0,
        MaimAndMend = 122, // L20
        EnhancedBenefic = 124, // L36
        MaimAndMendII = 125, // L40
        EnhancedDraw = 495, // L40
        CombustMastery = 186, // L46
        EnhancedDrawII = 496, // L50
        MaleficMastery = 187, // L54
        MaleficMasteryII = 188, // L64
        HyperLightspeed = 189, // L68
        CombustMasteryII = 314, // L72
        MaleficMasteryIII = 315, // L72
        EnhancedEssentialDignity = 316, // L78
        MaleficMasteryIV = 497, // L82
        GravityMastery = 498, // L82
        EnhancedHealingMagic = 499, // L85
        EnhancedCelestialIntersection = 500, // L88
    }

    public enum CDGroup : int
    {
        Microcosmos = 0, // 1.0 max
        Play = 1, // 1.0 max, shared by the Ewer, the Spire, the Bole, Play, the Spear, the Arrow, the Balance
        Undraw = 2, // 1.0 max
        HoroscopeEnd = 3, // 1.0 max
        LadyOfCrowns = 4, // 1.0 max, shared by Lady of Crowns, Lord of Crowns
        Redraw = 5, // 1.0 max
        StellarDetonation = 6, // 3.0 max
        Astrodyne = 7, // 1.0 max
        CelestialIntersection = 9, // 2*30.0 max
        EssentialDignity = 10, // 2*40.0 max
        Draw = 11, // 2*30.0 max
        MinorArcana = 12, // 60.0 max
        EarthlyStar = 13, // 60.0 max
        Horoscope = 14, // 60.0 max
        Exaltation = 15, // 60.0 max
        CelestialOpposition = 16, // 60.0 max
        CollectiveUnconscious = 17, // 60.0 max
        Lightspeed = 18, // 120.0 max
        Synastry = 19, // 120.0 max
        Macrocosmos = 20, // 180.0 max
        NeutralSect = 21, // 120.0 max
        Divination = 23, // 120.0 max
        Swiftcast = 44, // 60.0 max
        LucidDreaming = 45, // 60.0 max
        Surecast = 48, // 120.0 max
        Rescue = 49, // 120.0 max
    }

    public enum SID : uint
    {
        None = 0,
        Combust = 838, // applied by Combust to target, dot
        CombustII = 843, // applied by Combust II to target, dot
        CombustIII = 1881, // applied by Combust III to target, dot
        EnhancedBeneficII = 815, // applied by Benefic to self, guaranteed crit for bene 2
        AspectedBenefic = 835, // applied by Aspected Benefic to target, regen
        CollectiveUnconscious = 849, // applied by Collective Unconscious to party, damage reduction
        CollectiveUnconsciousChannel = 848, // applied by Collective Unconscious to self, removed upon movement
        EarthlyDominance = 1224, // applied by Earthly Star to self
        GiantDominance = 1248, // applied by Earthly Star to self
        Macrocosmos = 2718, // applied by Macrocosmos to party, compiles damage taken
        Intersection = 1889, // applied by Celestial Intersection to target, shield
        Exaltation = 2717, // applied by Exaltation to target, damage reduction
        NeutralSect = 1892, // applied by Neutral Sect to self, increases healing potency
        NeutralSectShield = 1921, // applied by Aspected Helios or Aspected Benefic to party, shield
        AspectedHelios = 836, // applied by Aspected Helios to party, regen
        ClarifyingDraw = 2713, // applied by Draw to self, allows redraw
        Lightspeed = 841, // applied by Lightspeed to self, cast speeds -2.5s
        SynastrySource = 845, // applied by Synastry to self
        SynastryTarget = 846, // applied by Synastry to target
        Divination = 1878, // applied by Divination to party, damage buff
        TheBalance = 1882, // applied by the Balance to target, damage buff
        TheBole = 1883, // applied by the Bole to target, damage buff
        TheArrow = 1884, // applied by the Arrow to target, damage buff
        TheSpear = 1885, // applied by the Spear to target, damage buff
        TheEwer = 1886, // applied by the Ewer to target, damage buff
        TheSpire = 1887, // applied by the Spire to target, damage buff
        HarmonyOfSpirit = 2714, // applied by Astrodyne to self, MP regen
        HarmonyOfBody = 2715, // applied by Astrodyne to self, haste
        HarmonyOfMind = 2716, // applied by Astrodyne to self, damage and healing potency increase
        Swiftcast = 167, // applied by Swiftcast to self
        Surecast = 160, // applied by Surecast to self
        Sleep = 3, // applied by Repose to target
        LucidDreaming = 1204, // applied by Lucid Dreaming to self
        Opposition = 1879, // applied by Celestial Opposition to party, regen
        Horoscope = 1890, // applied by Horoscope to party, delayed heal
        HoroscopeHelios = 1891, // applied by Helios, Aspected Helios to party, delayed heal
    }

    public static class Definitions
    {
        public static uint[] UnlockQuests = { 67551, 67553, 67554, 67558, 67560, 67561, 67949 };

        public static bool Unlocked(AID aid, int level, int questProgress)
        {
            return aid switch
            {
                AID.Benefic => level >= 2,
                AID.Combust => level >= 4,
                AID.Lightspeed => level >= 6,
                AID.Repose => level >= 8,
                AID.Esuna => level >= 10,
                AID.Helios => level >= 10,
                AID.Ascend => level >= 12,
                AID.LucidDreaming => level >= 14,
                AID.EssentialDignity => level >= 15,
                AID.Swiftcast => level >= 18,
                AID.BeneficII => level >= 26,
                AID.TheEwer => level >= 30,
                AID.TheSpire => level >= 30,
                AID.Draw => level >= 30,
                AID.TheBole => level >= 30,
                AID.Undraw => level >= 30,
                AID.Play => level >= 30,
                AID.TheSpear => level >= 30,
                AID.TheArrow => level >= 30,
                AID.TheBalance => level >= 30,
                AID.AspectedBenefic => level >= 34,
                AID.Redraw => level >= 40 && questProgress > 0,
                AID.AspectedHelios => level >= 42,
                AID.Surecast => level >= 44,
                AID.Gravity => level >= 45 && questProgress > 1,
                AID.CombustII => level >= 46,
                AID.Rescue => level >= 48,
                AID.Astrodyne => level >= 50,
                AID.Synastry => level >= 50 && questProgress > 2,
                AID.Divination => level >= 50,
                AID.MaleficII => level >= 54 && questProgress > 3,
                AID.CollectiveUnconscious => level >= 58 && questProgress > 4,
                AID.CelestialOpposition => level >= 60 && questProgress > 5,
                AID.EarthlyStar => level >= 62,
                AID.StellarDetonation => level >= 62,
                AID.MaleficIII => level >= 64,
                AID.MinorArcana => level >= 70 && questProgress > 6,
                AID.LadyOfCrowns => level >= 70 && questProgress > 6,
                AID.LordOfCrowns => level >= 70 && questProgress > 6,
                AID.MaleficIV => level >= 72,
                AID.CombustIII => level >= 72,
                AID.CelestialIntersection => level >= 74,
                AID.Horoscope => level >= 76,
                AID.HoroscopeEnd => level >= 76,
                AID.NeutralSect => level >= 80,
                AID.FallMalefic => level >= 82,
                AID.GravityII => level >= 82,
                AID.Exaltation => level >= 86,
                AID.Macrocosmos => level >= 90,
                AID.Microcosmos => level >= 90,
                _ => true,
            };
        }

        public static bool Unlocked(TraitID tid, int level, int questProgress)
        {
            return tid switch
            {
                TraitID.MaimAndMend => level >= 20,
                TraitID.EnhancedBenefic => level >= 36,
                TraitID.MaimAndMendII => level >= 40,
                TraitID.EnhancedDraw => level >= 40 && questProgress > 0,
                TraitID.CombustMastery => level >= 46,
                TraitID.EnhancedDrawII => level >= 50,
                TraitID.MaleficMastery => level >= 54 && questProgress > 3,
                TraitID.MaleficMasteryII => level >= 64,
                TraitID.HyperLightspeed => level >= 68,
                TraitID.CombustMasteryII => level >= 72,
                TraitID.MaleficMasteryIII => level >= 72,
                TraitID.EnhancedEssentialDignity => level >= 78,
                TraitID.MaleficMasteryIV => level >= 82,
                TraitID.GravityMastery => level >= 82,
                TraitID.EnhancedHealingMagic => level >= 85,
                TraitID.EnhancedCelestialIntersection => level >= 88,
                _ => true,
            };
        }

        public static Dictionary<ActionID, ActionDefinition> SupportedActions;

        static Definitions()
        {
            SupportedActions = CommonDefinitions.CommonActionData(CommonDefinitions.IDPotionMnd);
            SupportedActions.GCDCast(AID.Malefic, 25, 1.5f);
            SupportedActions.GCDCast(AID.Benefic, 30, 1.5f);
            SupportedActions.GCD(AID.Combust, 25);
            SupportedActions.OGCD(AID.Lightspeed, 0, CDGroup.Lightspeed, 120.0f);
            SupportedActions.GCDCast(AID.Repose, 30, 2.5f);
            SupportedActions.GCDCast(AID.Esuna, 30, 1.0f);
            SupportedActions.GCDCast(AID.Helios, 0, 1.5f);
            SupportedActions.GCDCast(AID.Ascend, 30, 8.0f);
            SupportedActions.OGCD(AID.LucidDreaming, 0, CDGroup.LucidDreaming, 60.0f);
            SupportedActions.OGCDWithCharges(AID.EssentialDignity, 30, CDGroup.EssentialDignity, 40.0f, 2);
            SupportedActions.OGCD(AID.Swiftcast, 0, CDGroup.Swiftcast, 60.0f);
            SupportedActions.GCDCast(AID.BeneficII, 30, 1.5f);
            SupportedActions.OGCD(AID.TheEwer, 30, CDGroup.Play, 1.0f);
            SupportedActions.OGCD(AID.TheSpire, 30, CDGroup.Play, 1.0f);
            SupportedActions.OGCDWithCharges(AID.Draw, 0, CDGroup.Draw, 30.0f, 2);
            SupportedActions.OGCD(AID.TheBole, 30, CDGroup.Play, 1.0f);
            SupportedActions.OGCD(AID.Undraw, 0, CDGroup.Undraw, 1.0f);
            SupportedActions.OGCD(AID.Play, 0, CDGroup.Play, 1.0f);
            SupportedActions.OGCD(AID.TheSpear, 30, CDGroup.Play, 1.0f);
            SupportedActions.OGCD(AID.TheArrow, 30, CDGroup.Play, 1.0f);
            SupportedActions.OGCD(AID.TheBalance, 30, CDGroup.Play, 1.0f);
            SupportedActions.GCD(AID.AspectedBenefic, 30);
            SupportedActions.OGCD(AID.Redraw, 0, CDGroup.Redraw, 1.0f);
            SupportedActions.GCDCast(AID.AspectedHelios, 0, 1.5f);
            SupportedActions.OGCD(AID.Surecast, 0, CDGroup.Surecast, 120.0f);
            SupportedActions.GCDCast(AID.Gravity, 25, 1.5f);
            SupportedActions.GCD(AID.CombustII, 25);
            SupportedActions.OGCD(AID.Rescue, 30, CDGroup.Rescue, 120.0f);
            SupportedActions.OGCD(AID.Astrodyne, 0, CDGroup.Astrodyne, 1.0f);
            SupportedActions.OGCD(AID.Synastry, 30, CDGroup.Synastry, 120.0f);
            SupportedActions.OGCD(AID.Divination, 0, CDGroup.Divination, 120.0f);
            SupportedActions.GCDCast(AID.MaleficII, 25, 1.5f);
            SupportedActions.OGCD(AID.CollectiveUnconscious, 0, CDGroup.CollectiveUnconscious, 60.0f);
            SupportedActions.OGCD(AID.CelestialOpposition, 0, CDGroup.CelestialOpposition, 60.0f);
            SupportedActions.OGCD(AID.EarthlyStar, 30, CDGroup.EarthlyStar, 60.0f);
            SupportedActions.OGCD(AID.StellarDetonation, 0, CDGroup.StellarDetonation, 3.0f);
            SupportedActions.GCDCast(AID.MaleficIII, 25, 1.5f);
            SupportedActions.OGCD(AID.MinorArcana, 0, CDGroup.MinorArcana, 60.0f);
            SupportedActions.OGCD(AID.LadyOfCrowns, 0, CDGroup.LadyOfCrowns, 1.0f);
            SupportedActions.OGCD(AID.LordOfCrowns, 0, CDGroup.LadyOfCrowns, 1.0f);
            SupportedActions.GCDCast(AID.MaleficIV, 25, 1.5f);
            SupportedActions.GCD(AID.CombustIII, 25);
            SupportedActions.OGCDWithCharges(AID.CelestialIntersection, 30, CDGroup.CelestialIntersection, 30.0f, 2);
            SupportedActions.OGCD(AID.Horoscope, 0, CDGroup.Horoscope, 60.0f);
            SupportedActions.OGCD(AID.HoroscopeEnd, 0, CDGroup.HoroscopeEnd, 1.0f);
            SupportedActions.OGCD(AID.NeutralSect, 0, CDGroup.NeutralSect, 120.0f);
            SupportedActions.GCDCast(AID.FallMalefic, 25, 1.5f);
            SupportedActions.GCDCast(AID.GravityII, 25, 1.5f);
            SupportedActions.OGCD(AID.Exaltation, 30, CDGroup.Exaltation, 60.0f);
            SupportedActions.OGCD(AID.Macrocosmos, 0, CDGroup.Macrocosmos, 180.0f);
            SupportedActions.OGCD(AID.Microcosmos, 0, CDGroup.Microcosmos, 1.0f);
        }
    }
}
