using UnityEngine;

namespace Game
{
    public interface IMovementController
    {
        public void SetMovementInput(Vector2 movementInput);
        public void MoveTo(Vector2 destination);
        public Vector2 MovementDirection { get; }
        public bool IsFollowingPath { get; }
    }
}
