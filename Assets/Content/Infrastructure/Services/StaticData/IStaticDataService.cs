using Content.Data.Inventory;
using Content.Data.Stats;
using Content.Gameplay.Code.Inventory;
using Content.Gameplay.Code.Stats.Contracts;
using Zenject;

namespace Content.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IInitializable
    {
        CharacterStatData GetDefaultCharacterStatData();
        InventoryData GetDefaultInventoryData();
        StatConfigData GetStatConfigData(StatBase stat);
        InventoryItem GetInventoryItemDefinition(string itemId);
    }
}