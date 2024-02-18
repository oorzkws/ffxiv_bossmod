using System.Collections.Generic;
using Lumina.Excel.GeneratedSheets;

namespace BossMod
{
    // TODO: add eureka actions...i guess...if anybody cares about that
    public static class DutyActions
    {
        // lost actions have their own bozja-specific action ID that are shown by the SimpleTweaks action ID tweak,
        // from 1-99; the list below is in that order.
        // the Reminiscence status effect reflects what lost actions a player has equipped; the value of the status param
        // is (duty action 1 ID) + (duty action 2 ID << 8)
        private static uint[] ALL =
        [
            20701, // Lost Paralyze III
            20702, // Lost Banish III
            20703, // Lost Manawall
            20704, // Lost Dispel
            20705, // Lost Stealth
            20706, // Lost Spellforge
            20707, // Lost Steelsting
            20708, // Lost Swift
            20709, // Lost Protect
            20710, // Lost Shell
            20711, // Lost Reflect
            20712, // Lost Stoneskin
            20713, // Lost Bravery
            20714, // Lost Focus
            20715, // Lost Font of Magic
            20716, // Lost Font of Skill
            20717, // Lost Font of Power
            20718, // Lost Slash
            20719, // Lost Death
            20720, // Banner of Noble Ends
            20721, // Banner of Honored Sacrifice
            20722, // Banner of Tireless Conviction
            20723, // Banner of Firm Resolve
            20724, // Banner of Solemn Clarity
            20725, // Banner of Honed Acuity
            20726, // Lost Cure
            20727, // Lost Cure II
            20728, // Lost Cure III
            20729, // Lost Cure IV
            20730, // Lost Arise
            20731, // Lost Incense
            20732, // Lost Fair Trade
            20733, // Mimic
            20734, // Dynamis Dice
            20735, // Resistance Phoenix
            20736, // Resistance Reraiser
            20737, // Resistance Potion Kit
            20738, // Resistance Ether Kit
            20739, // Resistance Medikit
            20740, // Resistance Potion
            20741, // Essence of the Aetherweaver
            20742, // Essence of the Martialist
            20743, // Essence of the Savior
            20744, // Essence of the Veteran
            20745, // Essence of the Platebearer
            20746, // Essence of the Guardian
            20747, // Essence of the Ordained
            20748, // Essence of the Skirmisher
            20749, // Essence of the Watcher
            20750, // Essence of the Profane
            20751, // Essence of the Irregular
            20752, // Essence of the Breathtaker
            20753, // Essence of the Bloodsucker
            20754, // Essence of the Beast
            20755, // Essence of the Templar
            20756, // Deep Essence of the Aetherweaver
            20757, // Deep Essence of the Martialist
            20758, // Deep Essence of the Savior
            20759, // Deep Essence of the Veteran
            20760, // Deep Essence of the Platebearer
            20761, // Deep Essence of the Guardian
            20762, // Deep Essence of the Ordained
            20763, // Deep Essence of the Skirmisher
            20764, // Deep Essence of the Watcher
            20765, // Deep Essence of the Profane
            20766, // Deep Essence of the Irregular
            20767, // Deep Essence of the Breathtaker
            20768, // Deep Essence of the Bloodsucker
            20769, // Deep Essence of the Beast
            20770, // Deep Essence of the Templar
            22344, // Lost Perception
            22345, // Lost Sacrifice
            22346, // Pure Essence of the Gambler
            22347, // Pure Essence of the Elder
            22348, // Pure Essence of the Duelist
            22349, // Pure Essence of the Fiendhunter
            22350, // Pure Essence of the Indomitable
            22351, // Pure Essence of the Divine
            22352, // Lost Flare Star
            22353, // Lost Rend Armor
            22354, // Lost Seraph Strike
            22355, // Lost Aethershield
            22356, // Lost Dervish
            23907, // Lodestone
            23908, // Lost Stoneskin II
            23909, // Lost Burst
            23910, // Lost Rampage
            23911, // Light Curtain
            23912, // Lost Reraise
            23913, // Lost Chainspell
            23914, // Lost Assassination
            23915, // Lost Protect II
            23916, // Lost Shell II
            23917, // Lost Bubble
            23918, // Lost Impetus
            23919, // Lost Excellence
            23920, // Lost Full Cure
            23921, // Lost Blood Rage
            23922, // Resistance Elixir
        ];

        public static void Register(ref Dictionary<ActionID, ActionDefinition> res)
        {
            var actions = Service.LuminaGameData!.GetExcelSheet<Action>()!;
            foreach (var actionID in ALL)
            {
                var actSheet = actions.GetRow(actionID)!;
                // for future reference, animlock for swapping lost actions is 2.1s
                // this is not exactly accurate for Rend and Seraph Strike because they're dash actions and the
                // animation lock is increased with distance from target, just like Onslaught
                (var actType, var animLock) =
                    actSheet.ActionCategory.Row == 5 ? (ActionType.Item, 1.100f) : (ActionType.Spell, 0.600f);
                var actId = new ActionID(actType, actionID);
                res[actId] = new(
                    actSheet.Range,
                    actSheet.Cast100ms * 0.1f,
                    actSheet.CooldownGroup - 1,
                    actSheet.Recast100ms * 0.1f,
                    System.Math.Max((int)actSheet.MaxCharges, 1),
                    animLock
                );
            }
        }
    }
}
