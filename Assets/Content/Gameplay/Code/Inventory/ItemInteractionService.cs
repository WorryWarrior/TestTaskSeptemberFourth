using System;
using Content.Gameplay.Code.Inventory.Contracts;
using Content.Gameplay.Code.Stats;
using Content.Gameplay.Code.Stats.Contracts;
using UnityEngine;

namespace Content.Gameplay.Code.Inventory
{
    public class ItemInteractionService : IItemInteractionService
    {
        private GameObject _characterInstance;

        public void InitializeCharacter(GameObject characterInstance)
        {
            _characterInstance = characterInstance;
        }

        public void UseItem(InventoryItem item)
        {
            StatBase characterStat = GetCharacterStat(item.StatType);

            if (item.StatType != StatType.Health)
            {
                characterStat.ChangeMaxValue(item.StatDelta);
            }

            characterStat.ChangeValue(item.StatDelta);
        }

        private StatBase GetCharacterStat(StatType statType)
        {
            return statType switch
            {
                StatType.Health => _characterInstance.GetComponent<HealthStat>(),
                StatType.Wisdom => _characterInstance.GetComponent<WisdomStat>(),
                StatType.Endurance => _characterInstance.GetComponent<EnduranceStat>(),
                StatType.Strength => _characterInstance.GetComponent<StrengthStat>(),
                _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
            };
        }
    }
}