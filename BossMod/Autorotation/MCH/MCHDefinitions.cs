using System.Collections.Generic;

namespace BossMod.MCH
{
    public enum AID : uint
    {
        None = 0,

        // GCDs
        SplitShot = 2866, // L1, instant, range 25, single-target 0/0, targets=hostile, animLock=???
        SlugShot = 2868, // L2, instant, range 25, single-target 0/0, targets=hostile, animLock=???
        SpreadShot = 2870, // L18, instant, range 12, AOE cone 12/0, targets=hostile, animLock=???
        CleanShot = 2873, // L26, instant, range 25, single-target 0/0, targets=hostile, animLock=???
        HeatBlast = 7410, // L35, instant, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        AutoCrossbow = 16497, // L52, instant, range 12, AOE cone 12/0, targets=hostile, animLock=0.600s
        HeatedSplitShot = 7411, // L54, instant, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        HeatedSlugShot = 7412, // L60, instant, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        HeatedCleanShot = 7413, // L64, instant, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        ArmPunch = 16504, // L80, instant, range 3, single-target 0/0, targets=hostile, animLock=???
        RollerDash = 17206, // L80, instant, range 30, single-target 0/0, targets=hostile, animLock=???
        Scattergun = 25786, // L82, instant, range 12, AOE cone 12/0, targets=hostile, animLock=0.600s

        // oGCDs
        HotShot = 2872, // L4, instant, 40.0s CD (group 7), range 25, single-target 0/0, targets=hostile, animLock=???
        LegGraze = 7554, // L6, instant, 30.0s CD (group 42), range 25, single-target 0/0, targets=hostile, animLock=0.600s
        SecondWind = 7541, // L8, instant, 120.0s CD (group 49), range 0, single-target 0/0, targets=self, animLock=0.600s
        Reassemble = 2876, // L10, instant, 55.0s CD (group 21) (2 charges), range 0, single-target 0/0, targets=self, animLock=0.600s
        FootGraze = 7553, // L10, instant, 30.0s CD (group 41), range 25, single-target 0/0, targets=hostile, animLock=0.600s
        GaussRound = 2874, // L15, instant, 30.0s CD (group 9) (3 charges), range 25, single-target 0/0, targets=hostile, animLock=0.600s
        Peloton = 7557, // L20, instant, 5.0s CD (group 40), range 0, AOE circle 30/0, targets=self, animLock=0.600s
        HeadGraze = 7551, // L24, instant, 30.0s CD (group 43), range 25, single-target 0/0, targets=hostile, animLock=0.600s
        Hypercharge = 17209, // L30, instant, 10.0s CD (group 4), range 0, single-target 0/0, targets=self, animLock=0.600s
        ArmsLength = 7548, // L32, instant, 120.0s CD (group 48), range 0, single-target 0/0, targets=self, animLock=0.600s
        RookOverload = 7416, // L40, instant, 0.0s CD (group 25), range 40, single-target 0/0, targets=hostile, animLock=???
        RookAutoturret = 2864, // L40, instant, 6.0s CD (group 3), range 0, single-target 0/0, targets=self, animLock=???
        RookOverdrive = 7415, // L40, instant, 15.0s CD (group 3), range 25, single-target 0/0, targets=self, animLock=???
        Wildfire = 2878, // L45, instant, 120.0s CD (group 19), range 25, single-target 0/0, targets=hostile, animLock=0.600s
        Detonator = 16766, // L45, instant, 1.0s CD (group 0), range 25, single-target 0/0, targets=self, animLock=0.600s
        Ricochet = 2890, // L50, instant, 30.0s CD (group 10) (3 charges), range 25, AOE circle 5/0, targets=hostile, animLock=0.600s
        Tactician = 16889, // L56, instant, 120.0s CD (group 23), range 0, AOE circle 30/0, targets=self, animLock=0.600s
        Drill = 16498, // L58, instant, 20.0s CD (group 6), range 25, single-target 0/0, targets=hostile, animLock=0.600s
        Dismantle = 2887, // L62, instant, 120.0s CD (group 18), range 25, single-target 0/0, targets=hostile, animLock=0.600s
        BarrelStabilizer = 7414, // L66, instant, 120.0s CD (group 20), range 0, single-target 0/0, targets=self, animLock=0.600s
        Flamethrower = 7418, // L70, instant, 60.0s CD (group 12), range 0, single-target 8/0, targets=self, animLock=0.600s
        Bioblaster = 16499, // L72, instant, 20.0s CD (group 6), range 12, AOE cone 12/0, targets=hostile, animLock=0.600s
        AirAnchor = 16500, // L76, instant, 40.0s CD (group 8), range 25, single-target 0/0, targets=hostile, animLock=0.600s
        QueenOverdrive = 16502, // L80, instant, 15.0s CD (group 1), range 30, single-target 0/0, targets=self, animLock=???
        AutomatonQueen = 16501, // L80, instant, 6.0s CD (group 1), range 0, single-target 0/0, targets=self, animLock=0.600s
        PileBunker = 16503, // L80, instant, 0.0s CD (group 25), range 3, single-target 0/0, targets=hostile, animLock=???
        CrownedCollider = 25787, // L86, instant, 0.0s CD (group 25), range 3, single-target 0/0, targets=hostile, animLock=???
        ChainSaw = 25788, // L90, instant, 60.0s CD (group 11), range 25, AOE rect 25/4, targets=hostile, animLock=0.600s
    }

