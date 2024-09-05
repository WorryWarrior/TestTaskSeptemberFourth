using UnityEngine;

namespace Content.Infrastructure.Services.Input
{
    public interface IInputService
    {
        public float Magnitude { get; }
        public Vector2 Direction { get; }
    }
}