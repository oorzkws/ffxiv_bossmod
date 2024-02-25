using System.Collections.Generic;

namespace BossMod.RDM
{
    public enum AID : uint
    {
        None = 0,

        // GCDs
        Riposte = 7504, // L1, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        EnchantedRiposte = 7527, // L1, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        Jolt = 7503, // L2, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=???
        Verthunder = 7505, // L4, 5.0s cast, range 25, single-target 0/0, targets=hostile, animLock=???
        Sleep = 25880, // L10, 2.5s cast, range 30, AOE circle 5/0, targets=hostile, animLock=0.100s
        Veraero = 7507, // L10, 5.0s cast, range 25, single-target 0/0, targets=hostile, animLock=???
        Scatter = 7509, // L15, 5.0s cast, range 25, AOE circle 5/0, targets=hostile, animLock=???
        VerthunderII = 16524, // L18, 2.0s cast, range 25, AOE circle 5/0, targets=hostile, animLock=???
        VeraeroII = 16525, // L22, 2.0s cast, range 25, AOE circle 5/0, targets=hostile, animLock=???
        Verfire = 7510, // L26, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=???
        Verstone = 7511, // L30, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        Zwerchhau = 7512, // L35, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        EnchantedZwerchhau = 7528, // L35, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        EnchantedRedoublement = 7529, // L50, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        Redoublement = 7516, // L50, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        EnchantedMoulinet = 7530, // L52, instant, range 8, AOE cone 8/0, targets=hostile, animLock=0.600s
        Moulinet = 7513, // L52, instant, range 8, AOE cone 8/0, targets=hostile, animLock=???
        Vercure = 7514, // L54, 2.0s cast, range 30, single-target 0/0, targets=self/party/friendly, animLock=???
        JoltII = 7524, // L62, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        Verraise = 7523, // L64, 10.0s cast, range 30, single-target 0/0, targets=party/friendly, animLock=???
        Impact = 16526, // L66, 5.0s cast, range 25, AOE circle 5/0, targets=hostile, animLock=???
        Verflare = 7525, // L68, instant, range 25, AOE circle 5/0, targets=hostile, animLock=0.600s
        Verholy = 7526, // L70, instant, range 25, AOE circle 5/0, targets=hostile, animLock=???
        EnchantedReprise = 16528, // L76, instant, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        Reprise = 16529, // L76, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        Scorch = 16530, // L80, instant, range 25, AOE circle 5/0, targets=hostile, animLock=0.600s
        VerthunderIII = 25855, // L82, 5.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        VeraeroIII = 25856, // L82, 5.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        Resolution = 25858, // L90, instant, range 25, AOE rect 25/4, targets=hostile, animLock=0.600s

        // oGCDs
        CorpsACorps = 7506, // L6, instant, 35.0s CD (group 10) (2 charges), range 25, single-target 0/0, targets=hostile, animLock=0.600s
        Addle = 7560, // L8, instant, 90.0s CD (group 46), range 25, single-target 0/0, targets=hostile, animLock=0.600s
        LucidDreaming = 7562, // L14, instant, 60.0s CD (group 45), range 0, single-target 0/0, targets=self, animLock=0.600s
        Swiftcast = 7561, // L18, instant, 60.0s CD (group 44), range 0, single-target 0/0, targets=self, animLock=0.600s
        Engagement = 16527, // L40, instant, 35.0s CD (group 12) (2 charges), range 3, single-target 0/0, targets=hostile, animLock=0.600s
        Displacement = 7515, // L40, instant, 35.0s CD (group 12) (2 charges), range 5, single-target 0/0, targets=hostile, animLock=???
        Surecast = 7559, // L44, instant, 120.0s CD (group 48), range 0, single-target 0/0, targets=self, animLock=0.600s
        Fleche = 7517, // L45, instant, 25.0s CD (group 4), range 25, single-target 0/0, targets=hostile, animLock=???
        Acceleration = 7518, // L50, instant, 55.0s CD (group 19) (2 charges), range 0, single-target 0/0, targets=self, animLock=0.600s
        ContreSixte = 7519, // L56, instant, 45.0s CD (group 7), range 25, AOE circle 6/0, targets=hostile, animLock=???
        Embolden = 7520, // L58, instant, 120.0s CD (group 20), range 0, AOE circle 30/0, targets=self, animLock=0.600s
        Manafication = 7521, // L60, instant, 120.0s CD (group 21), range 0, single-target 0/0, targets=self, animLock=0.600s
        MagickBarrier = 25857, // L86, instant, 120.0s CD (group 23), range 0, AOE circle 30/0, targets=self, animLock=0.600s
    }

