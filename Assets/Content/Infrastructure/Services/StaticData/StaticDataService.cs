using System;
using System.Collections.Generic;
using System.Linq;
using Content.Data.Inventory;
using Content.Data.Stats;
using Content.Gameplay.Code.Inventory;
using Content.Gameplay.Code.Stats;
using Content.Gameplay.Code.Stats.Contracts;

namespace Content.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<Type, StatConfigData> _statDatas;
        private InventoryItem[] _inventoryItemDefinitions;

        public void Initialize()
        {
            _statDatas = new Dictionary<Type, StatConfigData>
            {
                [typeof(HealthStat)] =    new() { StatName = "Health",    DisplayMaxValue = true},
                [typeof(EnduranceStat)] = new() { StatName = "Endurance", DisplayMaxValue = false},
                [typeof(StrengthStat)] =  new() { StatName = "Strength",  DisplayMaxValue = false},
                [typeof(WisdomStat)] =    new() { StatName = "Wisdom",    DisplayMaxValue = false},
            };

            _inventoryItemDefinitions = new InventoryItem[]
            {
                new()
                {
                    Id = "Item_Healing_Potion",
                    Name = "Healing Potion",
                    MaxStackQuantity = 5,
                    StatDelta = 1,
                    StatType = StatType.Health
                },
                new()
                {
                    Id = "Item_Poison",
                    Name = "Poison",
                    MaxStackQuantity = 5,
                    StatDelta = -1,
                    StatType = StatType.Health
                },
                new()
                {
                    Id = "Item_Speed_Potion",
                    Name = "Speed Potion",
                    MaxStackQuantity = 5,
                    StatDelta = 1,
                    StatType = StatType.Endurance
                },
                new()
                {
                    Id = "Item_Strength_Hammer",
                    Name = "Hammer of Strength",
                    MaxStackQuantity = 1,
                    StatDelta = 1,
                    StatType = StatType.Strength
                },
                new()
                {
                    Id = "Item_Wisdom_Book",
                    Name = "Book of Wisdom",
                    MaxStackQuantity = 1,
                    StatDelta = 1,
                    StatType = StatType.Wisdom
                }
            };

            /*_defaultCharacterStatData = new CharacterStatData
            {
                Health =    new StatEntry { CurrentValue = 3, MaxValue = 3 },
                Endurance = new StatEntry { CurrentValue = 0, MaxValue = 0 },
                Strength =  new StatEntry { CurrentValue = 0, MaxValue = 0 },
                Wisdom =    new StatEntry { CurrentValue = 0, MaxValue = 0 }
            };*/

        }

        public CharacterStatData GetDefaultCharacterStatData()
        {
            return new CharacterStatData
            {
                Health =    new StatEntry { CurrentValue = 3, MaxValue = 3 },
                Endurance = new StatEntry { CurrentValue = 0, MaxValue = 0 },
                Strength =  new StatEntry { CurrentValue = 0, MaxValue = 0 },
                Wisdom =    new StatEntry { CurrentValue = 0, MaxValue = 0 }
            };
        }

        public InventoryData GetDefaultInventoryData()
        {
            InventoryData res = new InventoryData { ItemSlots = new ItemSlotData[10] };

            for (int i = 0; i < res.ItemSlots.Length; i++)
            {
                res.ItemSlots[i] = new ItemSlotData
                {
                    InventoryItem = null,
                    Quantity = -1
                };
            }

            return res;
        }

        public StatConfigData GetStatConfigData(StatBase stat)
        {
            return _statDatas.TryGetValue(stat.GetType(), out StatConfigData value) ? value : null;
        }

        public InventoryItem GetInventoryItemDefinition(string itemId)
        {
            return _inventoryItemDefinitions.FirstOrDefault(definition => string.Equals(definition.Id, itemId));
        }
    }
}