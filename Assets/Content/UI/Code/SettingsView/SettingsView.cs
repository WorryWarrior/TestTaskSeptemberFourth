using Content.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Content.UI.Code.SettingsView
{
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private Button restartButton;

        private GameStateMachine _stateMachine;

        [Inject]
        private void Construct(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Initialize()
        {
            restartButton.onClick.AddListener(() => _stateMachine.Enter<EndGameState>());
        }
    }
}