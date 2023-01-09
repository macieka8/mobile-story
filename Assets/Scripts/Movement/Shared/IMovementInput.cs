using UnityEngine;
using System;

namespace Game
{
    public interface IMovementInput
    {
        public event Action<Vector2> OnMovementInput;
    }
}
