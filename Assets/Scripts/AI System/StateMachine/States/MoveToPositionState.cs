using UnityEngine;
using System.Collections;
namespace Game
{
    public class MoveToPositionState : State
    {
        IMovementController _movementController;
        [SerializeField] Transform _destination;

        void Awake()
        {
            _movementController = GetComponentInParent<IMovementController>();
        }

        public override void Enter()
        {
            base.Enter();
            _movementController.MoveTo(_destination.position);
        }

        public override void Exit()
        {
            _movementController.SetMovementInput(Vector2.zero);
            base.Exit();
        }
    }
}
