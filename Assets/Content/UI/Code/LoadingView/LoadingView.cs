using UnityEngine;

namespace Content.UI.Code.LoadingView
{
    public class LoadingView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        public void Toggle(bool value)
        {
            canvasGroup.alpha = value ? 1f : 0f;
        }
    }
}