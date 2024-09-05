using UnityEngine;

namespace Content.Gameplay.Code.Inventory.Contracts
{
    public interface IItemInteractionService
    {
        void InitializeCharacter(GameObject characterInstance);
        void UseItem(InventoryItem item);
    }
}