using System.Collections.Generic;

namespace BossMod.BLU
{
    public enum AID : uint
    {
        None = 0,

        // GCDs
        Whistle = 18309, // L1, 1.0s cast, range 0, single-target 0/0, targets=self, animLock=0.100s
        DivineCataract = 23274, // L1, instant, range 0, AOE circle 10/0, targets=self, animLock=???
        FeculentFlood = 23271, // L1, 2.0s cast, range 20, AOE rect 20/4, targets=hostile, animLock=0.100s
        SaintlyBeam = 23270, // L1, 2.0s cast, range 25, AOE circle 6/0, targets=hostile, animLock=0.100s
        Stotram = 23269, // L1, 2.0s cast, range 0, AOE circle 15/0, targets=self, animLock=0.100s
        WhiteDeath = 23268, // L1, instant, range 25, single-target 0/0, targets=hostile, animLock=???
        TatamiGaeshi = 23266, // L1, 2.0s cast, range 20, AOE rect 20/5, targets=hostile, animLock=0.100s
        Tingle = 23265, // L1, 2.0s cast, range 20, AOE circle 6/0, targets=hostile, animLock=0.100s
        AethericMimicryReleaseHealer = 19240, // L1, instant, range 0, single-target 0/0, targets=self, animLock=???
        AethericMimicryReleaseDamage = 19239, // L1, instant, range 0, single-target 0/0, targets=self, animLock=???
        BasicInstinct = 23276, // L1, 2.0s cast, range 0, single-target 0/0, targets=self, animLock=???
        AethericMimicryReleaseTank = 19238, // L1, instant, range 0, single-target 0/0, targets=self, animLock=???
        AethericMimicry = 18322, // L1, 1.0s cast, range 25, single-target 0/0, targets=party/friendly, animLock=0.100s
        CondensedLibra = 18321, // L1, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        Reflux = 18319, // L1, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        Exuviation = 18318, // L1, 2.0s cast, range 0, AOE circle 6/0, targets=self, animLock=0.100s
        RevengeBlast = 18316, // L1, 2.0s cast, range 3, single-target 0/0, targets=hostile, animLock=0.100s
        Cactguard = 18315, // L1, 1.0s cast, range 25, single-target 0/0, targets=party, animLock=0.100s
        PerpetualRay = 18314, // L1, 3.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        Launcher = 18313, // L1, 2.0s cast, range 0, AOE circle 15/0, targets=self, animLock=0.100s
        BlackKnightsTour = 18311, // L1, 2.0s cast, range 20, AOE rect 20/4, targets=hostile, animLock=0.100s
        WhiteKnightsTour = 18310, // L1, 2.0s cast, range 20, AOE rect 20/4, targets=hostile, animLock=0.100s
        MustardBomb = 23279, // L1, 2.0s cast, range 25, AOE circle 6/0, targets=hostile, animLock=0.100s
        MortalFlame = 34579, // L1, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        LaserEye = 34577, // L1, 2.0s cast, range 25, AOE circle 8/0, targets=hostile, animLock=0.100s
        ConvictionMarcato = 34574, // L1, 2.0s cast, range 25, AOE rect 25/5, targets=hostile, animLock=0.100s
        DimensionalShift = 34573, // L1, 5.0s cast, range 0, AOE circle 10/0, targets=self, animLock=0.100s
        DivinationRune = 34572, // L1, 2.0s cast, range 0, AOE cone 15/0, targets=self, animLock=0.100s
        DeepClean = 34570, // L1, 2.0s cast, range 25, AOE circle 6/0, targets=hostile, animLock=0.100s
        PeatPelt = 34569, // L1, 2.0s cast, range 25, AOE circle 6/0, targets=hostile, animLock=0.100s
        WildRage = 34568, // L1, 5.0s cast, range 0, AOE circle 10/0, targets=self, animLock=0.100s
        BreathOfMagic = 34567, // L1, 2.0s cast, range 0, AOE cone 10/0, targets=self, animLock=0.100s
        Blaze = 23278, // L1, 2.0s cast, range 25, AOE circle 6/0, targets=hostile, animLock=0.100s
        Rehydration = 34566, // L1, 5.0s cast, range 0, single-target 0/0, targets=self, animLock=0.100s
        RightRound = 34564, // L1, 2.0s cast, range 0, AOE circle 8/0, targets=self, animLock=0.100s
        GoblinPunch = 34563, // L1, instant, range 3, single-target 0/0, targets=hostile, animLock=0.600s
        StotramHeal = 23416, // L1, 2.0s cast, range 0, AOE circle 15/0, targets=self, animLock=0.100s
        PhantomFlurryEnd = 23289, // L1, instant, range 0, AOE cone 16/0, targets=self, animLock=???
        PeripheralSynthesis = 23286, // L1, 2.0s cast, range 20, AOE rect 20/4, targets=hostile, animLock=0.100s
        ChocoMeteor = 23284, // L1, 2.0s cast, range 25, AOE circle 8/0, targets=hostile, animLock=0.100s
        MaledictionOfWater = 23283, // L1, 2.0s cast, range 0, AOE rect 15/6, targets=self, animLock=0.100s
        HydroPull = 23282, // L1, 2.0s cast, range 0, AOE circle 15/0, targets=self, animLock=0.100s
        AetherialSpark = 23281, // L1, 2.0s cast, range 20, AOE rect 20/4, targets=hostile, animLock=0.100s
        Schiltron = 34565, // L1, 2.0s cast, range 0, single-target 0/0, targets=self, animLock=0.100s
        FrogLegs = 18307, // L1, 1.0s cast, range 0, AOE circle 4/0, targets=self, animLock=0.100s
        ToadOil = 11410, // L1, 2.0s cast, range 0, single-target 0/0, targets=self, animLock=0.100s
        Transfusion = 11409, // L1, 2.0s cast, range 25, single-target 0/0, targets=party, animLock=???
        SelfDestruct = 11408, // L1, 2.0s cast, range 0, AOE circle 20/0, targets=self, animLock=1.600s
        FinalSting = 11407, // L1, 2.0s cast, range 3, single-target 0/0, targets=hostile, animLock=???
        WhiteWind = 11406, // L1, 2.0s cast, range 0, AOE circle 15/0, targets=self, animLock=0.100s
        Missile = 11405, // L1, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        Glower = 11404, // L1, 2.0s cast, range 15, AOE rect 15/3, targets=hostile, animLock=0.100s
        Faze = 11403, // L1, 2.0s cast, range 0, AOE cone 4/0, targets=self, animLock=???
        FlameThrower = 11402, // L1, 2.0s cast, range 0, AOE cone 8/0, targets=self, animLock=0.100s
        Loom = 11401, // L1, 1.0s cast, range 15, Ground circle 1/0, targets=area, animLock=0.950s
        SharpenedKnife = 11400, // L1, 1.0s cast, range 3, single-target 0/0, targets=hostile, animLock=0.100s
        TheLook = 11399, // L1, 2.0s cast, range 0, AOE cone 6/0, targets=self, animLock=0.100s
        DrillCannons = 11398, // L1, 2.0s cast, range 20, AOE rect 20/3, targets=hostile, animLock=0.100s
        SonicBoom = 18308, // L1, 1.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        ThousandNeedles = 11397, // L1, 6.0s cast, range 0, AOE circle 4/0, targets=self, animLock=0.100s
        BloodDrain = 11395, // L1, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        MindBlast = 11394, // L1, 1.0s cast, range 0, AOE circle 6/0, targets=self, animLock=0.100s
        Bristle = 11393, // L1, 1.0s cast, range 0, single-target 0/0, targets=self, animLock=0.100s
        AcornBomb = 11392, // L1, 2.0s cast, range 25, AOE circle 6/0, targets=hostile, animLock=0.100s
        Plaincracker = 11391, // L1, 2.0s cast, range 0, AOE circle 6/0, targets=self, animLock=0.100s
        AquaBreath = 11390, // L1, 2.0s cast, range 0, AOE cone 8/0, targets=self, animLock=0.100s
        FlyingFrenzy = 11389, // L1, 1.0s cast, range 20, AOE circle 6/0, targets=hostile, animLock=0.900s
        BadBreath = 11388, // L1, 2.0s cast, range 0, AOE cone 8/0, targets=self, animLock=0.100s
        HighVoltage = 11387, // L1, 2.0s cast, range 0, AOE circle 12/0, targets=self, animLock=0.100s
        SongOfTorment = 11386, // L1, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        WaterCannon = 11385, // L1, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        FourTonzeWeight = 11384, // L1, 2.0s cast, range 25, AOE circle 4/0, targets=area, animLock=0.100s
        Snort = 11383, // L1, 2.0s cast, range 0, AOE cone 6/0, targets=self, animLock=0.100s
        BombToss = 11396, // L1, 2.0s cast, range 25, AOE circle 6/0, targets=area, animLock=0.100s
        StickyTongue = 11412, // L1, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        Level5Petrify = 11414, // L1, 2.0s cast, range 0, AOE cone 6/0, targets=self, animLock=0.100s
        Gobskin = 18304, // L1, 2.0s cast, range 0, AOE circle 20/0, targets=self, animLock=0.100s
        PomCure = 18303, // L1, 1.5s cast, range 30, single-target 0/0, targets=self/party/friendly, animLock=0.100s
        EerieSoundwave = 18302, // L1, 2.0s cast, range 0, AOE circle 6/0, targets=self, animLock=0.100s
        Chirp = 18301, // L1, 2.0s cast, range 0, AOE circle 3/0, targets=self, animLock=0.100s
        AbyssalTransfixion = 18300, // L1, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        Kaltstrahl = 18299, // L1, 2.0s cast, range 0, AOE cone 6/0, targets=self, animLock=0.100s
        Electrogenesis = 18298, // L1, 2.0s cast, range 25, AOE circle 6/0, targets=hostile, animLock=0.100s
        Northerlies = 18297, // L1, 2.0s cast, range 0, AOE cone 6/0, targets=self, animLock=0.100s
        ProteanWave = 18296, // L1, 2.0s cast, range 0, AOE cone 15/0, targets=self, animLock=0.100s
        AlpineDraft = 18295, // L1, 2.0s cast, range 20, AOE rect 20/4, targets=hostile, animLock=0.100s
        TailScrew = 11413, // L1, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        MoonFlute = 11415, // L1, 2.0s cast, range 0, single-target 0/0, targets=self, animLock=0.100s
        Doom = 11416, // L1, 2.0s cast, range 25, single-target 0/0, targets=hostile, animLock=0.100s
        MightyGuard = 11417, // L1, 2.0s cast, range 0, single-target 0/0, targets=self, animLock=0.100s
        IceSpikes = 11418, // L1, 2.0s cast, range 0, single-target 0/0, targets=self, animLock=0.100s
        TheDragonsVoice = 11420, // L1, 2.0s cast, range 0, Enemy AOE donut 20/0, targets=self, animLock=0.100s
        TheRamsVoice = 11419, // L1, 2.0s cast, range 0, AOE circle 6/0, targets=self, animLock=0.100s
        FlyingSardine = 11423, // L1, instant, range 25, single-target 0/0, targets=hostile, animLock=0.600s
        Diamondback = 11424, // L1, 2.0s cast, range 0, single-target 0/0, targets=self, animLock=2.100s
        FireAngon = 11425, // L1, 1.0s cast, range 25, AOE circle 4/0, targets=hostile, animLock=0.100s
        InkJet = 11422, // L1, 2.0s cast, range 0, AOE cone 6/0, targets=self, animLock=0.100s
        Sleep = 25880, // L10, 2.5s cast, range 30, AOE circle 5/0, targets=hostile, animLock=0.100s

