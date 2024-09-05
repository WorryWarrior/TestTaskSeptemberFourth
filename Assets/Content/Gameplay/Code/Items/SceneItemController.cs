using Content.Gameplay.Code.Character.Movement;
using Content.Gameplay.Code.Inventory.Contracts;
using Content.Infrastructure.Services.StaticData;
using UnityEngine;
using Zenject;

namespace Content.Gameplay.Code.Items
{
    public class SceneItemController : MonoBehaviour
    {
        [SerializeField] private string inventoryItemId;

        private IInventoryService _inventoryService;
        private IStaticDataService _staticDataService;

        [Inject]
        private void Construct(
            IInventoryService inventoryService,
            IStaticDataService staticDataService)
        {
            _inventoryService = inventoryService;
            _staticDataService = staticDataService;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<CharacterMovementController>() != null)
            {
                _inventoryService.TryAddInventoryItem(_staticDataService.GetInventoryItemDefinition(inventoryItemId), 1);

                Destroy(gameObject);
            }
        }
    }
}