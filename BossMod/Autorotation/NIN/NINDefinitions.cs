using System.Collections.Generic;

namespace BossMod.NIN
{
    public enum AID : uint
    {
        None = 0,

        // GCDs
        SpinningEdge = 2240, // L1, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        GustSlash = 2242, // L4, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        ThrowingDagger = 2247, // L15, instant, range 20, single-target 0/0, targets=hostile, animLock=???
        AeolianEdge = 2255, // L26, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        Ninjutsu = 2260, // L30, instant, range 0, single-target 0/0, targets=self, animLock=???
        FumaTen = 18873, // L30, instant, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        Ten2 = 18805, // L30, instant, range 0, single-target 0/0, targets=self, animLock=0.350s
        FumaChi = 18874, // L30, instant, range 25, single-target 0/0, targets=hostile, animLock=???
        RabbitMedium = 2272, // L30, instant, range 0, single-target 0/0, targets=self, animLock=???
        FumaJin = 18875, // L30, instant, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        FumaShuriken = 2265, // L30, instant, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        Raiton = 2267, // L35, instant, range 20, single-target 0/0, targets=hostile, animLock=0.600s
        Katon = 2266, // L35, instant, range 20, AOE circle 5/0, targets=hostile, animLock=0.600s
        Chi2 = 18806, // L35, instant, range 0, single-target 0/0, targets=self, animLock=0.350s
        TCJKaton = 18876, // L35, instant, range 20, AOE circle 5/0, targets=hostile, animLock=0.600s
        TCJRaiton = 18877, // L35, instant, range 20, single-target 0/0, targets=hostile, animLock=0.600s
        DeathBlossom = 2254, // L38, instant, range 0, AOE circle 5/0, targets=self, animLock=???
        TCJDoton = 18880, // L45, instant, range 0, Ground circle 5/0, targets=self, animLock=0.600s
        TCJHuton = 18879, // L45, instant, range 0, single-target 0/0, targets=self, animLock=???
        TCJHyoton = 18878, // L45, instant, range 25, single-target 0/0, targets=hostile, animLock=???
        Hyoton = 2268, // L45, instant, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        Jin2 = 18807, // L45, instant, range 0, single-target 0/0, targets=self, animLock=0.350s
        Suiton = 2271, // L45, instant, range 20, single-target 0/0, targets=hostile, animLock=0.600s
        Doton = 2270, // L45, instant, range 0, Ground circle 5/0, targets=self, animLock=0.600s
        Huton = 2269, // L45, instant, range 0, single-target 0/0, targets=self, animLock=0.600s
        TCJSuiton = 18881, // L45, instant, range 20, single-target 0/0, targets=hostile, animLock=0.600s
        HakkeMujinsatsu = 16488, // L52, instant, range 0, AOE circle 5/0, targets=self, animLock=???
        ArmorCrush = 3563, // L54, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        Huraijin = 25876, // L60, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        HyoshoRanryu = 16492, // L76, instant, range 25, single-target 0/0, targets=hostile, animLock=???
        GokaMekkyaku = 16491, // L76, instant, range 20, AOE circle 5/0, targets=hostile, animLock=???
        PhantomKamaitachi = 25774, // L82, instant, range 20, single-target 0/0, targets=hostile, animLock=???
        ForkedRaiju = 25777, // L90, instant, range 20, single-target 0/0, targets=hostile, animLock=0.600s
        FleetingRaiju = 25778, // L90, instant, range 3, single-target 0/0, targets=hostile, animLock=???