    public enum TraitID : uint
    {
        None = 0,
        Dualcast = 216, // L1
        MaimAndMend = 200, // L20
        MaimAndMendII = 201, // L40
        EnhancedJolt = 195, // L62
        ScatterMastery = 303, // L66
        ManaStack = 482, // L68
        EnhancedDisplacement = 304, // L72
        RedMagicMastery = 306, // L74
        EnhancedManafication = 305, // L78
        RedMagicMasteryII = 483, // L82
        RedMagicMasteryIII = 484, // L84
        EnhancedAcceleration = 485, // L88
        EnhancedManaficationII = 486, // L90
    }

    public enum SID : uint
    {
        None = 0,
        Embolden = 1239, // applied by Embolden to self
        Addle = 1203, // applied by Addle to target
        MagickBarrier = 2707, // applied by Magick Barrier to self
        Sleep = 3, // applied by Sleep to target
        Acceleration = 1238, // applied by Acceleration to self
        Manafication = 1971, // applied by Manafication to self
        Surecast = 160, // applied by Surecast to self
        LucidDreaming = 1204, // applied by Lucid Dreaming to self
        Swiftcast = 167, // applied by Swiftcast to self
        VerstoneReady = 1235, // applied by Veraero III to self
        VerfireReady = 1234, // applied by Verthunder III to self
        Sprint = 50, // applied by Sprint to self
        Dualcast = 1249, // applied by any non-instant cast spell to self
    }

    public enum CDGroup : int
    {
        Fleche = 4, // 25.0 max
        ContreSixte = 7, // 45.0 max
        CorpsACorps = 10, // 2*35.0 max
        Engagement = 12, // 2*35.0 max, shared by Engagement, Displacement
        Acceleration = 19, // 2*55.0 max
        Embolden = 20, // 120.0 max
        Manafication = 21, // 120.0 max
        MagickBarrier = 23, // 120.0 max
        Swiftcast = 44, // 60.0 max
        LucidDreaming = 45, // 60.0 max
        Addle = 46, // 90.0 max
        Surecast = 48, // 120.0 max
    }

    public static class Definitions
    {
        public static uint[] UnlockQuests = { 68118, 68123 };

        public static bool Unlocked(AID aid, int level, int questProgress)
        {
            return aid switch
            {
                AID.Jolt => level >= 2,
                AID.Verthunder => level >= 4,
                AID.CorpsACorps => level >= 6,
                AID.Addle => level >= 8,
                AID.Sleep => level >= 10,
                AID.Veraero => level >= 10,
                AID.LucidDreaming => level >= 14,
                AID.Scatter => level >= 15,
                AID.VerthunderII => level >= 18,
                AID.Swiftcast => level >= 18,
                AID.VeraeroII => level >= 22,
                AID.Verfire => level >= 26,
                AID.Verstone => level >= 30,
                AID.Zwerchhau => level >= 35,
                AID.EnchantedZwerchhau => level >= 35,
                AID.Engagement => level >= 40,
                AID.Displacement => level >= 40,
                AID.Surecast => level >= 44,
                AID.Fleche => level >= 45,
                AID.EnchantedRedoublement => level >= 50,
                AID.Acceleration => level >= 50,
                AID.Redoublement => level >= 50,
                AID.EnchantedMoulinet => level >= 52,
                AID.Moulinet => level >= 52,
                AID.Vercure => level >= 54,
                AID.ContreSixte => level >= 56,
                AID.Embolden => level >= 58,
                AID.Manafication => level >= 60 && questProgress > 0,
                AID.JoltII => level >= 62,
                AID.Verraise => level >= 64,
                AID.Impact => level >= 66,
                AID.Verflare => level >= 68,
                AID.Verholy => level >= 70 && questProgress > 1,
                AID.EnchantedReprise => level >= 76,
                AID.Reprise => level >= 76,
                AID.Scorch => level >= 80,
                AID.VerthunderIII => level >= 82,
                AID.VeraeroIII => level >= 82,
                AID.MagickBarrier => level >= 86,
                AID.Resolution => level >= 90,
                _ => true,
            };
        }

        public static bool Unlocked(TraitID tid, int level, int questProgress)
        {
            return tid switch
            {
                TraitID.MaimAndMend => level >= 20,
                TraitID.MaimAndMendII => level >= 40,
                TraitID.EnhancedJolt => level >= 62,
                TraitID.ScatterMastery => level >= 66,
                TraitID.ManaStack => level >= 68,
                TraitID.EnhancedDisplacement => level >= 72,
                TraitID.RedMagicMastery => level >= 74,
                TraitID.EnhancedManafication => level >= 78,
                TraitID.RedMagicMasteryII => level >= 82,
                TraitID.RedMagicMasteryIII => level >= 84,
                TraitID.EnhancedAcceleration => level >= 88,
                TraitID.EnhancedManaficationII => level >= 90,
                _ => true,
            };
        }

