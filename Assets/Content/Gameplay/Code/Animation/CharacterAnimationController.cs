using Content.Gameplay.Code.Character.Movement;
using UnityEngine;

namespace Content.Gameplay.Character.Animation
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] private CharacterMovementController movementController;
        [SerializeField] private Animator animator;

        private readonly int _animatorMovementToggleName = Animator.StringToHash("IsMoving");

        private void Update()
        {
            animator.SetBool(_animatorMovementToggleName, movementController.MovementSpeed > 0f);
        }
    }
}