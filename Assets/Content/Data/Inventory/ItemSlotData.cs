using System;
using Content.Gameplay.Code.Inventory;

namespace Content.Data.Inventory
{
    [Serializable]
    public class ItemSlotData
    {
        public InventoryItem InventoryItem { get; set; }
        public int Quantity { get; set; } = -1;
    }
}