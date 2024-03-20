using System.Collections.Generic;

namespace BossMod.SGE
{
    public enum AID : uint
    {
        None = 0,

        // GCDs
        Dosis = 24283, // L1, 1.5s cast, range 25, single-target 0/0, targets=hostile, animLock=???
        Diagnosis = 24284, // L2, 1.5s cast, range 30, single-target 0/0, targets=self/party/friendly, animLock=0.100s
        Repose = 16560, // L8, 2.5s cast, range 30, single-target 0/0, targets=hostile, animLock=0.100s
        Prognosis = 24286, // L10, 2.0s cast, range 0, AOE circle 15/0, targets=self, animLock=0.100s
        Esuna = 7568, // L10, 1.0s cast, range 30, single-target 0/0, targets=self/party/friendly, animLock=0.100s
        Egeiro = 24287, // L12, 8.0s cast, range 30, single-target 0/0, targets=party/friendly, animLock=???
        EukrasianDosis = 24293, // L30, instant, range 25, single-target 0/0, targets=hostile, animLock=???
        Eukrasia = 24290, // L30, instant, range 0, single-target 0/0, targets=self, animLock=0.600s
        EukrasianDiagnosis = 24291, // L30, instant, range 30, single-target 0/0, targets=self/party/friendly, animLock=???
        EukrasianPrognosis = 24292, // L30, instant, range 0, AOE circle 15/0, targets=self, animLock=???
        Dyskrasia = 24297, // L46, instant, range 0, AOE circle 5/0, targets=self, animLock=???
        Toxikon = 24304, // L66, instant, range 25, AOE circle 5/0, targets=hostile, animLock=???
        EukrasianDosisII = 24308, // L72, instant, range 25, single-target 0/0, targets=hostile, animLock=???
        DosisII = 24306, // L72, 1.5s cast, range 25, single-target 0/0, targets=hostile, animLock=???
        DosisIII = 24312, // L82, 1.5s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        EukrasianDosisIII = 24314, // L82, instant, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        DyskrasiaII = 24315, // L82, instant, range 0, AOE circle 5/0, targets=self, animLock=0.600s
        ToxikonII = 24316, // L82, instant, range 25, AOE circle 5/0, targets=hostile, animLock=0.600s

        // oGCDs
        KardiaHeal = 28119, // L4, instant, 0.0s CD (group -1), range 30, single-target 0/0, targets=self/party, animLock=???
        Kardia = 24285, // L4, instant, 5.0s CD (group 1), range 30, single-target 0/0, targets=self/party, animLock=0.600s
        LucidDreaming = 7562, // L14, instant, 60.0s CD (group 45), range 0, single-target 0/0, targets=self, animLock=0.600s
        Swiftcast = 7561, // L18, instant, 60.0s CD (group 44), range 0, single-target 0/0, targets=self, animLock=0.600s
        Physis = 24288, // L20, instant, 60.0s CD (group 10), range 0, AOE circle 15/0, targets=self, animLock=???
        Phlegma = 24289, // L26, instant, 40.0s CD (group 18) (2 charges), range 6, AOE circle 5/0, targets=hostile, animLock=???
        Soteria = 24294, // L35, instant, 90.0s CD (group 14), range 0, single-target 0/0, targets=self, animLock=0.600s
        Icarus = 24295, // L40, instant, 45.0s CD (group 6), range 25, single-target 0/0, targets=party/hostile, animLock=???
        Surecast = 7559, // L44, instant, 120.0s CD (group 48), range 0, single-target 0/0, targets=self, animLock=0.600s
        Druochole = 24296, // L45, instant, 1.0s CD (group 0), range 30, single-target 0/0, targets=self/party/friendly, animLock=0.600s
        Rescue = 7571, // L48, instant, 120.0s CD (group 49), range 30, single-target 0/0, targets=party, animLock=???
        Kerachole = 24298, // L50, instant, 30.0s CD (group 3), range 0, AOE circle 30/0, targets=self, animLock=0.600s
        Ixochole = 24299, // L52, instant, 30.0s CD (group 4), range 0, AOE circle 15/0, targets=self, animLock=0.600s
        Zoe = 24300, // L56, instant, 120.0s CD (group 19), range 0, single-target 0/0, targets=self, animLock=0.600s
        Pepsis = 24301, // L58, instant, 30.0s CD (group 2), range 0, AOE circle 15/0, targets=self, animLock=0.600s
        PhysisII = 24302, // L60, instant, 60.0s CD (group 23), range 0, AOE circle 30/0, targets=self, animLock=0.600s
        Taurochole = 24303, // L62, instant, 45.0s CD (group 7), range 30, single-target 0/0, targets=self/party, animLock=0.600s
        Haima = 24305, // L70, instant, 120.0s CD (group 20), range 30, single-target 0/0, targets=self/party, animLock=0.600s
        PhlegmaII = 24307, // L72, instant, 40.0s CD (group 16) (2 charges), range 6, AOE circle 5/0, targets=hostile, animLock=???
        Rhizomata = 24309, // L74, instant, 90.0s CD (group 15), range 0, single-target 0/0, targets=self, animLock=0.600s
        Holos = 24310, // L76, instant, 120.0s CD (group 11), range 0, AOE circle 30/0, targets=self, animLock=0.600s
        Panhaima = 24311, // L80, instant, 120.0s CD (group 21), range 0, AOE circle 30/0, targets=self, animLock=0.600s
        PhlegmaIII = 24313, // L82, instant, 40.0s CD (group 17) (2 charges), range 6, AOE circle 5/0, targets=hostile, animLock=0.600s
        Krasis = 24317, // L86, instant, 60.0s CD (group 12), range 30, single-target 0/0, targets=self/party, animLock=0.600s
        PneumaHeal = 27524, // L90, instant, 0.0s CD (group -1), range 0, AOE circle 20/0, targets=self, animLock=0.100s
        Pneuma = 24318, // L90, 1.5s cast, 120.0s CD (group 22), range 25, AOE rect 25/4, targets=hostile, animLock=0.100s
    }

