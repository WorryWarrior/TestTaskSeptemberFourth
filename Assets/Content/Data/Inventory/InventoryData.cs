using System;

namespace Content.Data.Inventory
{
    [Serializable]
    public class InventoryData
    {
        public ItemSlotData[] ItemSlots { get; set; }
    }
}