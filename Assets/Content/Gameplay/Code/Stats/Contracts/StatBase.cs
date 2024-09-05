using UnityEngine;

namespace Content.Gameplay.Code.Stats.Contracts
{
    public delegate void OnStatValueChangedEventHandler(int currentValue, int maxValue);

    public abstract class StatBase : MonoBehaviour
    {
        public abstract StatType StatType { get; }
        public int CurrentValue { get; private set; }
        public int MaxValue { get; private set; }

        public event OnStatValueChangedEventHandler OnValueChanged;

        public void SetMaxValue(int maxValue)
        {
            MaxValue = maxValue;

            CurrentValue = MaxValue;
        }

        public void SetCurrentValue(int currentValue)
        {
            CurrentValue = currentValue;
        }

        public virtual void ChangeValue(int delta)
        {
            CurrentValue = Mathf.Clamp(CurrentValue + delta, 0, MaxValue);

            OnValueChanged?.Invoke(CurrentValue, MaxValue);
        }

        public virtual void ChangeMaxValue(int delta)
        {
            MaxValue += delta;

            OnValueChanged?.Invoke(CurrentValue, MaxValue);
        }
    }
}