        // oGCDs
        TheRoseOfDestruction = 23275, // L1, 2.0s cast, 30.0s CD (group 16), range 25, single-target 0/0, targets=hostile, animLock=0.100s
        ChelonianGate = 23273, // L1, 2.0s cast, 30.0s CD (group 16), range 0, single-target 0/0, targets=self, animLock=???
        AngelsSnack = 23272, // L1, 2.0s cast, 120.0s CD (group 15), range 0, AOE circle 20/0, targets=self, animLock=0.100s
        ColdFog = 23267, // L1, 2.0s cast, 90.0s CD (group 13), range 0, single-target 0/0, targets=self, animLock=???
        TripleTrident = 23264, // L1, 2.0s cast, 90.0s CD (group 4), range 3, single-target 0/0, targets=hostile, animLock=0.100s
        Quasar = 18324, // L1, instant, 60.0s CD (group 6), range 0, AOE circle 15/0, targets=self, animLock=0.600s
        Surpanakha = 18323, // L1, instant, 30.0s CD (group 5) (4 charges), range 0, AOE cone 16/0, targets=self, animLock=0.600s
        Devour = 18320, // L1, 1.0s cast, 60.0s CD (group 11), range 3, single-target 0/0, targets=hostile, animLock=0.100s
        AngelWhisper = 18317, // L1, 10.0s cast, 300.0s CD (group 12), range 25, single-target 0/0, targets=party/friendly, animLock=???
        Level5Death = 18312, // L1, 2.0s cast, 180.0s CD (group 10), range 0, AOE circle 6/0, targets=self, animLock=0.100s
        JKick = 18325, // L1, instant, 60.0s CD (group 6), range 25, AOE circle 6/0, targets=hostile, animLock=0.900s
        Ultravibration = 23277, // L1, 2.0s cast, 120.0s CD (group 10), range 0, AOE circle 6/0, targets=self, animLock=0.100s
        SeaShanty = 34580, // L1, instant, 120.0s CD (group 19), range 0, AOE circle 10/0, targets=self, animLock=0.600s
        CandyCane = 34578, // L1, 1.0s cast, 90.0s CD (group 7), range 25, AOE circle 5/0, targets=hostile, animLock=0.100s
        WingedReprobation = 34576, // L1, 1.0s cast, 90.0s CD (group 21), range 25, AOE rect 25/5, targets=hostile, animLock=0.100s
        ForceField = 34575, // L1, 2.0s cast, 120.0s CD (group 22), range 0, single-target 0/0, targets=self, animLock=0.100s
        RubyDynamics = 34571, // L1, 2.0s cast, 30.0s CD (group 16), range 0, AOE cone 12/0, targets=self, animLock=0.100s
        Nightbloom = 23290, // L1, instant, 120.0s CD (group 18), range 0, AOE circle 10/0, targets=self, animLock=0.600s
        PhantomFlurry = 23288, // L1, instant, 120.0s CD (group 17), range 0, AOE cone 8/0, targets=self, animLock=0.600s
        BothEnds = 23287, // L1, instant, 120.0s CD (group 18), range 0, AOE circle 20/0, targets=self, animLock=0.600s
        MatraMagic = 23285, // L1, 2.0s cast, 120.0s CD (group 15), range 25, single-target 0/0, targets=hostile, animLock=0.100s
        DragonForce = 23280, // L1, 2.0s cast, 120.0s CD (group 15), range 0, single-target 0/0, targets=self, animLock=0.100s
        Apokalypsis = 34581, // L1, instant, 120.0s CD (group 20), range 0, single-target 25/0, targets=self, animLock=0.600s
        BeingMortal = 34582, // L1, instant, 120.0s CD (group 20), range 0, AOE circle 10/0, targets=self, animLock=0.600s
        OffGuard = 11411, // L1, 1.0s cast, 60.0s CD (group 3), range 25, single-target 0/0, targets=hostile, animLock=0.100s
        Avail = 18306, // L1, 1.0s cast, 120.0s CD (group 8), range 10, single-target 0/0, targets=party, animLock=0.100s
        MagicHammer = 18305, // L1, 1.0s cast, 90.0s CD (group 7), range 25, AOE circle 8/0, targets=hostile, animLock=0.100s
        VeilOfTheWhorl = 11431, // L1, instant, 90.0s CD (group 2), range 0, single-target 0/0, targets=self, animLock=0.600s
        ShockStrike = 11429, // L1, instant, 60.0s CD (group 1), range 25, AOE circle 3/0, targets=hostile, animLock=0.600s
        GlassDance = 11430, // L1, instant, 90.0s CD (group 2), range 0, AOE cone 12/0, targets=self, animLock=0.600s
        MountainBuster = 11428, // L1, instant, 60.0s CD (group 1), range 0, AOE cone 6/0, targets=self, animLock=0.600s
        PeculiarLight = 11421, // L1, 1.0s cast, 60.0s CD (group 3), range 0, AOE circle 6/0, targets=self, animLock=0.100s
        FeatherRain = 11426, // L1, instant, 30.0s CD (group 0), range 30, AOE circle 5/0, targets=area, animLock=0.600s
        Eruption = 11427, // L1, instant, 30.0s CD (group 0), range 25, AOE circle 5/0, targets=area, animLock=0.600s
        Addle = 7560, // L8, instant, 90.0s CD (group 46), range 25, single-target 0/0, targets=hostile, animLock=0.600s
        LucidDreaming = 7562, // L14, instant, 60.0s CD (group 45), range 0, single-target 0/0, targets=self, animLock=0.600s
        Swiftcast = 7561, // L18, instant, 60.0s CD (group 44), range 0, single-target 0/0, targets=self, animLock=0.600s
        Surecast = 7559, // L44, instant, 120.0s CD (group 48), range 0, single-target 0/0, targets=self, animLock=0.600s
    }