    public enum TraitID : uint
    {
        None = 0,
        MaimAndMend = 368, // L20
        MaimAndMendII = 369, // L40
        Addersgall = 370, // L45
        SomanouticOath = 371, // L54
        PhysisMastery = 510, // L60
        SomanouticOathII = 372, // L64
        Addersting = 373, // L66
        OffensiveMagicMastery = 374, // L72
        EnhancedKerachole = 375, // L78
        OffensiveMagicMasteryII = 376, // L82
        EnhancedHealingMagic = 377, // L85
        EnhancedZoe = 378, // L88
    }

    public enum CDGroup : int
    {
        Druochole = 0, // 1.0 max
        Kardia = 1, // 5.0 max
        Pepsis = 2, // 30.0 max
        Kerachole = 3, // 30.0 max
        Ixochole = 4, // 30.0 max
        Icarus = 6, // 45.0 max
        Taurochole = 7, // 45.0 max
        Physis = 10, // 60.0 max
        Holos = 11, // 120.0 max
        Krasis = 12, // 60.0 max
        Soteria = 14, // 90.0 max
        Rhizomata = 15, // 90.0 max
        PhlegmaII = 16, // 2*40.0 max
        PhlegmaIII = 17, // 2*40.0 max
        Phlegma = 18, // 2*40.0 max
        Zoe = 19, // 120.0 max
        Haima = 20, // 120.0 max
        Panhaima = 21, // 120.0 max
        Pneuma = 22, // 120.0 max
        PhysisII = 23, // 60.0 max
        Swiftcast = 44, // 60.0 max
        LucidDreaming = 45, // 60.0 max
        Surecast = 48, // 120.0 max
        Rescue = 49, // 120.0 max
    }

