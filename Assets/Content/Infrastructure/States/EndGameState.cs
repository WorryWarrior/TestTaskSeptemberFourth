using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.Services.StaticData;
using Content.Infrastructure.States.Contracts;

namespace Content.Infrastructure.States
{
    public class EndGameState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneLoader _sceneLoader;
        
        public EndGameState(
            GameStateMachine stateMachine,
            IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService,
            IStaticDataService staticDataService,
            ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
            _staticDataService = staticDataService;
            _sceneLoader = sceneLoader;
        }

        public async void Enter()
        {
            await _sceneLoader.LoadScene(SceneName.Boot, OnBootSceneLoaded);
        }

        public void Exit()
        {
            
        }

        private void OnBootSceneLoaded(SceneName sceneName)
        {
            _persistentDataService.Inventory = _staticDataService.GetDefaultInventoryData();
            _persistentDataService.CharacterStat = _staticDataService.GetDefaultCharacterStatData();

            _saveLoadService.SaveInventory();
            _saveLoadService.SaveCharacterStats();

            _stateMachine.Enter<BootstrapState>();
        }
    }
}