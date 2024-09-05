using System.Collections.Generic;
using UnityEngine;

namespace Content.UI.Code.CharacterStatView
{
    public class CharacterStatView : MonoBehaviour
    {
        [field: SerializeField] public RectTransform StatEntryContainer { get; private set; }

        private readonly Dictionary<int, CharacterStatEntry> _statEntries = new();

        public void RegisterStatEntry(int entryStatId, CharacterStatEntry entry)
        {
            _statEntries.Add(entryStatId, entry);
        }

        public void RefreshStatEntry(int entryStatId, int currentValue, int maxValue)
        {
            _statEntries[entryStatId].Refresh(currentValue, maxValue);
        }
    }
}