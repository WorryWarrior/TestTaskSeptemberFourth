using Content.Data.Inventory;
using Content.Data.Stats;

namespace Content.Infrastructure.Services.PersistentData
{
    public interface IPersistentDataService
    {
        CharacterStatData CharacterStat { get; set; }
        InventoryData Inventory { get; set; }
    }
}