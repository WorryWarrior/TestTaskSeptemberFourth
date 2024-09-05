using System.Collections.Generic;
using Content.Gameplay.Code.Stats;
using Content.Gameplay.Code.Stats.Contracts;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.Watchers.Contracts;

namespace Content.Infrastructure.Watchers
{
    public class CharacterStatWatcher : ICharacterStatWatcher
    {
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;

        private IEnumerable<StatBase> _characterStats;

        public CharacterStatWatcher(
            IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService)
        {
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
        }

        public void Initialize(IEnumerable<StatBase> characterStats)
        {
            _characterStats = characterStats;

            foreach (StatBase stat in _characterStats)
            {
                stat.OnValueChanged += (a, b) => SynchronizeSavedData(a, b, stat.StatType);
            }
        }

        private void SynchronizeSavedData(int currentValue, int maxValue, StatType statType)
        {
            foreach (StatBase stat in _characterStats)
            {
                if (stat.StatType != statType)
                    continue;

                switch (stat.StatType)
                {
                    case StatType.Health:
                        _persistentDataService.CharacterStat.Health.CurrentValue = currentValue;
                        _persistentDataService.CharacterStat.Health.MaxValue = maxValue;
                        break;
                    case StatType.Wisdom:
                        _persistentDataService.CharacterStat.Wisdom.CurrentValue = currentValue;
                        _persistentDataService.CharacterStat.Wisdom.MaxValue = maxValue;
                        break;
                    case StatType.Endurance:
                        _persistentDataService.CharacterStat.Endurance.CurrentValue = currentValue;
                        _persistentDataService.CharacterStat.Endurance.MaxValue = maxValue;
                        break;
                    case StatType.Strength:
                        _persistentDataService.CharacterStat.Strength.CurrentValue = currentValue;
                        _persistentDataService.CharacterStat.Strength.MaxValue = maxValue;
                        break;
                    default:
                        break;
                }
            }

            _saveLoadService.SaveCharacterStats();
        }
    }
}