using System;
using FFXIVClientStructs.FFXIV.Client.Game.InstanceContent;
using ImGuiNET;

namespace BossMod
{
    public sealed unsafe class LostActionsHolster : IDisposable
    {
        const int HOLSTER_SIZE = 93;

        private readonly byte* _hol = null;
        private UISimpleWindow? _debugWindow;

        public LostActionsHolster()
        {
            var dir = FFXIVClientStructs.FFXIV.Client.Game.Event.EventFramework.Instance()->GetPublicContentDirector();
            if (dir != null && dir->Type is PublicContentDirectorType.Bozja)
            {
                _hol = (byte*)((nint)dir + 11308);
            }
        }

        public void ToggleDebugWindow()
        {
            _debugWindow ??= new("Lost Actions holster", DrawHolster, false, new(200, 200));
            _debugWindow.IsOpen = !_debugWindow.IsOpen;
        }

        public void Dispose()
        {
            _debugWindow?.Dispose();
        }

        public int IndexOf(uint actionID)
        {
            if (!IsActive)
                return -1;

            for (var i = 0; i < HOLSTER_SIZE; i++)
                if (DutyActions.GetRealIdFromBozjaId(_hol[i]) == actionID)
                    return i;

            return -1;
        }

        public bool IsActive => _hol != null;

        public uint GetSlot(uint i)
        {
            if (i >= HOLSTER_SIZE || !IsActive)
                return 0;

            return DutyActions.GetRealIdFromBozjaId(_hol[i]);
        }

        private void DrawHolster()
        {
            if (!IsActive)
            {
                ImGui.TextUnformatted("inactive, not in bozja");
                return;
            }

            for (var i = 0u; i < 93; i++)
            {
                var act = GetSlot(i);
                if (act > 0)
                {
                    ImGui.TextUnformatted($"{i}: {act}");
                    ImGui.SameLine();
                    if (ImGui.Button($"put in slot 1##{i}"))
                        Service.Log($"ExecuteCommand(2950, {i}, 0, playerID, 0)");
                    ImGui.SameLine();
                    if (ImGui.Button($"put in slot 2##{i}"))
                        Service.Log($"ExecuteCommand(2950, {i}, 1, playerID, 0)");
                }
            }
        }
    }
}