        // oGCDs
        ShadeShift = 2241, // L2, instant, 120.0s CD (group 20), range 0, single-target 0/0, targets=self, animLock=???
        SecondWind = 7541, // L8, instant, 120.0s CD (group 49), range 0, single-target 0/0, targets=self, animLock=???
        Hide = 2245, // L10, instant, 20.0s CD (group 1), range 0, single-target 0/0, targets=self, animLock=0.600s
        LegSweep = 7863, // L10, instant, 40.0s CD (group 41), range 3, single-target 0/0, targets=hostile, animLock=???
        Bloodbath = 7542, // L12, instant, 90.0s CD (group 46), range 0, single-target 0/0, targets=self, animLock=???
        Mug = 2248, // L15, instant, 120.0s CD (group 21), range 3, single-target 0/0, targets=hostile, animLock=???
        TrickAttack = 2258, // L18, instant, 60.0s CD (group 11), range 3, single-target 0/0, targets=hostile, animLock=???
        Feint = 7549, // L22, instant, 90.0s CD (group 47), range 10, single-target 0/0, targets=hostile, animLock=???
        Ten = 2259, // L30, instant, 20.0s CD (group 8) (2 charges), range 0, single-target 0/0, targets=self, animLock=0.350s
        ArmsLength = 7548, // L32, instant, 120.0s CD (group 48), range 0, single-target 0/0, targets=self, animLock=???
        Chi = 2261, // L35, instant, 20.0s CD (group 8) (2 charges), range 0, single-target 0/0, targets=self, animLock=0.350s
        Assassinate = 2246, // L40, instant, 60.0s CD (group 9), range 3, single-target 0/0, targets=hostile, animLock=???
        Shukuchi = 2262, // L40, instant, 60.0s CD (group 16) (2 charges), range 20, Ground circle 1/0, targets=area, animLock=???
        Jin = 2263, // L45, instant, 20.0s CD (group 8) (2 charges), range 0, single-target 0/0, targets=self, animLock=0.350s
        TrueNorth = 7546, // L50, instant, 45.0s CD (group 45) (2 charges), range 0, single-target 0/0, targets=self, animLock=0.600s
        Kassatsu = 2264, // L50, instant, 60.0s CD (group 13), range 0, single-target 0/0, targets=self, animLock=???
        DreamWithinADream = 3566, // L56, instant, 60.0s CD (group 15), range 3, single-target 0/0, targets=hostile, animLock=???
        HellfrogMedium = 7401, // L62, instant, 1.0s CD (group 0), range 25, AOE circle 6/0, targets=hostile, animLock=???
        Bhavacakra = 7402, // L68, instant, 1.0s CD (group 0), range 3, single-target 0/0, targets=hostile, animLock=???
        TenChiJin = 7403, // L70, instant, 120.0s CD (group 19), range 0, single-target 0/0, targets=self, animLock=0.600s
        Meisui = 16489, // L72, instant, 120.0s CD (group 22), range 0, single-target 0/0, targets=self, animLock=???
        Bunshin = 16493, // L80, instant, 90.0s CD (group 14), range 0, single-target 0/0, targets=self, animLock=???

        // actions performed by shadow during bunshin; these are just included for completeness' sake as they can't be directly used
        ShadowHuraijin = 25877, // L60, instant, 0.0s CD (group 3), range 100, single-target 0/0, targets=hostile, animLock=???
        ShadowDeathBlossom = 17419, // L80, instant, 0.0s CD (group 3), range 0, AOE circle 5/0, targets=self, animLock=???
        ShadowThrowingDagger = 17418, // L80, instant, 0.0s CD (group 3), range 100, single-target 0/0, targets=hostile, animLock=???
        ShadowArmorCrush = 17417, // L80, instant, 0.0s CD (group 3), range 100, single-target 0/0, targets=hostile, animLock=???
        ShadowAeolianEdge = 17415, // L80, instant, 0.0s CD (group 3), range 100, single-target 0/0, targets=hostile, animLock=???
        ShadowGustSlash = 17414, // L80, instant, 0.0s CD (group 3), range 100, single-target 0/0, targets=hostile, animLock=???
        ShadowSpinningEdge = 17413, // L80, instant, 0.0s CD (group 3), range 100, single-target 0/0, targets=hostile, animLock=???
        ShadowHakkeMujinsatsu = 17420, // L80, instant, 0.0s CD (group 3), range 0, AOE circle 5/0, targets=self, animLock=???
        ShadowPhantomKamaitachi = 25775, // L82, instant, 0.0s CD (group 3), range 100, AOE circle 5/0, targets=hostile, animLock=???
        ShadowForkedRaiju = 25878, // L90, instant, 0.0s CD (group 3), range 100, single-target 0/0, targets=hostile, animLock=???
        ShadowFleetingRaiju = 25879, // L90, instant, 0.0s CD (group 3), range 100, single-target 0/0, targets=hostile, animLock=???

