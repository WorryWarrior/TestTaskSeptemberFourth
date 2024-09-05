using Content.Data.Inventory;

namespace Content.Gameplay.Code.Inventory.Contracts
{
    public delegate void OnInventorySlotUpdatedEventHandler(int inventorySlotIndex);

    public interface IInventoryService
    {
        public event OnInventorySlotUpdatedEventHandler OnInventorySlotUpdated;
        public int InventorySize { get; }

        public bool TryAddInventoryItem(InventoryItem inventoryItem, int count);
        public void DeleteInventoryItem(int itemSlotIndex);
        public bool TryGetInventoryItem(int itemSlotIndex, out ItemSlotData itemSlotData);
        public void UseInventoryItem(int itemSlotIndex);
        public void SwapOrMergeInventorySlots(int firstSlotIndex, int secondSlotIndex);
    }
}