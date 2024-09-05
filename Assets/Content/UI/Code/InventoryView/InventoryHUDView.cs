using System;
using System.Collections.Generic;
using Content.Infrastructure.Factories.Contracts;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Content.UI.Code.InventoryView
{
    public delegate void SingleInventorySlotIndexEventHandler(int firstSlotIndex);
    public delegate void DoubleInventorySlotIndexEventHandler(int firstSlotIndex, int secondSlotIndex);

    public class InventoryHUDView : MonoBehaviour
    {
        private const int SLOT_INDEX_UNINITIALIZED_VALUE = -1;

        [SerializeField] private RectTransform window;
        [SerializeField] private RectTransform inventorySlotContainer;
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private Button toggleButton;

        private IUIFactory _uiFactory;

        private readonly List<InventorySlotController> _inventorySlotControllers = new();
        private InventoryDragPreviewController _dragPreviewController;

        private int _dragStartSlotIndex;
        private int _dragEndSlotIndex = SLOT_INDEX_UNINITIALIZED_VALUE;

        public event DoubleInventorySlotIndexEventHandler OnInventorySlotsSwapRequested;
        public event SingleInventorySlotIndexEventHandler OnInventorySlotUseRequested;
        public event SingleInventorySlotIndexEventHandler OnInventorySlotDeleteRequested;
        public event SingleInventorySlotIndexEventHandler OnInventorySlotHoverStarted;
        public event Action OnInventorySlotHoverEnded;

        public Action ToggleButtonPressed;

        [Inject]
        private void Construct(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Initialize()
        {
            toggleButton.onClick.AddListener(() => ToggleButtonPressed?.Invoke());

            ToggleVisibility();
        }

        public async UniTask CreateInventorySlots(int inventorySlotCount)
        {
            for (int i = 0; i < inventorySlotCount; i++)
            {
                InventorySlotController inventorySlot = await _uiFactory.CreateInventorySlot();

                inventorySlot.OnInventorySlotClicked += ProcessInventorySlotUse;

                inventorySlot.OnInventorySlotDragStarted += draggedSlotIndex =>
                {
                    _dragPreviewController.SetPreviewSprite(GetInventorySlot(draggedSlotIndex).SlotItemSprite);
                    _dragPreviewController.TogglePreview(true);

                    _dragStartSlotIndex = draggedSlotIndex;
                };

                inventorySlot.OnInventorySlotDragEnded += () =>
                {
                    _dragPreviewController.TogglePreview(false);

                    if (_dragEndSlotIndex == SLOT_INDEX_UNINITIALIZED_VALUE)
                    {
                        OnInventorySlotDeleteRequested?.Invoke(_dragStartSlotIndex);
                        return;
                    }

                    if (_dragStartSlotIndex != _dragEndSlotIndex && !(GetInventorySlot(_dragStartSlotIndex).IsEmpty
                                                                      && GetInventorySlot(_dragEndSlotIndex).IsEmpty))
                    {
                        OnInventorySlotsSwapRequested?.Invoke(_dragStartSlotIndex, _dragEndSlotIndex);
                    }
                };

                inventorySlot.OnInventorySlotHoverStarted += ProcessInventorySlotHoverStart;
                inventorySlot.OnInventorySlotHoverEnded += ProcessInventorySlotHoverEnd;

                inventorySlot.Initialize(i);
                inventorySlot.transform.SetParent(inventorySlotContainer);
                _inventorySlotControllers.Add(inventorySlot);
            }
        }

        public async UniTask CreateInventoryDragPreview()
        {
            _dragPreviewController = await _uiFactory.CreateInventoryDragPreview();
            _dragPreviewController.TogglePreview(false);
            _dragPreviewController.transform.SetParent(transform);
        }

        public void SetHeaderText(string value)
        {
            headerText.text = value;
        }

        public void ToggleVisibility()
        {
            window.transform.localScale = window.transform.localScale == Vector3.one ? Vector3.zero : Vector3.one;
        }

        public InventorySlotController GetInventorySlot(int inventorySlotIndex) =>
            _inventorySlotControllers[inventorySlotIndex];

        private void ProcessInventorySlotUse(int inventorySlotIndex)
        {
            OnInventorySlotUseRequested?.Invoke(inventorySlotIndex);
        }

        private void ProcessInventorySlotHoverStart(int hoveredSlotIndex)
        {
            _dragEndSlotIndex = hoveredSlotIndex;

            OnInventorySlotHoverStarted?.Invoke(hoveredSlotIndex);
        }

        private void ProcessInventorySlotHoverEnd(int hoveredSlotIndex)
        {
            _dragEndSlotIndex = SLOT_INDEX_UNINITIALIZED_VALUE;

            OnInventorySlotHoverEnded?.Invoke();
        }
    }
}