        // not performed by shadow but still automatic
        HollowNozuchi = 25776, // L86, instant, 0.0s CD (group 3), range 100, AOE circle 5/0, targets=self/area/!dead, animLock=???
    }

    public enum TraitID : uint
    {
        None = 0,
        AllFours = 90, // L14
        FleetOfFoot = 93, // L20
        AdeptAssassination = 515, // L56
        Shukiho = 165, // L62
        EnhancedShukuchi = 166, // L64
        EnhancedMug = 167, // L66
        EnhancedShukuchiII = 279, // L74
        MeleeMastery = 516, // L74
        EnhancedKassatsu = 250, // L76
        ShukihoII = 280, // L78
        ShukihoIII = 439, // L84
        MeleeMasteryII = 522, // L84
        EnhancedMeisui = 440, // L88
        EnhancedRaiton = 441, // L90
    }

    public enum SID : uint
    {
        None = 0,
        ShadeShift = 488, // applied by Shade Shift to self
        Hidden = 614, // applied by Hide to self
        Stun = 2, // applied by Leg Sweep to target
        Bloodbath = 84, // applied by Bloodbath to self
        VulnerabilityUp = 638, // applied by Mug to target
        Mudra = 496, // applied by Chi, Ten, Ten, Chi, Jin, Jin to self
        TrickAttack = 3254, // applied by Trick Attack to target
        Feint = 1195, // applied by Feint to target
        TrueNorth = 1250, // applied by True North to self
        Bunshin = 1954, // applied by Bunshin to self
        PhantomKamaitachiReady = 2723, // applied by Bunshin to self
        Kassatsu = 497, // applied by Kassatsu to self
        TenChiJin = 1186, // applied by Ten Chi Jin, Fuma Shuriken, Raiton to self
        Bind = 13, // applied by Hyoton to target
        RaijuReady = 2690, // applied by Raiton, Raiton to self
        Suiton = 507, // applied by Suiton, Suiton to self
        Meisui = 2689, // applied by Meisui to self
        Doton = 501, // applied by Doton to self
        ArmsLength = 1209, // applied by Arm's Length to self
    }

    public enum CDGroup : int
    {
        HellfrogMedium = 0, // 1.0 max, shared by Hellfrog Medium, Bhavacakra
        Hide = 1, // 20.0 max
        Ten = 8, // 2*20.0 max, shared by Ten, Chi, Jin
        Assassinate = 9, // 60.0 max
        TrickAttack = 11, // 60.0 max
        Kassatsu = 13, // 60.0 max
        Bunshin = 14, // 90.0 max
        DreamWithinADream = 15, // 60.0 max
        Shukuchi = 16, // 2*60.0 max
        TenChiJin = 19, // 120.0 max
        ShadeShift = 20, // 120.0 max
        Mug = 21, // 120.0 max
        Meisui = 22, // 120.0 max
        LegSweep = 41, // 40.0 max
        TrueNorth = 45, // 2*45.0 max
        Bloodbath = 46, // 90.0 max
        Feint = 47, // 90.0 max
        ArmsLength = 48, // 120.0 max
        SecondWind = 49, // 120.0 max
    }

    public static class Definitions
    {
        public static uint[] UnlockQuests =
        {
            65680,
            65681,
            65748,
            65750,
            65752,
            65768,
            65770,
            67220,
            67221,
            67222,
            67224,
            68488
        };