    public enum TraitID : uint
    {
        None = 0,
        Learning = 219, // L1
        MaimAndMend = 220, // L10
        MaimAndMendII = 221, // L20
        MaimAndMendIII = 222, // L30
        MaimAndMendIV = 223, // L40
        MaimAndMendV = 224, // L50
    }

    public enum CDGroup : int
    {
        FeatherRain = 0, // 30.0 max, shared by Feather Rain, Eruption
        ShockStrike = 1, // 60.0 max, shared by Shock Strike, Mountain Buster
        VeilOfTheWhorl = 2, // 90.0 max, shared by Veil of the Whorl, Glass Dance
        OffGuard = 3, // 60.0 max, shared by Off-guard, Peculiar Light
        TripleTrident = 4, // 90.0 max
        Surpanakha = 5, // 4*30.0 max
        Quasar = 6, // 60.0 max, shared by Quasar, J Kick
        CandyCane = 7, // 90.0 max, shared by Candy Cane, Magic Hammer
        Avail = 8, // 120.0 max
        Level5Death = 10, // variable max, shared by Level 5 Death, Ultravibration
        Devour = 11, // 60.0 max
        AngelWhisper = 12, // 300.0 max
        ColdFog = 13, // 90.0 max
        AngelsSnack = 15, // 120.0 max, shared by Angel's Snack, Matra Magic, Dragon Force
        TheRoseOfDestruction = 16, // 30.0 max, shared by The Rose of Destruction, Chelonian Gate, Ruby Dynamics
        PhantomFlurry = 17, // 120.0 max
        Nightbloom = 18, // 120.0 max, shared by Nightbloom, Both Ends
        SeaShanty = 19, // 120.0 max
        Apokalypsis = 20, // 120.0 max, shared by Apokalypsis, Being Mortal
        WingedReprobation = 21, // 90.0 max
        ForceField = 22, // 120.0 max
        Swiftcast = 44, // 60.0 max
        LucidDreaming = 45, // 60.0 max
        Addle = 46, // 90.0 max
        Surecast = 48, // 120.0 max
    }