    public enum SID : uint
    {
        None = 0,
        Haima = 2612, // applied by Haima to self
        Haimatinon = 2642, // applied by Haima to self
        Panhaima = 2613, // applied by Panhaima to self
        Panhaimatinon = 2643, // applied by Panhaima to self
        Holosakos = 3365, // applied by Holos to self
        Holos = 3003, // applied by Holos to self
        Kerachole = 2618, // applied by Kerachole to self
        Kerakeia = 2938, // applied by Kerachole to self
        Taurochole = 2619, // applied by Taurochole to self
        PhysisII = 2620, // applied by Physis II to self
        Autophysis = 2621, // applied by Physis II to self
        EukrasianDiagnosis = 2607, // applied by Eukrasian Diagnosis to target
        DifferentialDiagnosis = 2608, // applied by crit Eukrasian Diagnosis to target
        EukrasianPrognosis = 2609, // applied by Eukrasian Prognosis to self
        EukrasianDosis = 2614, // applied by Eukrasian Dosis to target
        EukrasianDosisII = 2615, // applied by Eukrasian Dosis II to target
        EukrasianDosisIII = 2616, // applied by Eukrasian Dosis III to target
        Krasis = 2622, // applied by Krasis to self
        Soteria = 2610, // applied by Soteria to self
        Kardion = 2605, // applied by Kardia to self
        Kardia = 2604, // applied by Kardia to self
        Swiftcast = 167, // applied by Swiftcast to self
        LucidDreaming = 1204, // applied by Lucid Dreaming to self
        Surecast = 160, // applied by Surecast to self
        Sleep = 3, // applied by Repose to target
        Raise = 148, // applied by Egeiro to target
        Zoe = 2611, // applied by Zoe to self
    }

    public static class Definitions
    {
        public static uint[] UnlockQuests = { 69608 };

        public static bool Unlocked(AID aid, int level, int questProgress)
        {
            return aid switch
            {
                AID.Diagnosis => level >= 2,
                AID.KardiaHeal => level >= 4,
                AID.Kardia => level >= 4,
                AID.Repose => level >= 8,
                AID.Prognosis => level >= 10,
                AID.Esuna => level >= 10,
                AID.Egeiro => level >= 12,
                AID.LucidDreaming => level >= 14,
                AID.Swiftcast => level >= 18,
                AID.Physis => level >= 20,
                AID.Phlegma => level >= 26,
                AID.EukrasianDosis => level >= 30,
                AID.Eukrasia => level >= 30,
                AID.EukrasianDiagnosis => level >= 30,
                AID.EukrasianPrognosis => level >= 30,
                AID.Soteria => level >= 35,
                AID.Icarus => level >= 40,
                AID.Surecast => level >= 44,
                AID.Druochole => level >= 45,
                AID.Dyskrasia => level >= 46,
                AID.Rescue => level >= 48,
                AID.Kerachole => level >= 50,
                AID.Ixochole => level >= 52,
                AID.Zoe => level >= 56,
                AID.Pepsis => level >= 58,
                AID.PhysisII => level >= 60,
                AID.Taurochole => level >= 62,
                AID.Toxikon => level >= 66,
                AID.Haima => level >= 70,
                AID.EukrasianDosisII => level >= 72,
                AID.DosisII => level >= 72,
                AID.PhlegmaII => level >= 72,
                AID.Rhizomata => level >= 74,
                AID.Holos => level >= 76,
                AID.Panhaima => level >= 80 && questProgress > 0,
                AID.DosisIII => level >= 82,
                AID.PhlegmaIII => level >= 82,
                AID.EukrasianDosisIII => level >= 82,
                AID.DyskrasiaII => level >= 82,
                AID.ToxikonII => level >= 82,
                AID.Krasis => level >= 86,
                AID.PneumaHeal => level >= 90,
                AID.Pneuma => level >= 90,
                _ => true,
            };
        }

        public static bool Unlocked(TraitID tid, int level, int questProgress)
        {
            return tid switch
            {
                TraitID.MaimAndMend => level >= 20,
                TraitID.MaimAndMendII => level >= 40,
                TraitID.Addersgall => level >= 45,
                TraitID.SomanouticOath => level >= 54,
                TraitID.PhysisMastery => level >= 60,
                TraitID.SomanouticOathII => level >= 64,
                TraitID.Addersting => level >= 66,
                TraitID.OffensiveMagicMastery => level >= 72,
                TraitID.EnhancedKerachole => level >= 78,
                TraitID.OffensiveMagicMasteryII => level >= 82,
                TraitID.EnhancedHealingMagic => level >= 85,
                TraitID.EnhancedZoe => level >= 88,
                _ => true,
            };
        }

        public static Dictionary<ActionID, ActionDefinition> SupportedActions;

