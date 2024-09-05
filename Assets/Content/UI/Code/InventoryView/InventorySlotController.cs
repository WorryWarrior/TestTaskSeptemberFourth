using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Content.UI.Code.InventoryView
{
    public class InventorySlotController : MonoBehaviour,
        IDragHandler, IBeginDragHandler, IEndDragHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        private const int SlotQuantityEmpty = -1;

        [SerializeField] private Button clickButton;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI quantityText;

        public int SlotIndex { get; private set; }
        public bool IsEmpty { get; private set; }
        public Sprite SlotItemSprite => itemIcon.sprite;

        private float _lastClickTime;
        private int _clickCount;

        public event Action<int> OnInventorySlotClicked;
        public event Action<int> OnInventorySlotDragStarted;
        public event Action OnInventorySlotDragEnded;

        public event Action<int> OnInventorySlotHoverStarted;
        public event Action<int> OnInventorySlotHoverEnded;

        public void Initialize(int slotIndex)
        {
            clickButton.onClick.AddListener(ProcessClick);
            SlotIndex = slotIndex;
        }

        public void UpdateSlotItemQuantity(int value)
        {
            string textValue = value > 1 ? value.ToString() : string.Empty;
            quantityText.text = textValue;

            IsEmpty = value == SlotQuantityEmpty;
        }

        public void UpdateSlotItemIcon(Sprite value)
        {
            if (itemIcon == null)
                return;

            itemIcon.sprite = value;
            itemIcon.enabled = value != null;
        }

        public void SetSlotEmpty()
        {
            UpdateSlotItemIcon(null);
            UpdateSlotItemQuantity(SlotQuantityEmpty);
        }

        private void ProcessClick()
        {
            if (_lastClickTime + 0.75f < Time.time)
            {
                _clickCount = 0;
            }

            _clickCount++;

            if (_clickCount == 2)
            {
                _clickCount = 0;
                OnInventorySlotClicked?.Invoke(SlotIndex);
            }

            _lastClickTime = Time.time;
        }

        public void OnBeginDrag(PointerEventData eventData) => OnInventorySlotDragStarted?.Invoke(SlotIndex);
        public void OnEndDrag(PointerEventData eventData) => OnInventorySlotDragEnded?.Invoke();
        public void OnPointerEnter(PointerEventData eventData) => OnInventorySlotHoverStarted?.Invoke(SlotIndex);
        public void OnPointerExit(PointerEventData eventData) => OnInventorySlotHoverEnded?.Invoke(SlotIndex);
        public void OnDrag(PointerEventData eventData) { }

        private void OnDestroy()
        {
            itemIcon = null;
        }
    }
}