    public enum CDGroup : int
    {
        Detonator = 0, // 1.0 max, shared by Detonator, Rook Overload, Pile Bunker, Crowned Collider
        QueenOverdrive = 1, // variable max, shared by Queen Overdrive, Automaton Queen
        RookAutoturret = 3, // variable max, shared by Rook Autoturret, Rook Overdrive
        Hypercharge = 4, // 10.0 max
        Drill = 6, // 20.0 max, shared by Drill, Bioblaster
        HotShot = 7, // 40.0 max
        AirAnchor = 8, // 40.0 max
        GaussRound = 9, // 3*30.0 max
        Ricochet = 10, // 3*30.0 max
        ChainSaw = 11, // 60.0 max
        Flamethrower = 12, // 60.0 max
        Dismantle = 18, // 120.0 max
        Wildfire = 19, // 120.0 max
        BarrelStabilizer = 20, // 120.0 max
        Reassemble = 21, // 2*55.0 max
        Tactician = 23, // 120.0 max
        Peloton = 40, // 5.0 max
        FootGraze = 41, // 30.0 max
        LegGraze = 42, // 30.0 max
        HeadGraze = 43, // 30.0 max
        ArmsLength = 48, // 120.0 max
        SecondWind = 49, // 120.0 max
    }

    public enum TraitID : uint
    {
        None = 0,
        IncreasedActionDamage = 117, // L20
        IncreasedActionDamageII = 119, // L40
        SplitShotMastery = 288, // L54
        SlugShotMastery = 289, // L60
        CleanShotMastery = 290, // L64
        ChargedActionMastery = 292, // L74
        HotShotMastery = 291, // L76
        EnhancedWildfire = 293, // L78
        Promotion = 294, // L80
        SpreadShotMastery = 449, // L82
        EnhancedReassemble = 450, // L84
        MarksmansMastery = 517, // L84
        QueensGambit = 451, // L86
        EnhancedTactician = 452, // L88
    }

    public enum SID : uint
    {
        None = 0,
        Bioblaster = 1866, // applied by Bioblaster to target
        Flamethrower = 1205, // applied by Flamethrower to self
        Dismantled = 860, // applied by Dismantle to target
        Tactician = 1951, // applied by Tactician to self
        Overheated = 2688, // applied by Hypercharge to self
        Wildfire = 861, // applied by Wildfire to target
        WildfireActive = 1946, // applied by Wildfire to self
        ArmsLength = 1209, // applied by Arm's Length to self
        Reassembled = 851, // applied by Reassemble to self
        Heavy = 14, // applied by Leg Graze to target
        Bind = 13, // applied by Foot Graze to target
        Peloton = 1199, // applied by Peloton to self
    }

