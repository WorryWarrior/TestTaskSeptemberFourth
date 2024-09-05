using Content.Data.Inventory;
using Content.Data.Stats;

namespace Content.Infrastructure.Services.PersistentData
{
    public class PersistentDataService : IPersistentDataService
    {
        public CharacterStatData CharacterStat { get; set; }
        public InventoryData Inventory { get; set; }
    }
}