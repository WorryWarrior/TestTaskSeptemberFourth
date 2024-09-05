using System.Collections.Generic;
using Content.Data.Stats;
using Content.Gameplay.Code.Camera;
using Content.Gameplay.Code.Inventory.Contracts;
using Content.Gameplay.Code.Items;
using Content.Gameplay.Code.Level;
using Content.Gameplay.Code.Stats;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Contracts;
using Content.Infrastructure.Services.PersistentData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Content.Infrastructure.Factories
{
    public class LevelFactory : ILevelFactory
    {
        private const string LevelPrefabId     = "PFB_Level";
        private const string CharacterPrefabId = "PFB_Character";
        private const string CameraPrefabId    = "PFB_Camera";
        private const string ItemPrefabLabel   = "LAB_Items";

        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IPersistentDataService _persistentDataService;
        private readonly IItemInteractionService _itemInteractionService;

        public LevelFactory(
            DiContainer container,
            IAssetProvider assetProvider,
            IPersistentDataService persistentDataService,
            IItemInteractionService itemInteractionService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _persistentDataService = persistentDataService;
            _itemInteractionService = itemInteractionService;
        }

        public async UniTask WarmUp()
        {
            await _assetProvider.Load<GameObject>(CharacterPrefabId);
            await _assetProvider.Load<GameObject>(LevelPrefabId);
            await _assetProvider.Load<GameObject>(CameraPrefabId);

            await _assetProvider.LoadByLabel<GameObject>(ItemPrefabLabel);
        }

        public void CleanUp()
        {
            _assetProvider.Release(CharacterPrefabId);
            _assetProvider.Release(LevelPrefabId);
            _assetProvider.Release(CameraPrefabId);

            _assetProvider.Release(ItemPrefabLabel);
        }

        public async UniTask<LevelController> CreateLevel()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(LevelPrefabId);
            LevelController controller = Object.Instantiate(prefab).GetComponent<LevelController>();

            _container.InjectGameObject(controller.gameObject);

            return controller;
        }

        public async UniTask<CameraController> CreateCamera()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(CameraPrefabId);
            CameraController controller = Object.Instantiate(prefab).GetComponent<CameraController>();

            _container.InjectGameObject(controller.gameObject);

            return controller;
        }

        public async UniTask<GameObject> CreateCharacter()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(CharacterPrefabId);
            GameObject character = Object.Instantiate(prefab);

            _itemInteractionService.InitializeCharacter(character);

            CharacterStatData characterStatData = _persistentDataService.CharacterStat;

            character.GetComponent<HealthStat>().SetMaxValue(characterStatData.Health.MaxValue);
            character.GetComponent<HealthStat>().SetCurrentValue(characterStatData.Health.CurrentValue);
            character.GetComponent<EnduranceStat>().SetMaxValue(characterStatData.Endurance.MaxValue);
            character.GetComponent<WisdomStat>().SetMaxValue(characterStatData.Wisdom.MaxValue);
            character.GetComponent<StrengthStat>().SetMaxValue(characterStatData.Strength.MaxValue);

            _container.InjectGameObject(character);

            return character;
        }

        public async UniTask<SceneItemController> CreateRandomItem()
        {
            IList<GameObject> prefabs = await _assetProvider.LoadByLabel<GameObject>(ItemPrefabLabel);
            SceneItemController item = Object.Instantiate(prefabs[UnityEngine.Random.Range(0, prefabs.Count)])
                .GetComponent<SceneItemController>();

            _container.InjectGameObject(item.gameObject);

            return item;
        }
    }
}