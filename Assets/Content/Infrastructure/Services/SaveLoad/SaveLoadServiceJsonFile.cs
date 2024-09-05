using System.IO;
using Content.Data.Inventory;
using Content.Data.Stats;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.Services.PersistentData;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using static Newtonsoft.Json.JsonConvert;

namespace Content.Infrastructure.Services.SaveLoad
{
    public class SaveLoadServiceJsonFile : ISaveLoadService
    {
        private const string CharacterStatsSaveFileName = "CharacterStats.json";
        private const string InventorySaveFileName      = "Inventory.json";

        private readonly IPersistentDataService _persistentDataService;
        private readonly ILoggingService _loggingService;

        public SaveLoadServiceJsonFile(
            IPersistentDataService persistentDataService,
            ILoggingService loggingService)
        {
            _persistentDataService = persistentDataService;
            _loggingService = loggingService;
        }

        public void SaveCharacterStats()
        {
            string jsonData = SerializeObject(_persistentDataService.CharacterStat, Formatting.Indented);
            string savePath = GetCharacterStatsSaveFilePath();

            using StreamWriter sw = File.CreateText(savePath);
            sw.WriteLine(jsonData);

            _loggingService.LogMessage($"Saved character stats to file at {savePath}", this);
        }

        public UniTask<CharacterStatData> LoadCharacterStats()
        {
            string savePath = GetCharacterStatsSaveFilePath();

            if (!File.Exists(savePath))
                return UniTask.FromResult<CharacterStatData>(null);

            CharacterStatData statData = DeserializeObject<CharacterStatData>(File.ReadAllText(savePath));

            _loggingService.LogMessage($"Loaded character stats from file at {savePath}", this);

            return UniTask.FromResult(statData);
        }

        public void SaveInventory()
        {
            string jsonData = SerializeObject(_persistentDataService.Inventory, Formatting.Indented);
            string savePath = GetInventorySaveFilePath();

            using StreamWriter sw = File.CreateText(savePath);
            sw.WriteLine(jsonData);

            _loggingService.LogMessage($"Saved inventory to file at {savePath}", this);
        }

        public UniTask<InventoryData> LoadInventory()
        {
            string savePath = GetInventorySaveFilePath();

            if (!File.Exists(savePath))
                return UniTask.FromResult<InventoryData>(null);

            InventoryData data = DeserializeObject<InventoryData>(File.ReadAllText(savePath));

            _loggingService.LogMessage($"Loaded inventory from file at {savePath}", this);

            return UniTask.FromResult(data);
        }

        private string GetInventorySaveFilePath()
        {
            return //$@"E:\Unity\Projects\TestTaskSeptemberFourth\Assets\Saves\{InventorySaveFileName}";
                   $"{UnityEngine.Application.persistentDataPath}{Path.DirectorySeparatorChar}{InventorySaveFileName}";
        }

        private string GetCharacterStatsSaveFilePath()
        {
            return //$@"E:\Unity\Projects\TestTaskSeptemberFourth\Assets\Saves\{CharacterStatsSaveFileName}";
                $"{UnityEngine.Application.persistentDataPath}{Path.DirectorySeparatorChar}{CharacterStatsSaveFileName}";
        }
    }
}