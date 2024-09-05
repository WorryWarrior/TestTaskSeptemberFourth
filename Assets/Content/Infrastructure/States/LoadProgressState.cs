using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.Services.StaticData;
using Content.Infrastructure.States.Contracts;
using Cysharp.Threading.Tasks;

namespace Content.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataService _staticDataService;

        public LoadProgressState(
            GameStateMachine stateMachine,
            IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService,
            IStaticDataService staticDataService)
        {
            _stateMachine = stateMachine;
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
            _staticDataService = staticDataService;
        }

        public async void Enter()
        {
            await LoadProgressOrCreateNew();

            _stateMachine.Enter<LoadGameSceneState>();
        }

        public void Exit()
        {

        }

        private async UniTask LoadProgressOrCreateNew()
        {
            _persistentDataService.Inventory = await _saveLoadService.LoadInventory() ?? _staticDataService.GetDefaultInventoryData();
            _persistentDataService.CharacterStat = await _saveLoadService.LoadCharacterStats() ?? _staticDataService.GetDefaultCharacterStatData();

            _saveLoadService.SaveCharacterStats();
            _saveLoadService.SaveInventory();
        }
    }
}