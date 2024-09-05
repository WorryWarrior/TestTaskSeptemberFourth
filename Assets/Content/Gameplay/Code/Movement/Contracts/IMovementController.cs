namespace Content.Gameplay.Code.Contracts
{
    public interface IMovementController
    {
        float MovementSpeed { get; }
        void Move();
    }
}