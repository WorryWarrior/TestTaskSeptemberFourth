using Content.Gameplay.Code.Camera;
using Content.Gameplay.Code.Items;
using Content.Gameplay.Code.Level;
using Content.Gameplay.Code.Stats.Contracts;
using Content.Infrastructure.Factories.Contracts;
using Content.Infrastructure.States.Contracts;
using Content.Infrastructure.Watchers.Contracts;
using Content.UI.Code.LoadingView;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Content.Infrastructure.States
{
    public class LoadLevelState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ILevelFactory _levelFactory;
        private readonly IUIFactory _uiFactory;
        private readonly ICharacterStatWatcher _characterStatWatcher;

        private GameObject _character;
        private LoadingView _loadingView;

        public LoadLevelState(
            GameStateMachine gameStateMachine,
            ILevelFactory levelFactory,
            IUIFactory uiFactory,
            ICharacterStatWatcher characterStatWatcher)
        {
            _stateMachine = gameStateMachine;
            _levelFactory = levelFactory;
            _uiFactory = uiFactory;
            _characterStatWatcher = characterStatWatcher;
        }

        public async void Enter()
        {
            await _uiFactory.WarmUp();
            await InitLoadingUI();

            await _levelFactory.WarmUp();
            await InitGameWorld();
            await InitUI();

            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            _levelFactory.CleanUp();
            _uiFactory.CleanUp();
        }

        private async UniTask InitGameWorld()
        {
            LevelController levelController = await _levelFactory.CreateLevel();
            CameraController cameraController = await _levelFactory.CreateCamera();

            foreach (Transform itemPosition in levelController.ItemPositions)
            {
                SceneItemController itemInstance = await _levelFactory.CreateRandomItem();
                itemInstance.transform.position = itemPosition.position;
            }

            _character = await _levelFactory.CreateCharacter();

            _character.transform.position = levelController.CharacterStartPosition.position;
            cameraController.SetFollowTarget(_character.transform, snapPosition: true);

            _characterStatWatcher.Initialize(_character.GetComponents<StatBase>());
        }

        private async UniTask InitLoadingUI()
        {
            await _uiFactory.CreateUIRoot();

            _loadingView = await _uiFactory.CreateOrGetLoadingView();
            _loadingView.Toggle(true);
        }

        private async UniTask InitUI()
        {
            await _uiFactory.CreateJoystickView();
            await _uiFactory.CreateCharacterStatView(_character.GetComponents<StatBase>());
            await _uiFactory.CreateSettingsView();
            await _uiFactory.CreateInventoryView();

            _loadingView.Toggle(false);
        }
    }
}