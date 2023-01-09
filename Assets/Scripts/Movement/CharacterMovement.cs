using UnityEngine;
using System.Collections;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour, IMovementStopper, IMovementController
    {
        [SerializeField] float _acceleration;
        [SerializeField] Attribute _speed;

        Rigidbody2D _rigidbody;
        Vector2 _movement;
        Vector2[] _path;
        Coroutine _followPathCoroutine;

        public Attribute Speed => _speed;
        public bool IsMovementStopped { get; private set; } = false;
        public bool IsFollowingPath { get; private set; } = false;
        public Vector2 MovementDirection => _movement;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (IsMovementStopped) return;

            _rigidbody.AddForce(_movement * _acceleration * Time.deltaTime, ForceMode2D.Impulse);

            if (_rigidbody.velocity.magnitude > _speed.Value)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * _speed.Value;
            }
        }

        IEnumerator StopMovementCoroutine(float seconds)
        {
            IsMovementStopped = true;
            yield return new WaitForSeconds(seconds);
            IsMovementStopped = false;
        }

        IEnumerator FollowPathCoroutine()
        {
            if (_path.Length < 1)
            {
                IsFollowingPath = false;
                _movement = Vector2.zero;
                yield break;
            }

            var currentWaypoint = _path[0];
            var currentPathIndex = 0;

            while (true)
            {
                if (!IsFollowingPath) yield break;
                var distanceToDestination = Vector2.Distance(currentWaypoint, transform.position);
                if (distanceToDestination < 0.3f)
                {
                    // Get next waypoint
                    currentPathIndex++;
                    if (currentPathIndex >= _path.Length)
                    {
                        // Final destination reached
                        IsFollowingPath = false;
                        _movement = Vector2.zero;
                        yield break;
                    }
                    currentWaypoint = _path[currentPathIndex];
                }
                else
                {
                    // Move to current waypoint
                    var direction = currentWaypoint - (Vector2)transform.position;
                    direction.Normalize();
                    _movement = direction;
                }
                yield return null;
            }
        }

        void FollowPath(PathResponse response)
        {
            if (response.Success && response.Waypoints != null)
            {
                _path = response.Waypoints;
                if (_followPathCoroutine != null) StopCoroutine(_followPathCoroutine);
                _followPathCoroutine = StartCoroutine(FollowPathCoroutine());
            }
            else
            {
                IsFollowingPath = false;
            }
        }

        public void SetMovementInput(Vector2 movementInput)
        {
            IsFollowingPath = false;
            _movement = movementInput;
        }

        public void StopMovement(float seconds)
        {
            if (IsMovementStopped == true) return;
            StartCoroutine(StopMovementCoroutine(seconds));
        }

        public void MoveTo(Vector2 destination)
        {
            if (_followPathCoroutine != null)
            {
                StopCoroutine(_followPathCoroutine);
                _movement = Vector2.zero;
            }

            IsFollowingPath = true;
            PathRequestManager.RequestPath(_rigidbody.position, destination, FollowPath);
        }

        void OnDrawGizmosSelected()
        {
            if (_path != null && _path.Length > 0)
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < _path.Length - 1; i++)
                {
                    Gizmos.DrawSphere(_path[i], 0.3f);
                    Gizmos.DrawLine(_path[i], _path[i + 1]);
                }
                Gizmos.DrawSphere(_path[_path.Length - 1], 0.3f);
            }
        }
    }
}
