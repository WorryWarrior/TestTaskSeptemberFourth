using Content.Data.Inventory;
using Content.Gameplay.Code.Inventory;
using Content.Gameplay.Code.Inventory.Contracts;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Services.Logging;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Content.UI.Code.InventoryView
{
    public class InventoryHUDController : MonoBehaviour
    {
        [SerializeField] private InventoryHUDView inventoryHUDView;

        private IInventoryService _inventoryService;
        private ILoggingService _loggingService;

        private IAssetProvider _assetProvider;

        [Inject]
        private void Construct(
            IInventoryService inventoryService,
            IAssetProvider assetProvider,
            ILoggingService loggingService
        )
        {
            _inventoryService = inventoryService;
            _loggingService = loggingService;
            _assetProvider = assetProvider;
        }

        public async UniTask Initialize()
        {
            _inventoryService.OnInventorySlotUpdated += async index => await RefreshInventorySlot(index);

            inventoryHUDView.OnInventorySlotUseRequested    += UseInventorySlot;
            inventoryHUDView.OnInventorySlotsSwapRequested  += SwapInventorySlots;
            inventoryHUDView.OnInventorySlotDeleteRequested += ClearInventorySlot;
            inventoryHUDView.OnInventorySlotHoverStarted    += UpdateSelectedSlotItemDescription;
            inventoryHUDView.OnInventorySlotHoverEnded      += DisableSelectedSlotItemDescription;

            await inventoryHUDView.CreateInventorySlots(_inventoryService.InventorySize);
            await inventoryHUDView.CreateInventoryDragPreview();

            await RefreshAllInventorySlots();

            inventoryHUDView.ToggleButtonPressed = () => inventoryHUDView.ToggleVisibility();
            inventoryHUDView.Initialize();
        }

        private async UniTask RefreshAllInventorySlots()
        {
            for (int i = 0; i < _inventoryService.InventorySize; i++)
            {
                await RefreshInventorySlot(i);
            }
        }

        private async UniTask RefreshInventorySlot(int inventorySlotIndex)
        {
            InventorySlotController inventorySlotController = inventoryHUDView.GetInventorySlot(inventorySlotIndex);

            if (_inventoryService.TryGetInventoryItem(inventorySlotController.SlotIndex, out ItemSlotData itemSlot))
            {
                Sprite itemSprite = await _assetProvider.Load<Sprite>(itemSlot.InventoryItem.Id);

                inventorySlotController.UpdateSlotItemIcon(itemSprite);
                inventorySlotController.UpdateSlotItemQuantity(itemSlot.Quantity);
            }
            else
            {
                inventorySlotController.SetSlotEmpty();
            }
        }

        private void UpdateSelectedSlotItemDescription(int selectedSlotIndex)
        {
            if (_inventoryService.TryGetInventoryItem(selectedSlotIndex, out ItemSlotData itemSlot))
            {
                InventoryItem item = itemSlot.InventoryItem;

                inventoryHUDView.SetHeaderText(item.StatDelta > 0
                    ? $"{item.Name}: {item.StatType} +{item.StatDelta}"
                    : $"{item.Name}: {item.StatType} {item.StatDelta}");
            }
        }

        private void DisableSelectedSlotItemDescription()
        {
            inventoryHUDView.SetHeaderText("");
        }

        private void UseInventorySlot(int slotIndex) => _inventoryService.UseInventoryItem(slotIndex);
        private void ClearInventorySlot(int slotIndex) => _inventoryService.DeleteInventoryItem(slotIndex);
        private void SwapInventorySlots(int firstSlotIndex, int secondSlotIndex) =>
            _inventoryService.SwapOrMergeInventorySlots(firstSlotIndex, secondSlotIndex);
    }
}