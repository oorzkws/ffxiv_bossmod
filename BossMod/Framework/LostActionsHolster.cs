using System;
using FFXIVClientStructs.FFXIV.Client.Game.InstanceContent;
using ImGuiNET;

namespace BossMod
{
    public sealed unsafe class LostActionsHolster
    {
        const int HolsterSize = 93;

        private readonly byte* _hol = null;

        public LostActionsHolster()
        {
            var dir = FFXIVClientStructs.FFXIV.Client.Game.Event.EventFramework.Instance()->GetPublicContentDirector();
            if (dir != null && dir->Type is PublicContentDirectorType.Bozja or PublicContentDirectorType.Delubrum)
                _hol = (byte*)((nint)dir + 11308);
        }

        public int IndexOf(uint actionID)
        {
            if (!IsActive)
                return -1;

            for (var i = 0; i < HolsterSize; i++)
                if (DutyActions.GetRealIdFromBozjaId(_hol[i]) == actionID)
                    return i;

            return -1;
        }

        public bool IsActive => _hol != null;

        public uint GetSlot(uint i)
        {
            if (i >= HolsterSize || !IsActive)
                return 0;

            return DutyActions.GetRealIdFromBozjaId(_hol[i]);
        }
    }
}
