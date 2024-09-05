using TMPro;
using UnityEngine;

namespace Content.UI.Code.CharacterStatView
{
    public class CharacterStatEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statText;

        private string _statName;
        private bool _displayMaxValue;

        public void Initialize(string statName, bool displayMaxValue)
        {
            _statName = statName;
            _displayMaxValue = displayMaxValue;
        }

        public void Refresh(int currentValue, int maxValue)
        {
            statText.text = _displayMaxValue ? $"{_statName}: {currentValue}/{maxValue}" : $"{_statName}: {currentValue}";
        }
    }
}