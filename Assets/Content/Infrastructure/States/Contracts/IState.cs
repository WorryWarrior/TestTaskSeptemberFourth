namespace Content.Infrastructure.States.Contracts
{
    public interface IState : IExitableState
    {
        public void Enter();
    }
}