        public static bool Unlocked(AID aid, int level, int questProgress)
        {
            return aid switch
            {
                AID.ShadeShift => level >= 2,
                AID.GustSlash => level >= 4,
                AID.SecondWind => level >= 8,
                AID.Hide => level >= 10,
                AID.LegSweep => level >= 10,
                AID.Bloodbath => level >= 12,
                AID.ThrowingDagger => level >= 15 && questProgress > 0,
                AID.Mug => level >= 15 && questProgress > 1,
                AID.TrickAttack => level >= 18,
                AID.Feint => level >= 22,
                AID.AeolianEdge => level >= 26,
                AID.Ninjutsu => level >= 30 && questProgress > 2,
                AID.FumaShuriken => level >= 30 && questProgress > 2,
                AID.Ten => level >= 30 && questProgress > 2,
                AID.RabbitMedium => level >= 30 && questProgress > 2,
                AID.ArmsLength => level >= 32,
                AID.Raiton => level >= 35 && questProgress > 3,
                AID.Katon => level >= 35 && questProgress > 3,
                AID.Chi => level >= 35 && questProgress > 3,
                AID.DeathBlossom => level >= 38,
                AID.Assassinate => level >= 40,
                AID.Shukuchi => level >= 40 && questProgress > 4,
                AID.Doton => level >= 45 && questProgress > 5,
                AID.Huton => level >= 45 && questProgress > 5,
                AID.Hyoton => level >= 45 && questProgress > 5,
                AID.Jin => level >= 45 && questProgress > 5,
                AID.Suiton => level >= 45 && questProgress > 5,
                AID.TrueNorth => level >= 50,
                AID.Kassatsu => level >= 50 && questProgress > 6,
                AID.HakkeMujinsatsu => level >= 52 && questProgress > 7,
                AID.ArmorCrush => level >= 54 && questProgress > 8,
                AID.DreamWithinADream => level >= 56 && questProgress > 9,
                AID.Huraijin => level >= 60 && questProgress > 10,
                AID.HellfrogMedium => level >= 62,
                AID.Bhavacakra => level >= 68,
                AID.TenChiJin => level >= 70 && questProgress > 11,
                AID.Meisui => level >= 72,
                AID.HyoshoRanryu => level >= 76,
                AID.GokaMekkyaku => level >= 76,
                AID.Bunshin => level >= 80,
                AID.SpinningEdge => level >= 80,
                AID.PhantomKamaitachi => level >= 82,
                AID.ForkedRaiju => level >= 90,
                AID.FleetingRaiju => level >= 90,
                _ => true,
            };
        }

        public static bool Unlocked(TraitID tid, int level, int questProgress)
        {
            return tid switch
            {
                TraitID.AllFours => level >= 14,
                TraitID.FleetOfFoot => level >= 20,
                TraitID.AdeptAssassination => level >= 56 && questProgress > 9,
                TraitID.Shukiho => level >= 62,
                TraitID.EnhancedShukuchi => level >= 64,
                TraitID.EnhancedMug => level >= 66,
                TraitID.EnhancedShukuchiII => level >= 74,
                TraitID.MeleeMastery => level >= 74,
                TraitID.EnhancedKassatsu => level >= 76,
                TraitID.ShukihoII => level >= 78,
                TraitID.ShukihoIII => level >= 84,
                TraitID.MeleeMasteryII => level >= 84,
                TraitID.EnhancedMeisui => level >= 88,
                TraitID.EnhancedRaiton => level >= 90,
                _ => true,
            };
        }

        public static Dictionary<ActionID, ActionDefinition> SupportedActions;

