using System;
using Content.Gameplay.Code.Stats;

namespace Content.Gameplay.Code.Inventory
{
    [Serializable]
    public class InventoryItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int MaxStackQuantity { get; set; }
        public int StatDelta { get; set; }
        public StatType StatType { get; set; }
    }
}