    public static class Definitions
    {
        public static uint[] UnlockQuests = { 67233, 67234, 67235, 67240, 67242, 67243, 67244, 67246, 67248, 68445 };

        public static bool Unlocked(AID aid, int level, int questProgress)
        {
            return aid switch
            {
                AID.SlugShot => level >= 2,
                AID.HotShot => level >= 4,
                AID.LegGraze => level >= 6,
                AID.SecondWind => level >= 8,
                AID.Reassemble => level >= 10,
                AID.FootGraze => level >= 10,
                AID.GaussRound => level >= 15,
                AID.SpreadShot => level >= 18,
                AID.Peloton => level >= 20,
                AID.HeadGraze => level >= 24,
                AID.CleanShot => level >= 26,
                AID.Hypercharge => level >= 30 && questProgress > 0,
                AID.ArmsLength => level >= 32,
                AID.HeatBlast => level >= 35 && questProgress > 1,
                AID.RookOverload => level >= 40,
                AID.RookAutoturret => level >= 40 && questProgress > 2,
                AID.RookOverdrive => level >= 40,
                AID.Wildfire => level >= 45,
                AID.Detonator => level >= 45,
                AID.Ricochet => level >= 50 && questProgress > 3,
                AID.AutoCrossbow => level >= 52 && questProgress > 4,
                AID.HeatedSplitShot => level >= 54 && questProgress > 5,
                AID.Tactician => level >= 56 && questProgress > 6,
                AID.Drill => level >= 58 && questProgress > 7,
                AID.HeatedSlugShot => level >= 60 && questProgress > 8,
                AID.Dismantle => level >= 62,
                AID.HeatedCleanShot => level >= 64,
                AID.BarrelStabilizer => level >= 66,
                AID.Flamethrower => level >= 70 && questProgress > 9,
                AID.Bioblaster => level >= 72,
                AID.AirAnchor => level >= 76,
                AID.ArmPunch => level >= 80,
                AID.QueenOverdrive => level >= 80,
                AID.AutomatonQueen => level >= 80,
                AID.RollerDash => level >= 80,
                AID.PileBunker => level >= 80,
                AID.Scattergun => level >= 82,
                AID.CrownedCollider => level >= 86,
                AID.ChainSaw => level >= 90,
                _ => true,
            };
        }

        public static bool Unlocked(TraitID tid, int level, int questProgress)
        {
            return tid switch
            {
                TraitID.IncreasedActionDamage => level >= 20,
                TraitID.IncreasedActionDamageII => level >= 40,
                TraitID.SplitShotMastery => level >= 54 && questProgress > 5,
                TraitID.SlugShotMastery => level >= 60 && questProgress > 8,
                TraitID.CleanShotMastery => level >= 64,
                TraitID.ChargedActionMastery => level >= 74,
                TraitID.HotShotMastery => level >= 76,
                TraitID.EnhancedWildfire => level >= 78,
                TraitID.Promotion => level >= 80,
                TraitID.SpreadShotMastery => level >= 82,
                TraitID.EnhancedReassemble => level >= 84,
                TraitID.MarksmansMastery => level >= 84,
                TraitID.QueensGambit => level >= 86,
                TraitID.EnhancedTactician => level >= 88,
                _ => true,
            };
        }

        public static Dictionary<ActionID, ActionDefinition> SupportedActions;