        static Definitions()
        {
            SupportedActions = CommonDefinitions.CommonActionData(CommonDefinitions.IDPotionDex);
            SupportedActions.GCD(AID.SpinningEdge, 3);
            SupportedActions.OGCD(AID.ShadeShift, 0, CDGroup.ShadeShift, 120.0f).EffectDuration =
                20;
            SupportedActions.GCD(AID.GustSlash, 3);
            SupportedActions.OGCD(AID.SecondWind, 0, CDGroup.SecondWind, 120.0f);
            SupportedActions.OGCD(AID.Hide, 0, CDGroup.Hide, 20.0f);
            SupportedActions.OGCD(AID.LegSweep, 3, CDGroup.LegSweep, 40.0f);
            SupportedActions.OGCD(AID.Bloodbath, 0, CDGroup.Bloodbath, 90.0f).EffectDuration = 20;
            SupportedActions.GCD(AID.ThrowingDagger, 20);
            SupportedActions.OGCD(AID.Mug, 3, CDGroup.Mug, 120.0f).EffectDuration = 20;
            SupportedActions.OGCD(AID.TrickAttack, 3, CDGroup.TrickAttack, 60.0f).EffectDuration =
                15;
            SupportedActions.OGCD(AID.Feint, 10, CDGroup.Feint, 90.0f).EffectDuration = 10;
            SupportedActions.GCD(AID.AeolianEdge, 3);
            SupportedActions.GCD(AID.Ninjutsu, 0);
            SupportedActions.GCD(AID.RabbitMedium, 0);
            SupportedActions.OGCD(AID.ArmsLength, 0, CDGroup.ArmsLength, 120.0f).EffectDuration = 6;
            SupportedActions.GCD(AID.DeathBlossom, 0);
            SupportedActions.OGCD(AID.Assassinate, 3, CDGroup.Assassinate, 60.0f);
            SupportedActions.OGCDWithCharges(AID.Shukuchi, 20, CDGroup.Shukuchi, 60.0f, 2, 0.800f);

            // mudra starters
            SupportedActions.OGCDWithCharges(AID.Ten, 0, CDGroup.Ten, 20.0f, 2, 0.350f);
            SupportedActions.OGCDWithCharges(AID.Chi, 0, CDGroup.Ten, 20.0f, 2, 0.350f);
            SupportedActions.OGCDWithCharges(AID.Jin, 0, CDGroup.Ten, 20.0f, 2, 0.350f);
            // mudra combos
            SupportedActions.GCD(AID.Ten2, 0, 0.350f);
            SupportedActions.GCD(AID.Chi2, 0, 0.350f);
            SupportedActions.GCD(AID.Jin2, 0, 0.350f);

            // standard ninjutsu
            SupportedActions.GCD(AID.FumaShuriken, 25);
            SupportedActions.GCD(AID.Raiton, 20);
            SupportedActions.GCD(AID.Katon, 20);
            SupportedActions.GCD(AID.Hyoton, 25);
            SupportedActions.GCD(AID.Doton, 0);
            SupportedActions.GCD(AID.Huton, 0);
            SupportedActions.GCD(AID.Suiton, 20);
            SupportedActions.GCD(AID.HyoshoRanryu, 25);
            SupportedActions.GCD(AID.GokaMekkyaku, 20);

            // tcj ninjutsu
            SupportedActions.GCD(AID.FumaTen, 25);
            SupportedActions.GCD(AID.FumaChi, 25);
            SupportedActions.GCD(AID.FumaJin, 25);
            SupportedActions.GCD(AID.TCJRaiton, 20);
            SupportedActions.GCD(AID.TCJKaton, 20);
            SupportedActions.GCD(AID.TCJHyoton, 25);
            SupportedActions.GCD(AID.TCJDoton, 0);
            SupportedActions.GCD(AID.TCJHuton, 0);
            SupportedActions.GCD(AID.TCJSuiton, 20);

            SupportedActions
                .OGCDWithCharges(AID.TrueNorth, 0, CDGroup.TrueNorth, 45.0f, 2)
                .EffectDuration = 10;
            SupportedActions.OGCD(AID.Kassatsu, 0, CDGroup.Kassatsu, 60.0f).EffectDuration = 15;
            SupportedActions.GCD(AID.HakkeMujinsatsu, 0);
            SupportedActions.GCD(AID.ArmorCrush, 3);
            SupportedActions.OGCD(AID.DreamWithinADream, 3, CDGroup.DreamWithinADream, 60.0f);
            SupportedActions.GCD(AID.Huraijin, 3);
            SupportedActions.OGCD(AID.HellfrogMedium, 25, CDGroup.HellfrogMedium, 1.0f);
            SupportedActions.OGCD(AID.Bhavacakra, 3, CDGroup.HellfrogMedium, 1.0f);
            SupportedActions.OGCD(AID.TenChiJin, 0, CDGroup.TenChiJin, 120.0f).EffectDuration = 6;
            SupportedActions.OGCD(AID.Meisui, 0, CDGroup.Meisui, 120.0f).EffectDuration = 30;
            SupportedActions.OGCD(AID.Bunshin, 0, CDGroup.Bunshin, 90.0f).EffectDuration = 30;
            SupportedActions.GCD(AID.PhantomKamaitachi, 20);
            SupportedActions.GCD(AID.ForkedRaiju, 20);
            SupportedActions.GCD(AID.FleetingRaiju, 3);
        }
    }
}
