using UnityEngine;

namespace Content.Gameplay.Code.Level
{
    public class LevelController : MonoBehaviour
    {
        [field: SerializeField] public Transform CharacterStartPosition { get; private set; }
        [field: SerializeField] public Transform[] ItemPositions { get; private set; }
    }
}