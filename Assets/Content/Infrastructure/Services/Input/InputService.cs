using UnityEngine;
using UnityEngine.InputSystem;

namespace Content.Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        private readonly PlayerControls _playerControls;

        public float Magnitude { get; private set; }
        public Vector2 Direction { get; private set; }

        public InputService()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();

            SubscribeControls();
        }
        
        private void SubscribeControls()
        {
            _playerControls.Player.Move.performed += OnMoveTick;
            _playerControls.Player.Move.canceled += OnMoveTick;
        }
        
        private void OnMoveTick(InputAction.CallbackContext context)
        {
            Vector2 dir = context.ReadValue<Vector2>();
            Magnitude = dir.magnitude;
            Direction = dir.normalized;
        }
    }
}