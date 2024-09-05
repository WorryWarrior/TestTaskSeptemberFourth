using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.States.Contracts;

namespace Content.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;

        public BootstrapState(
            GameStateMachine gameStateMachine)
        {
            _stateMachine = gameStateMachine;
        }

        public void Enter()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        {

        }
    }
}