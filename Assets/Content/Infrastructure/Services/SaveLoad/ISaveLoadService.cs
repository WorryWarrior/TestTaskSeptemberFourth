using Content.Data.Inventory;
using Content.Data.Stats;
using Cysharp.Threading.Tasks;

namespace Content.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveCharacterStats();
        UniTask<CharacterStatData> LoadCharacterStats();

        void SaveInventory();
        UniTask<InventoryData> LoadInventory();
    }
}