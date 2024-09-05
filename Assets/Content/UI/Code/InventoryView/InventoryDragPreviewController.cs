using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Content.UI.Code.InventoryView
{
    public class InventoryDragPreviewController : MonoBehaviour
    {
        [SerializeField] private Image previewImage;

        private Canvas _parentCanvas;
        private bool _isActive;

        public void Initialize(Canvas parentCanvas)
        {
            _parentCanvas = parentCanvas;
        }

        private void Update()
        {
            if (!_isActive)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.transform as RectTransform,
                Mouse.current.position.value, _parentCanvas.worldCamera,
                out Vector2 movePos);

            transform.position = _parentCanvas.transform.TransformPoint(movePos);
        }

        public void TogglePreview(bool value)
        {
            _isActive = value && previewImage.sprite != null;
            previewImage.enabled = _isActive;
        }

        public void SetPreviewSprite(Sprite value)
        {
            previewImage.sprite = value;
        }
    }
}