        static Definitions()
        {
            SupportedActions = CommonDefinitions.CommonActionData(CommonDefinitions.IDPotionMnd);
            SupportedActions.GCDCast(AID.Dosis, 25, 1.5f);
            SupportedActions.GCDCast(AID.Diagnosis, 30, 1.5f);
            SupportedActions.OGCD(AID.Kardia, 30, CDGroup.Kardia, 5.0f);
            SupportedActions.GCDCast(AID.Repose, 30, 2.5f);
            SupportedActions.GCDCast(AID.Prognosis, 0, 2.0f);
            SupportedActions.GCDCast(AID.Esuna, 30, 1.0f);
            SupportedActions.GCDCast(AID.Egeiro, 30, 8.0f);
            SupportedActions.OGCD(AID.LucidDreaming, 0, CDGroup.LucidDreaming, 60.0f);
            SupportedActions.OGCD(AID.Swiftcast, 0, CDGroup.Swiftcast, 60.0f);
            SupportedActions.OGCD(AID.Physis, 0, CDGroup.Physis, 60.0f).EffectDuration = 15;
            SupportedActions.OGCDWithCharges(AID.Phlegma, 6, CDGroup.Phlegma, 40.0f, 2);
            SupportedActions.GCD(AID.EukrasianDosis, 25);
            SupportedActions.GCD(AID.Eukrasia, 0);
            SupportedActions.GCD(AID.EukrasianDiagnosis, 30);
            SupportedActions.GCD(AID.EukrasianPrognosis, 0);
            SupportedActions.OGCD(AID.Soteria, 0, CDGroup.Soteria, 90.0f).EffectDuration = 15;
            SupportedActions.OGCD(AID.Icarus, 25, CDGroup.Icarus, 45.0f);
            SupportedActions.OGCD(AID.Surecast, 0, CDGroup.Surecast, 120.0f);
            SupportedActions.OGCD(AID.Druochole, 30, CDGroup.Druochole, 1.0f);
            SupportedActions.GCD(AID.Dyskrasia, 0);
            SupportedActions.OGCD(AID.Rescue, 30, CDGroup.Rescue, 120.0f);
            SupportedActions.OGCD(AID.Kerachole, 0, CDGroup.Kerachole, 30.0f).EffectDuration = 15;
            SupportedActions.OGCD(AID.Ixochole, 0, CDGroup.Ixochole, 30.0f);
            SupportedActions.OGCD(AID.Zoe, 0, CDGroup.Zoe, 120.0f).EffectDuration = 30;
            SupportedActions.OGCD(AID.Pepsis, 0, CDGroup.Pepsis, 30.0f);
            SupportedActions.OGCD(AID.PhysisII, 0, CDGroup.PhysisII, 60.0f).EffectDuration = 15;
            SupportedActions.OGCD(AID.Taurochole, 30, CDGroup.Taurochole, 45.0f).EffectDuration =
                15;
            SupportedActions.GCD(AID.Toxikon, 25);
            SupportedActions.OGCD(AID.Haima, 30, CDGroup.Haima, 120.0f).EffectDuration = 15;
            SupportedActions.GCD(AID.EukrasianDosisII, 25);
            SupportedActions.GCDCast(AID.DosisII, 25, 1.5f);
            SupportedActions.OGCDWithCharges(AID.PhlegmaII, 6, CDGroup.PhlegmaII, 40.0f, 2);
            SupportedActions.OGCD(AID.Rhizomata, 0, CDGroup.Rhizomata, 90.0f);
            SupportedActions.OGCD(AID.Holos, 0, CDGroup.Holos, 120.0f).EffectDuration = 30;
            SupportedActions.OGCD(AID.Panhaima, 0, CDGroup.Panhaima, 120.0f).EffectDuration = 15;
            SupportedActions.GCDCast(AID.DosisIII, 25, 1.5f);
            SupportedActions.OGCDWithCharges(AID.PhlegmaIII, 6, CDGroup.PhlegmaIII, 40.0f, 2);
            SupportedActions.GCD(AID.EukrasianDosisIII, 25);
            SupportedActions.GCD(AID.DyskrasiaII, 0);
            SupportedActions.GCD(AID.ToxikonII, 25);
            SupportedActions.OGCD(AID.Krasis, 30, CDGroup.Krasis, 60.0f).EffectDuration = 10;
            SupportedActions.OGCDCast(AID.Pneuma, 25, 1.5f, CDGroup.Pneuma, 120.0f);
        }
    }
}