    public enum SID : uint
    {
        None = 0,
        AethericMimicryHealer = 2126, // applied by Aetheric Mimicry to self
        AethericMimicryDPS = 2125, // applied by Aetheric Mimicry to self
        AethericMimicryTank = 2124, // applied by Aetheric Mimicry to self
        PhantomFlurry = 2502, // applied by Phantom Flurry to self
        LucidDreaming = 1204, // applied by Lucid Dreaming to self
        Swiftcast = 167, // applied by Swiftcast to self
        Surecast = 160, // applied by Surecast to self
        Addle = 1203, // applied by Addle to target
        Sleep = 3, // applied by Sleep, Acorn Bomb, Chirp to target
        Conked = 2115, // applied by Magic Hammer to target
        Gobskin = 2114, // applied by Gobskin to self
        Diamondback = 1722, // applied by Diamondback to self
        Bleeding = 1714, // applied by Song of Torment, Aetherial Spark to target
        OffGuard = 1717, // applied by Off-guard to target
        WaxingNocturne = 1718, // applied by Moon Flute to self
        SurpanakhasFury = 2130, // applied by Surpanakha to self
        Harmonized = 2118, // applied by Whistle to self
        Tingling = 2492, // applied by Tingle to self
        WingedReprobation = 3640, // applied by Winged Reprobation to self
        Boost = 1716, // applied by Bristle to self
        DeepFreeze = 1731, // applied by the Ram's Voice to target
        IceSpikes = 1720, // applied by Ice Spikes to self
        Petrification = 1, // applied by Level 5 Petrify to target
        Paralysis = 17, // applied by Glower, High Voltage, the Dragon's Voice, Mind Blast, Abyssal Transfixion to target
        Dropsy = 1736, // applied by Aqua Breath to target
        Stun = 2, // applied by Bomb Toss, Tatami-gaeshi to target
        BrushWithDeath = 2127, // applied by Self-destruct to self
        Heavy = 14, // applied by 4-tonze Weight to target
        Poison = 18, // applied by Bad Breath to target
        MightyGuard = 1719, // applied by Mighty Guard to self
        ToadOil = 1737, // applied by Toad Oil to self
        Blind = 15, // applied by Ink Jet to target
        PeculiarLight = 1721, // applied by Peculiar Light to target
        VeilOfTheWhorl = 1724, // applied by Veil of the Whorl to self
        MeatShield = 2117, // applied by Avail to target
        MeatilyShielded = 2116, // applied by Avail to self
        Slow = 9, // applied by White Knight's Tour to target
        Bind = 13, // applied by Black Knight's Tour to target
        PerpetualStun = 142, // applied by Perpetual Ray to target
        Cactguard = 2119, // applied by Cactguard to target
        RefluxHeavy = 2158, // applied by Reflux to target
        HPBoost = 2120, // applied by Devour to self
        UmbralAttenuation = 2122, // applied by Condensed Libra to target
        AstralAttenuation = 2121, // applied by Condensed Libra to target
        PhysicalAttenuation = 2123, // applied by Condensed Libra to target
        DragonForce = 2500, // applied by Dragon Force to self
        Lightheaded = 2501, // applied by Peripheral Synthesis to target
        IncendiaryBurns = 2499, // applied by Mustard Bomb to target
        BreathOfMagic = 3712, // applied by Breath of Magic to target
        Schiltron = 3631, // applied by Schiltron to self
        Begrimed = 3636, // applied by Peat Pelt to target
        SpickAndSpan = 3637, // applied by Deep Clean to self
        PhysicalVulnerabilityDown = 3638, // applied by Force Field to self
        MagicVulnerabilityDown = 3639, // applied by Force Field to self
        CandyCane = 3642, // applied by Candy Cane to target
        MortalFlame = 3643, // applied by Mortal Flame to target
        Apokalypsis = 3644, // applied by Apokalypsis to self
    }

