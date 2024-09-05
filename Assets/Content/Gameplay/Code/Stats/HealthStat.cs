using Content.Gameplay.Code.Stats.Contracts;
using Content.Infrastructure.States;
using Zenject;

namespace Content.Gameplay.Code.Stats
{
    public class HealthStat : StatBase
    {
        private GameStateMachine _stateMachine;
        public override StatType StatType => StatType.Health;

        [Inject]
        private void Construct(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void ChangeValue(int delta)
        {
            base.ChangeValue(delta);

            if (CurrentValue == 0)
            {
                _stateMachine.Enter<EndGameState>();
            }
        }
    }
}