        static Definitions()
        {
            SupportedActions = CommonDefinitions.CommonActionData(CommonDefinitions.IDPotionDex);
            SupportedActions.GCD(AID.SplitShot, 25);
            SupportedActions.GCD(AID.SlugShot, 25);
            SupportedActions.OGCD(AID.HotShot, 25, CDGroup.HotShot, 40.0f);
            SupportedActions.OGCD(AID.LegGraze, 25, CDGroup.LegGraze, 30.0f);
            SupportedActions.OGCD(AID.SecondWind, 0, CDGroup.SecondWind, 120.0f);
            SupportedActions.OGCDWithCharges(AID.Reassemble, 0, CDGroup.Reassemble, 55.0f, 2);
            SupportedActions.OGCD(AID.FootGraze, 25, CDGroup.FootGraze, 30.0f);
            SupportedActions.OGCDWithCharges(AID.GaussRound, 25, CDGroup.GaussRound, 30.0f, 3);
            SupportedActions.GCD(AID.SpreadShot, 12);
            SupportedActions.OGCD(AID.Peloton, 0, CDGroup.Peloton, 5.0f);
            SupportedActions.OGCD(AID.HeadGraze, 25, CDGroup.HeadGraze, 30.0f);
            SupportedActions.GCD(AID.CleanShot, 25);
            SupportedActions.OGCD(AID.Hypercharge, 0, CDGroup.Hypercharge, 10.0f);
            SupportedActions.OGCD(AID.ArmsLength, 0, CDGroup.ArmsLength, 120.0f);
            SupportedActions.GCD(AID.HeatBlast, 25);
            SupportedActions.OGCD(AID.RookAutoturret, 0, CDGroup.RookAutoturret, 6.0f);
            SupportedActions.OGCD(AID.RookOverdrive, 25, CDGroup.RookAutoturret, 15.0f);
            SupportedActions.OGCD(AID.Wildfire, 25, CDGroup.Wildfire, 120.0f);
            SupportedActions.OGCD(AID.Detonator, 25, CDGroup.Detonator, 1.0f);
            SupportedActions.OGCDWithCharges(AID.Ricochet, 25, CDGroup.Ricochet, 30.0f, 3);
            SupportedActions.GCD(AID.AutoCrossbow, 12);
            SupportedActions.GCD(AID.HeatedSplitShot, 25);
            SupportedActions.OGCD(AID.Tactician, 0, CDGroup.Tactician, 120.0f);
            SupportedActions.OGCD(AID.Drill, 25, CDGroup.Drill, 20.0f);
            SupportedActions.GCD(AID.HeatedSlugShot, 25);
            SupportedActions.OGCD(AID.Dismantle, 25, CDGroup.Dismantle, 120.0f);
            SupportedActions.GCD(AID.HeatedCleanShot, 25);
            SupportedActions.OGCD(AID.BarrelStabilizer, 0, CDGroup.BarrelStabilizer, 120.0f);
            SupportedActions.OGCD(AID.Flamethrower, 0, CDGroup.Flamethrower, 60.0f);
            SupportedActions.OGCD(AID.Bioblaster, 12, CDGroup.Drill, 20.0f);
            SupportedActions.OGCD(AID.AirAnchor, 25, CDGroup.AirAnchor, 40.0f);
            SupportedActions.OGCD(AID.QueenOverdrive, 30, CDGroup.QueenOverdrive, 15.0f);
            SupportedActions.OGCD(AID.AutomatonQueen, 0, CDGroup.QueenOverdrive, 6.0f);
            SupportedActions.GCD(AID.Scattergun, 12);
            SupportedActions.OGCD(AID.ChainSaw, 25, CDGroup.ChainSaw, 60.0f);
            // turret/queen actions
            SupportedActions.GCD(AID.ArmPunch, 3);
            SupportedActions.GCD(AID.RollerDash, 30);
            SupportedActions.OGCD(AID.RookOverload, 40, CDGroup.Detonator, 0.0f);
            SupportedActions.OGCD(AID.PileBunker, 3, CDGroup.Detonator, 0.0f);
            SupportedActions.OGCD(AID.CrownedCollider, 3, CDGroup.Detonator, 0.0f);
        }
    }
}
