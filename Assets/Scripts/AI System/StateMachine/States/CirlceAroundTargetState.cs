using System.Collections;
using UnityEngine;

namespace Game
{
    public class CirlceAroundTargetState : State
    {
        [SerializeField] float _circlingDistance;

        IMovementController _movementController;
        ITargetingHelper _targetingHelper;
        bool _circleClockwise;

        Coroutine _directionCoroutine;

        void Awake()
        {
            _movementController = GetComponentInParent<IMovementController>();
            _targetingHelper = GetComponentInParent<ITargetingHelper>();
        }

        void Start()
        {
        }

        void Update()
        {
            if (_targetingHelper.Target == null) return;
            var directiontoTarget = (_targetingHelper.Target.Position - (Vector2)transform.position).normalized;
            var distanceToTarget = Vector2.Distance(_targetingHelper.Target.Position, transform.position);
            var perpendicularDir = Vector2.Perpendicular(directiontoTarget) * (_circleClockwise ? 1 : -1);

            var fromTargetToDesired = (distanceToTarget - _circlingDistance) * directiontoTarget;
            _movementController.SetMovementInput((fromTargetToDesired + perpendicularDir).normalized);
        }

        public override void Enter()
        {
            base.Enter();
            _directionCoroutine = StartCoroutine(ChooseRandomDirectionCoroutine());
        }

        public override void Exit()
        {
            _movementController.SetMovementInput(Vector2.zero);
            if (_directionCoroutine != null) StopCoroutine(_directionCoroutine);
            base.Exit();
        }

        IEnumerator ChooseRandomDirectionCoroutine()
        {
            WaitForSeconds timeToUpdate = new WaitForSeconds(2f);
            while (true)
            {
                _circleClockwise = Random.value > 0.5f;
                yield return timeToUpdate;
            }
        }
    }
}
