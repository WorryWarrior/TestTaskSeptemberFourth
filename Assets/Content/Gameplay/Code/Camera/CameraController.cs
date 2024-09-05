using UnityEngine;

namespace Content.Gameplay.Code.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private float followFactor = 0.35f;

        private Transform _followTarget;

        private void LateUpdate()
        {
            if (!_followTarget)
                return;

            transform.position = Vector3.Lerp(transform.position, _followTarget.position + positionOffset, followFactor);
        }

        public void SetFollowTarget(Transform followTarget, bool snapPosition = false)
        {
            _followTarget = followTarget;

            if (snapPosition)
            {
                transform.position = _followTarget.position + positionOffset;

            }
        }
    }
}