        public static Dictionary<ActionID, ActionDefinition> SupportedActions;

        static Definitions()
        {
            SupportedActions = CommonDefinitions.CommonActionData(CommonDefinitions.IDPotionInt);
            SupportedActions.GCD(AID.Riposte, 3);
            SupportedActions.GCD(AID.EnchantedRiposte, 3);
            SupportedActions.GCDCast(AID.Jolt, 25, 2.0f);
            SupportedActions.GCDCast(AID.Verthunder, 25, 5.0f);
            SupportedActions.OGCDWithCharges(AID.CorpsACorps, 25, CDGroup.CorpsACorps, 35.0f, 2);
            SupportedActions.OGCD(AID.Addle, 25, CDGroup.Addle, 90.0f).EffectDuration = 10;
            SupportedActions.GCDCast(AID.Sleep, 30, 2.5f);
            SupportedActions.GCDCast(AID.Veraero, 25, 5.0f);
            SupportedActions.OGCD(AID.LucidDreaming, 0, CDGroup.LucidDreaming, 60.0f);
            SupportedActions.GCDCast(AID.Scatter, 25, 5.0f);
            SupportedActions.GCDCast(AID.VerthunderII, 25, 2.0f);
            SupportedActions.OGCD(AID.Swiftcast, 0, CDGroup.Swiftcast, 60.0f);
            SupportedActions.GCDCast(AID.VeraeroII, 25, 2.0f);
            SupportedActions.GCDCast(AID.Verfire, 25, 2.0f);
            SupportedActions.GCDCast(AID.Verstone, 25, 2.0f);
            SupportedActions.GCD(AID.Zwerchhau, 3);
            SupportedActions.GCD(AID.EnchantedZwerchhau, 3);
            SupportedActions.OGCDWithCharges(AID.Engagement, 3, CDGroup.Engagement, 35.0f, 2);
            SupportedActions.OGCDWithCharges(AID.Displacement, 5, CDGroup.Engagement, 35.0f, 2);
            SupportedActions.OGCD(AID.Surecast, 0, CDGroup.Surecast, 120.0f).EffectDuration = 6;
            SupportedActions.OGCD(AID.Fleche, 25, CDGroup.Fleche, 25.0f);
            SupportedActions.GCD(AID.EnchantedRedoublement, 3);
            SupportedActions.OGCDWithCharges(AID.Acceleration, 0, CDGroup.Acceleration, 55.0f, 2);
            SupportedActions.GCD(AID.Redoublement, 3);
            SupportedActions.GCD(AID.EnchantedMoulinet, 8);
            SupportedActions.GCD(AID.Moulinet, 8);
            SupportedActions.GCDCast(AID.Vercure, 30, 2.0f);
            SupportedActions.OGCD(AID.ContreSixte, 25, CDGroup.ContreSixte, 45.0f);
            SupportedActions.OGCD(AID.Embolden, 0, CDGroup.Embolden, 120.0f);
            SupportedActions.OGCD(AID.Manafication, 0, CDGroup.Manafication, 120.0f);
            SupportedActions.GCDCast(AID.JoltII, 25, 2.0f);
            SupportedActions.GCDCast(AID.Verraise, 30, 10.0f);
            SupportedActions.GCDCast(AID.Impact, 25, 5.0f);
            SupportedActions.GCD(AID.Verflare, 25);
            SupportedActions.GCD(AID.Verholy, 25);
            SupportedActions.GCD(AID.EnchantedReprise, 25);
            SupportedActions.GCD(AID.Reprise, 3);
            SupportedActions.GCD(AID.Scorch, 25);
            SupportedActions.GCDCast(AID.VerthunderIII, 25, 5.0f, 0.600f);
            SupportedActions.GCDCast(AID.VeraeroIII, 25, 5.0f, 0.600f);
            SupportedActions.OGCD(AID.MagickBarrier, 0, CDGroup.MagickBarrier, 120.0f).EffectDuration = 10;
            SupportedActions.GCD(AID.Resolution, 25);
        }

        public static bool IsAccelerated(this AID action) => action is AID.Veraero or AID.VeraeroIII or AID.Verthunder or AID.VerthunderIII or AID.Scatter or AID.Impact;
    }
}
