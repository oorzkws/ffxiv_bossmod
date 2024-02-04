using System;

namespace BossMod.NIN
{
    class Combos {
        public static readonly AID[] Sequences =
            [ AID.Ten, AID.FumaShuriken, AID.FumaShuriken, AID.FumaShuriken, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None
            , AID.Chi, AID.None, AID.Ten2, AID.Ten2, AID.None, AID.None, AID.Katon, AID.Katon, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None
            , AID.Ten, AID.Chi2, AID.None, AID.Chi2, AID.None, AID.None, AID.None, AID.None, AID.None, AID.Raiton, AID.None, AID.Raiton, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None
            , AID.Ten, AID.Jin2, AID.Jin2, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.Hyoton, AID.Hyoton, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None
            , AID.Jin, AID.None, AID.Jin2, AID.Chi2, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.Ten2, AID.None, AID.None, AID.Ten2, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.Huton, AID.None, AID.None, AID.Huton, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None
            , AID.Jin, AID.Jin2, AID.None, AID.Ten2, AID.None, AID.None, AID.None, AID.Chi2, AID.None, AID.None, AID.None, AID.None, AID.None, AID.Chi2, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.Doton, AID.None, AID.None, AID.None, AID.None, AID.None, AID.Doton, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None
            , AID.Chi, AID.Chi2, AID.Ten2, AID.None, AID.None, AID.None, AID.Jin2, AID.None, AID.None, AID.Jin2, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.None, AID.Suiton, AID.None, AID.None, AID.Suiton
            ];

        public static AID GetCurrentNinjutsu(int comboState, bool isKassatsu) => comboState switch {
            0 => AID.Ninjutsu,
            1 or 2 or 3 => AID.FumaShuriken,
            6 or 7 => isKassatsu ? AID.GokaMekkyaku : AID.Katon,
            9 or 11 => AID.Raiton,
            13 or 14 => isKassatsu ? AID.HyoshoRanryu : AID.Hyoton,
            27 or 30 => AID.Huton,
            39 or 45 => AID.Doton,
            54 or 57 => AID.Suiton,
            _ => AID.RabbitMedium
        };

        public static AID GetNextAction(AID ninjutsu, int comboState)
        {
            var ix = ninjutsu switch {
                AID.FumaShuriken => 0,
                AID.Katon or AID.GokaMekkyaku => 1,
                AID.Raiton => 2,
                AID.Hyoton or AID.HyoshoRanryu => 3,
                AID.Huton => 4,
                AID.Doton => 5,
                AID.Suiton => 6,
                _ => -1
            };
            if (ix == -1) {
                Service.Log($"Action {ninjutsu} is not a ninjutsu action");
                return AID.None;
            }
            try {
                return Sequences[ix * 58 + comboState];
            } catch (IndexOutOfRangeException) {
                Service.Log($"index {ix * 58 + comboState} (ix {ix}, combo state {comboState}) is invalid");
                return AID.None;
            }
        }
    }
}
