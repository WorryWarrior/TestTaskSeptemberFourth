using Content.Data.Inventory;
using Content.Gameplay.Code.Inventory.Contracts;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;

namespace Content.Gameplay.Code.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IItemInteractionService _itemInteractionService;

        public InventoryService(
            IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService,
            IItemInteractionService itemInteractionService
        )
        {
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
            _itemInteractionService = itemInteractionService;
        }

        public event OnInventorySlotUpdatedEventHandler OnInventorySlotUpdated;
        public int InventorySize => _persistentDataService.Inventory.ItemSlots.Length;

        public bool TryGetInventoryItem(int itemSlotIndex, out ItemSlotData itemSlotData)
        {
            itemSlotData = null;

            if (GetInventorySlot(itemSlotIndex).InventoryItem == null)
                return false;

            itemSlotData = GetInventorySlot(itemSlotIndex);

            return true;
        }

        public bool TryAddInventoryItem(InventoryItem inventoryItem, int count)
        {
            EligibleSlotSearchResult res = TryGetEligibleInventorySlot(inventoryItem, count, out int slotIndex);

            switch (res)
            {
                case EligibleSlotSearchResult.None:
                    return false;
                case EligibleSlotSearchResult.Merge:
                    GetInventorySlot(slotIndex).Quantity += count;
                    break;
                case EligibleSlotSearchResult.Insert:
                    GetInventorySlot(slotIndex).InventoryItem = inventoryItem;
                    GetInventorySlot(slotIndex).Quantity = count;
                    break;
            }

            OnInventorySlotUpdated?.Invoke(slotIndex);

            _saveLoadService.SaveInventory();

            return true;
        }

        public void DeleteInventoryItem(int itemSlotIndex)
        {
            ItemSlotData itemSlot = GetInventorySlot(itemSlotIndex);
            itemSlot.InventoryItem = null;
            itemSlot.Quantity = -1;

            OnInventorySlotUpdated?.Invoke(itemSlotIndex);

            _saveLoadService.SaveInventory();
        }

        public void UseInventoryItem(int itemSlotIndex)
        {
            InventoryItem inventoryItem = GetInventorySlot(itemSlotIndex).InventoryItem;

            if (inventoryItem == null)
                return;

            _itemInteractionService.UseItem(inventoryItem);
            DecrementSlotItemQuantity(itemSlotIndex);

            _saveLoadService.SaveInventory();
        }

        public void SwapOrMergeInventorySlots(int firstSlotIndex, int secondSlotIndex)
        {
            ItemSlotData firstItemSlot = GetInventorySlot(firstSlotIndex);
            ItemSlotData secondItemSlot = GetInventorySlot(secondSlotIndex);

            if (firstItemSlot.InventoryItem == null || secondItemSlot.InventoryItem == null ||
                !firstItemSlot.InventoryItem.Id.Equals(secondItemSlot.InventoryItem.Id) ||
                firstItemSlot.Quantity + secondItemSlot.Quantity > secondItemSlot.InventoryItem.MaxStackQuantity)
            {
                (_persistentDataService.Inventory.ItemSlots[firstSlotIndex],
                        _persistentDataService.Inventory.ItemSlots[secondSlotIndex]) =
                    (_persistentDataService.Inventory.ItemSlots[secondSlotIndex],
                        _persistentDataService.Inventory.ItemSlots[firstSlotIndex]);
            }
            else
            {
                GetInventorySlot(secondSlotIndex).Quantity += GetInventorySlot(firstSlotIndex).Quantity;

                DeleteInventoryItem(firstSlotIndex);
            }

            OnInventorySlotUpdated?.Invoke(firstSlotIndex);
            OnInventorySlotUpdated?.Invoke(secondSlotIndex);

            _saveLoadService.SaveInventory();
        }

        private ItemSlotData GetInventorySlot(int slotIndex) => _persistentDataService.Inventory.ItemSlots[slotIndex];

        private void DecrementSlotItemQuantity(int itemSlotIndex)
        {
            ItemSlotData itemSlot = GetInventorySlot(itemSlotIndex);

            itemSlot.Quantity--;

            if (itemSlot.Quantity <= 0)
            {
                DeleteInventoryItem(itemSlotIndex);
            }

            OnInventorySlotUpdated?.Invoke(itemSlotIndex);
        }

        private EligibleSlotSearchResult TryGetEligibleInventorySlot(InventoryItem inventoryItem, int count, out int freeSlotIndex)
        {
            freeSlotIndex = -1;

            for (int i = 0; i < InventorySize; i++)
            {
                ItemSlotData currentSlot = GetInventorySlot(i);

                if (currentSlot.InventoryItem != null && string.Equals(currentSlot.InventoryItem.Id, inventoryItem.Id) &&
                    currentSlot.Quantity + count <= currentSlot.InventoryItem.MaxStackQuantity)
                {
                    freeSlotIndex = i;
                    return EligibleSlotSearchResult.Merge;
                }
            }

            for (int i = 0; i < InventorySize; i++)
            {
                ItemSlotData currentSlot = GetInventorySlot(i);

                if (currentSlot.InventoryItem == null)
                {
                    freeSlotIndex = i;
                    return EligibleSlotSearchResult.Insert;
                }
            }

            return EligibleSlotSearchResult.None;
        }

        private enum EligibleSlotSearchResult
        {
            None,
            Insert,
            Merge
        }
    }
}