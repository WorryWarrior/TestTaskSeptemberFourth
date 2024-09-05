using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.States.Contracts;

namespace Content.Infrastructure.States
{
    public class LoadGameSceneState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;

        public LoadGameSceneState(
            GameStateMachine stateMachine,
            ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public async void Enter()
        {
            await _sceneLoader.LoadScene(SceneName.Core, OnGameSceneLoaded);
        }

        public void Exit()
        {
        }

        private void OnGameSceneLoaded(SceneName sceneName)
        {
            _stateMachine.Enter<LoadLevelState>();
        }
    }
}