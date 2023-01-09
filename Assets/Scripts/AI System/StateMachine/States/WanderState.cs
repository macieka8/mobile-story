using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class WanderState : State
    {
        [SerializeField] TransformVariable _wanderTransform;
        [SerializeField] float _notMovingTimeInSeconds;

        Collider2D _wanderPlace;

        IMovementController _movementController;
        Vector2 _currentDestination;
        float _minDistanceSqr = 0.3f * 0.3f;
        Coroutine _coroutine;

        void Awake()
        {
            _movementController = GetComponentInParent<IMovementController>();
            _wanderPlace = _wanderTransform.Value.GetComponent<Collider2D>();
        }

        IEnumerator WanderCoroutine()
        {
            _currentDestination = GetRandomPointInsideCollider(_wanderPlace);
            _movementController.MoveTo(_currentDestination);

            while (true)
            {
                var vectorToDestination = _currentDestination - (Vector2)transform.position;
                if (vectorToDestination.sqrMagnitude < _minDistanceSqr)
                {
                    // Reached destination, stop moving for specified time
                    yield return new WaitForSeconds(_notMovingTimeInSeconds);
                    _currentDestination = GetRandomPointInsideCollider(_wanderPlace);
                    _movementController.MoveTo(_currentDestination);
                }
                yield return null;
            }
        }

        public override void Enter()
        {
            base.Enter();
            _coroutine = StartCoroutine(WanderCoroutine());
        }

        public override void Exit()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            base.Exit();
        }

        Vector2 GetRandomPointInsideCollider(Collider2D collider)
        {
            var bounds = collider.bounds;
            var randomPoint = new Vector2();
            do
            {
                randomPoint.x = Random.Range(bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x);
                randomPoint.y = Random.Range(bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y);

            } while (!collider.OverlapPoint(randomPoint));

            return randomPoint;
        }
    }
}
