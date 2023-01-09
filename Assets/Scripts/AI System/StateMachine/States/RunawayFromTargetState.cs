using UnityEngine;

namespace Game
{
    public class RunawayFromTargetState : State
    {
        ITargetingHelper _targetingHelper;
        IMovementController _movementController;

        void Awake()
        {
            _targetingHelper = GetComponentInParent<ITargetingHelper>();
            _movementController = GetComponentInParent<IMovementController>();
        }

        void Update()
        {
            if (_targetingHelper.Target == null) return;
            var direction = (_targetingHelper.Target.Position - (Vector2)transform.position).normalized;
            _movementController.SetMovementInput(-direction);
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            _movementController.SetMovementInput(Vector2.zero);
            base.Exit();
        }
    }
}
