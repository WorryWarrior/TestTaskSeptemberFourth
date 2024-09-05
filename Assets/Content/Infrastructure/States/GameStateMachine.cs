using System;
using System.Collections.Generic;
using Content.Infrastructure.Factories;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.States.Contracts;
using Zenject;

namespace Content.Infrastructure.States
{
    public class GameStateMachine : IInitializable
    {
        private readonly StateFactory    _stateFactory;
        private readonly ILoggingService _logger;

        private Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;

        public GameStateMachine(StateFactory stateFactory, ILoggingService loggingService)
        {
            _stateFactory = stateFactory;
            _logger = loggingService;
        }

        public void Initialize()
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)]      = _stateFactory.CreateState<BootstrapState>(),
                [typeof(LoadProgressState)]   = _stateFactory.CreateState<LoadProgressState>(),
                [typeof(LoadGameSceneState)]  = _stateFactory.CreateState<LoadGameSceneState>(),
                [typeof(LoadLevelState)]      = _stateFactory.CreateState<LoadLevelState>(),
                [typeof(GameLoopState)]       = _stateFactory.CreateState<GameLoopState>(),
                [typeof(EndGameState)]           = _stateFactory.CreateState<EndGameState>(),
            };

            Enter<BootstrapState>();
        }

        public void Enter<TState>() where TState : class, IState =>
            ChangeState<TState>().Enter();

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload> =>
            ChangeState<TState>().Enter(payload);


        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            _logger.LogMessage($"State changed to {_currentState.GetType().Name}", this);

            return state;
        }
    }
}