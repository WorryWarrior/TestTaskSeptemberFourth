using UnityEngine;

namespace Content.UI.Code.RootView
{
    public class UIRootView : MonoBehaviour
    {
        [field: SerializeField] public Transform Background { get; private set; }
        [field: SerializeField] public Transform Foreground { get; private set; }
    }
}