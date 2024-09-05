using System.Collections.Generic;
using Content.Data.Stats;
using Content.Gameplay.Code.Stats.Contracts;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Contracts;
using Content.Infrastructure.Services.StaticData;
using Content.UI.Code.CharacterStatView;
using Content.UI.Code.InventoryView;
using Content.UI.Code.LoadingView;
using Content.UI.Code.RootView;
using Content.UI.Code.SettingsView;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Content.Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPrefabId                      = "PFB_UIRoot";
        private const string LoadingViewPrefabId                 = "PFB_LoadingView";
        private const string JoystickViewPrefabId                = "PFB_JoystickView";
        private const string CharacterStatViewPrefabId           = "PFB_CharacterStatView";
        private const string CharacterStatEntryPrefabId          = "PFB_CharacterStatEntry";
        private const string SettingsViewPrefabId                = "PFB_SettingsView";
        private const string InventoryViewPrefabId               = "PFB_InventoryView";
        private const string InventorySlotPrefabId               = "PFB_InventorySlot";
        private const string InventorDragPreviewPrefabId         = "PFB_InventoryDragPreview";

        private const string UIViewLabel                         = "LAB_UIView";

        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        private UIRootView _uiRoot;
        private LoadingView _loadingView;

        public UIFactory(
            DiContainer container,
            IAssetProvider assetProvider,
            IStaticDataService staticDataService)
        {
            _container = container;
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public async UniTask WarmUp()
        {
            await _assetProvider.LoadByLabel<GameObject>(UIViewLabel);
            /*await _assetProvider.Load<GameObject>(UIRootPrefabId);
            await _assetProvider.Load<GameObject>(LoadingViewPrefabId);
            await _assetProvider.Load<GameObject>(JoystickViewPrefabId);
            await _assetProvider.Load<GameObject>(CharacterStatViewPrefabId);
            await _assetProvider.Load<GameObject>(CharacterStatEntryPrefabId);

            await _assetProvider.Load<GameObject>(InventoryViewPrefabId);
            await _assetProvider.Load<GameObject>(InventorySlotPrefabId);
            await _assetProvider.Load<GameObject>(InventorDragPreviewPrefabId);*/
        }

        public void CleanUp()
        {
            _assetProvider.Release(UIViewLabel);
            /*_assetProvider.Release(LoadingViewPrefabId);
            _assetProvider.Release(JoystickViewPrefabId);
            _assetProvider.Release(CharacterStatViewPrefabId);
            _assetProvider.Release(CharacterStatEntryPrefabId);

            _assetProvider.Release(InventoryViewPrefabId);
            _assetProvider.Release(InventorySlotPrefabId);
            _assetProvider.Release(InventorDragPreviewPrefabId);*/
        }

        public async UniTask CreateUIRoot()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(UIRootPrefabId);
            _uiRoot = Object.Instantiate(prefab).GetComponent<UIRootView>();
        }

        public async UniTask<LoadingView> CreateOrGetLoadingView()
        {
            if (_loadingView != null)
            {
                return _loadingView;
            }

            GameObject prefab = await _assetProvider.Load<GameObject>(LoadingViewPrefabId);
            LoadingView view = Object.Instantiate(prefab, _uiRoot.Foreground).GetComponent<LoadingView>();

            return view;
        }

        public async UniTask CreateJoystickView()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(JoystickViewPrefabId);
            Object.Instantiate(prefab, _uiRoot.Background).GetComponent<Canvas>();
        }

        public async UniTask<CharacterStatView> CreateCharacterStatView(IEnumerable<StatBase> stats)
        {
            GameObject viewPrefab  = await _assetProvider.Load<GameObject>(CharacterStatViewPrefabId);
            GameObject entryPrefab = await _assetProvider.Load<GameObject>(CharacterStatEntryPrefabId);

            CharacterStatView view = Object.Instantiate(viewPrefab, _uiRoot.Background).GetComponent<CharacterStatView>();

            foreach (StatBase stat in stats)
            {
                StatConfigData statConfigData = _staticDataService.GetStatConfigData(stat);

                CharacterStatEntry statEntry = Object.Instantiate(entryPrefab, view.StatEntryContainer).GetComponent<CharacterStatEntry>();
                statEntry.Initialize(statConfigData.StatName, statConfigData.DisplayMaxValue);

                view.RegisterStatEntry((int)stat.StatType, statEntry);
                view.RefreshStatEntry((int)stat.StatType, stat.CurrentValue, stat.MaxValue);

                stat.OnValueChanged += (value, maxValue) =>
                    view.RefreshStatEntry((int)stat.StatType, value, maxValue);
            }

            return view;
        }

        public async UniTask<SettingsView> CreateSettingsView()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(SettingsViewPrefabId);
            SettingsView view = Object.Instantiate(prefab, _uiRoot.Background).GetComponent<SettingsView>();

            _container.InjectGameObject(view.gameObject);
            view.Initialize();

            return view;
        }

        public async UniTask<InventoryHUDController> CreateInventoryView()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(InventoryViewPrefabId);
            InventoryHUDController hud = Object.Instantiate(prefab, _uiRoot.Background).GetComponent<InventoryHUDController>();

            _container.InjectGameObject(hud.gameObject);
            await hud.Initialize();

            return hud;
        }

        public async UniTask<InventorySlotController> CreateInventorySlot()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(InventorySlotPrefabId);
            InventorySlotController slot = Object.Instantiate(prefab, _uiRoot.Background).GetComponent<InventorySlotController>();

            return slot;
        }

        public async UniTask<InventoryDragPreviewController> CreateInventoryDragPreview()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(InventorDragPreviewPrefabId);
            InventoryDragPreviewController dragPreview = Object.Instantiate(prefab, _uiRoot.Background).GetComponent<InventoryDragPreviewController>();

            dragPreview.Initialize(_uiRoot.GetComponent<Canvas>());
            return dragPreview;
        }
    }
}