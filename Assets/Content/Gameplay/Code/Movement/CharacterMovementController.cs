using Content.Gameplay.Code.Contracts;
using Content.Infrastructure.Services.Input;
using Content.Infrastructure.Services.PersistentData;
using UnityEngine;
using Zenject;

namespace Content.Gameplay.Code.Character.Movement
{
    public class CharacterMovementController : MonoBehaviour, IMovementController
    {
        private const float INPUT_MAGNITUDE_THRESHOLD = 0.01f;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private float movementFactor = 1f;
        [SerializeField, Range(0f, 1f)] private float minInputMagnitude = 0.35f;

        private IInputService _inputService;
        private IPersistentDataService _persistentDataService;

        public float MovementSpeed { get; private set; }

        [Inject]
        private void Construct(
            IInputService inputService,
            IPersistentDataService persistentDataService)
        {
            _inputService = inputService;
            _persistentDataService = persistentDataService;
        }

        private void Update()
        {
            MovementSpeed = _inputService.Magnitude > INPUT_MAGNITUDE_THRESHOLD ?
                Mathf.Clamp(_inputService.Magnitude, minInputMagnitude, 1f) *
                movementFactor * (1f + 0.25f * _persistentDataService.CharacterStat.Endurance.CurrentValue) : 0f;
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void Move()
        {
            if (MovementSpeed > 0f)
            {
                float angle = Mathf.Atan2(_inputService.Direction.y, -_inputService.Direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.up);
                rb.rotation = rotation;

                rb.MovePosition(rb.position + transform.forward * (MovementSpeed * Time.fixedDeltaTime));
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                //transform.position += transform.forward * MovementSpeed * Time.deltaTime;
            }
        }
    }
}