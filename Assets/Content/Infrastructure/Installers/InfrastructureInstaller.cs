using Content.Gameplay.Code.Inventory;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories;
using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.Services.Input;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.Services.StaticData;
using Content.Infrastructure.States;
using Content.Infrastructure.Watchers;
using Zenject;

namespace Content.Infrastructure.Installers
{
    public class InfrastructureInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ConfigureProviders();
            ConfigureServices();
            ConfigureWatchers();
            ConfigureFactories();

            ConfigureStates();
        }

        private void ConfigureProviders()
        {
            Container.BindInterfacesAndSelfTo<AssetProvider>().AsSingle();
            Container.BindInterfacesTo<SceneLoader>().AsSingle();
        }

        private void ConfigureServices()
        {
            Container.BindInterfacesTo<InputService>().AsSingle();
            Container.BindInterfacesTo<LoggingService>().AsSingle();
            Container.BindInterfacesTo<PersistentDataService>().AsSingle();
            Container.BindInterfacesTo<StaticDataService>().AsSingle();
            Container.BindInterfacesTo<SaveLoadServiceJsonFile>().AsSingle();
            Container.BindInterfacesTo<ItemInteractionService>().AsSingle();
            Container.BindInterfacesTo<InventoryService>().AsSingle();
        }

        private void ConfigureWatchers()
        {
            Container.BindInterfacesTo<CharacterStatWatcher>().AsSingle();
        }

        private void ConfigureFactories()
        {
            Container.Bind<StateFactory>().AsSingle();
            Container.BindInterfacesTo<UIFactory>().AsSingle();
            Container.BindInterfacesTo<LevelFactory>().AsSingle();
        }

        private void ConfigureStates()
        {
            Container.Bind<BootstrapState>().AsSingle().NonLazy();
            Container.Bind<LoadProgressState>().AsSingle().NonLazy();
            Container.Bind<LoadGameSceneState>().AsSingle().NonLazy();
            Container.Bind<LoadLevelState>().AsSingle().NonLazy();
            Container.Bind<GameLoopState>().AsSingle().NonLazy();
            Container.Bind<EndGameState>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
        }
    }
}