    public static class Definitions
    {
        public static Dictionary<ActionID, ActionDefinition> SupportedActions;
        public static uint[] UnlockQuests = { };

        public static bool Unlocked(AID aid, int level, int questProgress)
        {
            return aid switch
            {
                AID.Addle => level >= 8,
                AID.Sleep => level >= 10,
                AID.LucidDreaming => level >= 14,
                AID.Swiftcast => level >= 18,
                AID.Surecast => level >= 44,
                _ => true,
            };
        }

        public static bool Unlocked(TraitID tid, int level, int questProgress)
        {
            return tid switch
            {
                TraitID.MaimAndMend => level >= 10,
                TraitID.MaimAndMendII => level >= 20,
                TraitID.MaimAndMendIII => level >= 30,
                TraitID.MaimAndMendIV => level >= 40,
                TraitID.MaimAndMendV => level >= 50,
                _ => true,
            };
        }

        static Definitions()
        {
            SupportedActions = CommonDefinitions.CommonActionData(CommonDefinitions.IDPotionInt);
            SupportedActions.GCDCast(AID.Whistle, 0, 1.0f);
            SupportedActions.OGCDCast(AID.TheRoseOfDestruction, 25, CDGroup.TheRoseOfDestruction, 30.0f, 2.0f);
            SupportedActions.GCD(AID.DivineCataract, 0);
            SupportedActions.OGCDCast(AID.ChelonianGate, 0, CDGroup.TheRoseOfDestruction, 30.0f, 2.0f);
            SupportedActions.OGCDCast(AID.AngelsSnack, 0, CDGroup.AngelsSnack, 120.0f, 2.0f);
            SupportedActions.GCDCast(AID.FeculentFlood, 20, 2.0f);
            SupportedActions.GCDCast(AID.SaintlyBeam, 25, 2.0f);
            SupportedActions.GCDCast(AID.Stotram, 0, 2.0f);
            SupportedActions.GCD(AID.WhiteDeath, 25);
            SupportedActions.OGCDCast(AID.ColdFog, 0, CDGroup.ColdFog, 90.0f, 2.0f);
            SupportedActions.GCDCast(AID.TatamiGaeshi, 20, 2.0f);
            SupportedActions.GCDCast(AID.Tingle, 20, 2.0f);
            SupportedActions.OGCDCast(AID.TripleTrident, 3, CDGroup.TripleTrident, 90.0f, 2.0f);
            SupportedActions.GCD(AID.AethericMimicryReleaseHealer, 0);
            SupportedActions.GCD(AID.AethericMimicryReleaseDamage, 0);
            SupportedActions.GCDCast(AID.BasicInstinct, 0, 2.0f);
            SupportedActions.GCD(AID.AethericMimicryReleaseTank, 0);
            SupportedActions.OGCD(AID.Quasar, 0, CDGroup.Quasar, 60.0f);
            SupportedActions.OGCDWithCharges(AID.Surpanakha, 0, CDGroup.Surpanakha, 30.0f, 4);
            SupportedActions.GCDCast(AID.AethericMimicry, 25, 1.0f);
            SupportedActions.GCDCast(AID.CondensedLibra, 25, 2.0f);
            SupportedActions.OGCDCast(AID.Devour, 3, CDGroup.Devour, 60.0f, 1.0f);
            SupportedActions.GCDCast(AID.Reflux, 25, 2.0f);
            SupportedActions.GCDCast(AID.Exuviation, 0, 2.0f);
            SupportedActions.OGCDCast(AID.AngelWhisper, 25, CDGroup.AngelWhisper, 300.0f, 10.0f);
            SupportedActions.GCDCast(AID.RevengeBlast, 3, 2.0f);
            SupportedActions.GCDCast(AID.Cactguard, 25, 1.0f);
            SupportedActions.GCDCast(AID.PerpetualRay, 25, 3.0f);
            SupportedActions.GCDCast(AID.Launcher, 0, 2.0f);
            SupportedActions.OGCDCast(AID.Level5Death, 0, CDGroup.Level5Death, 180.0f, 2.0f);
            SupportedActions.GCDCast(AID.BlackKnightsTour, 20, 2.0f);
            SupportedActions.OGCD(AID.JKick, 25, CDGroup.Quasar, 60.0f);
            SupportedActions.GCDCast(AID.WhiteKnightsTour, 20, 2.0f);
            SupportedActions.OGCDCast(AID.Ultravibration, 0, CDGroup.Level5Death, 120.0f, 2.0f);
            SupportedActions.GCDCast(AID.MustardBomb, 25, 2.0f);
            SupportedActions.OGCD(AID.SeaShanty, 0, CDGroup.SeaShanty, 120.0f);
            SupportedActions.GCDCast(AID.MortalFlame, 25, 2.0f);
            SupportedActions.OGCDCast(AID.CandyCane, 25, CDGroup.CandyCane, 90.0f, 1.0f);
            SupportedActions.GCDCast(AID.LaserEye, 25, 2.0f);
            SupportedActions.OGCDCast(AID.WingedReprobation, 25, CDGroup.WingedReprobation, 90.0f, 1.0f);
            SupportedActions.OGCDCast(AID.ForceField, 0, CDGroup.ForceField, 120.0f, 2.0f);
            SupportedActions.GCDCast(AID.ConvictionMarcato, 25, 2.0f);
            SupportedActions.GCDCast(AID.DimensionalShift, 0, 5.0f);
            SupportedActions.GCDCast(AID.DivinationRune, 0, 2.0f);
            SupportedActions.OGCDCast(AID.RubyDynamics, 0, CDGroup.TheRoseOfDestruction, 30.0f, 2.0f);
            SupportedActions.GCDCast(AID.DeepClean, 25, 2.0f);
            SupportedActions.GCDCast(AID.PeatPelt, 25, 2.0f);
            SupportedActions.GCDCast(AID.WildRage, 0, 5.0f);
            SupportedActions.GCDCast(AID.BreathOfMagic, 0, 2.0f);
            SupportedActions.GCDCast(AID.Blaze, 25, 2.0f);
            SupportedActions.GCDCast(AID.Rehydration, 0, 5.0f);
            SupportedActions.GCDCast(AID.RightRound, 0, 2.0f);
            SupportedActions.GCD(AID.GoblinPunch, 3);
            SupportedActions.GCDCast(AID.StotramHeal, 0, 2.0f);
            SupportedActions.OGCD(AID.Nightbloom, 0, CDGroup.Nightbloom, 120.0f);
            SupportedActions.GCD(AID.PhantomFlurryEnd, 0);
            SupportedActions.OGCD(AID.PhantomFlurry, 0, CDGroup.PhantomFlurry, 120.0f);
            SupportedActions.OGCD(AID.BothEnds, 0, CDGroup.Nightbloom, 120.0f);
            SupportedActions.GCDCast(AID.PeripheralSynthesis, 20, 2.0f);
            SupportedActions.OGCDCast(AID.MatraMagic, 25, CDGroup.AngelsSnack, 120.0f, 2.0f);
            SupportedActions.GCDCast(AID.ChocoMeteor, 25, 2.0f);
            SupportedActions.GCDCast(AID.MaledictionOfWater, 0, 2.0f);
            SupportedActions.GCDCast(AID.HydroPull, 0, 2.0f);
            SupportedActions.GCDCast(AID.AetherialSpark, 20, 2.0f);
            SupportedActions.OGCDCast(AID.DragonForce, 0, CDGroup.AngelsSnack, 120.0f, 2.0f);
            SupportedActions.GCDCast(AID.Schiltron, 0, 2.0f);
            SupportedActions.OGCD(AID.Apokalypsis, 0, CDGroup.Apokalypsis, 120.0f);
            SupportedActions.OGCD(AID.BeingMortal, 0, CDGroup.Apokalypsis, 120.0f);
            SupportedActions.GCDCast(AID.FrogLegs, 0, 1.0f);
            SupportedActions.GCDCast(AID.ToadOil, 0, 2.0f);
            SupportedActions.GCDCast(AID.Transfusion, 25, 2.0f);
            SupportedActions.GCDCast(AID.SelfDestruct, 0, 2.0f);
            SupportedActions.GCDCast(AID.FinalSting, 3, 2.0f);
            SupportedActions.GCDCast(AID.WhiteWind, 0, 2.0f);
            SupportedActions.GCDCast(AID.Missile, 25, 2.0f);
            SupportedActions.GCDCast(AID.Glower, 15, 2.0f);
            SupportedActions.GCDCast(AID.Faze, 0, 2.0f);
            SupportedActions.GCDCast(AID.FlameThrower, 0, 2.0f);
            SupportedActions.GCDCast(AID.Loom, 15, 1.0f);
            SupportedActions.GCDCast(AID.SharpenedKnife, 3, 1.0f);
            SupportedActions.GCDCast(AID.TheLook, 0, 2.0f);
            SupportedActions.GCDCast(AID.DrillCannons, 20, 2.0f);
            SupportedActions.GCDCast(AID.SonicBoom, 25, 1.0f);
            SupportedActions.GCDCast(AID.ThousandNeedles, 0, 6.0f);
            SupportedActions.GCDCast(AID.BloodDrain, 25, 2.0f);
            SupportedActions.GCDCast(AID.MindBlast, 0, 1.0f);
            SupportedActions.GCDCast(AID.Bristle, 0, 1.0f);
            SupportedActions.GCDCast(AID.AcornBomb, 25, 2.0f);
            SupportedActions.GCDCast(AID.Plaincracker, 0, 2.0f);
            SupportedActions.GCDCast(AID.AquaBreath, 0, 2.0f);
            SupportedActions.GCDCast(AID.FlyingFrenzy, 20, 1.0f);
            SupportedActions.GCDCast(AID.BadBreath, 0, 2.0f);
            SupportedActions.GCDCast(AID.HighVoltage, 0, 2.0f);
            SupportedActions.GCDCast(AID.SongOfTorment, 25, 2.0f);
            SupportedActions.GCDCast(AID.WaterCannon, 25, 2.0f);
            SupportedActions.GCDCast(AID.FourTonzeWeight, 25, 2.0f);
            SupportedActions.GCDCast(AID.Snort, 0, 2.0f);
            SupportedActions.GCDCast(AID.BombToss, 25, 2.0f);
            SupportedActions.GCDCast(AID.StickyTongue, 25, 2.0f);
            SupportedActions.OGCDCast(AID.OffGuard, 25, CDGroup.OffGuard, 60.0f, 1.0f);
            SupportedActions.GCDCast(AID.Level5Petrify, 0, 2.0f);
            SupportedActions.OGCDCast(AID.Avail, 10, CDGroup.Avail, 120.0f, 1.0f);
            SupportedActions.OGCDCast(AID.MagicHammer, 25, CDGroup.CandyCane, 90.0f, 1.0f);
            SupportedActions.GCDCast(AID.Gobskin, 0, 2.0f);
            SupportedActions.GCDCast(AID.PomCure, 30, 1.5f);
            SupportedActions.GCDCast(AID.EerieSoundwave, 0, 2.0f);
            SupportedActions.GCDCast(AID.Chirp, 0, 2.0f);
            SupportedActions.GCDCast(AID.AbyssalTransfixion, 25, 2.0f);
            SupportedActions.GCDCast(AID.Kaltstrahl, 0, 2.0f);
            SupportedActions.GCDCast(AID.Electrogenesis, 25, 2.0f);
            SupportedActions.GCDCast(AID.Northerlies, 0, 2.0f);
            SupportedActions.GCDCast(AID.ProteanWave, 0, 2.0f);
            SupportedActions.GCDCast(AID.AlpineDraft, 20, 2.0f);
            SupportedActions.OGCD(AID.VeilOfTheWhorl, 0, CDGroup.VeilOfTheWhorl, 90.0f);
            SupportedActions.GCDCast(AID.TailScrew, 25, 2.0f);
            SupportedActions.OGCD(AID.ShockStrike, 25, CDGroup.ShockStrike, 60.0f);
            SupportedActions.OGCD(AID.GlassDance, 0, CDGroup.VeilOfTheWhorl, 90.0f);
            SupportedActions.OGCD(AID.MountainBuster, 0, CDGroup.ShockStrike, 60.0f);
            SupportedActions.GCDCast(AID.MoonFlute, 0, 2.0f);
            SupportedActions.GCDCast(AID.Doom, 25, 2.0f);
            SupportedActions.GCDCast(AID.MightyGuard, 0, 2.0f);
            SupportedActions.GCDCast(AID.IceSpikes, 0, 2.0f);
            SupportedActions.GCDCast(AID.TheDragonsVoice, 0, 2.0f);
            SupportedActions.OGCDCast(AID.PeculiarLight, 0, CDGroup.OffGuard, 60.0f, 1.0f);
            SupportedActions.GCDCast(AID.TheRamsVoice, 0, 2.0f);
            SupportedActions.GCD(AID.FlyingSardine, 25);
            SupportedActions.GCDCast(AID.Diamondback, 0, 2.0f);
            SupportedActions.GCDCast(AID.FireAngon, 25, 1.0f);
            SupportedActions.OGCD(AID.FeatherRain, 30, CDGroup.FeatherRain, 30.0f);
            SupportedActions.OGCD(AID.Eruption, 25, CDGroup.FeatherRain, 30.0f);
            SupportedActions.GCDCast(AID.InkJet, 0, 2.0f);
            SupportedActions.OGCD(AID.Addle, 25, CDGroup.Addle, 90.0f);
            SupportedActions.GCDCast(AID.Sleep, 30, 2.5f);
            SupportedActions.OGCD(AID.LucidDreaming, 0, CDGroup.LucidDreaming, 60.0f);
            SupportedActions.OGCD(AID.Swiftcast, 0, CDGroup.Swiftcast, 60.0f);
            SupportedActions.OGCD(AID.Surecast, 0, CDGroup.Surecast, 120.0f);
        }
    }
}
