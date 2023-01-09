using UnityEngine;

namespace Game
{
    public class ThrowablePotionProjectile : MonoBehaviour, IProjectile
    {
        [SerializeField] LayerMask _structureLayer;
        [SerializeField] Collider2D _collider;

        [SerializeField] GameObject _effectZone;
        [SerializeField] float _distance;
        [SerializeField] float _throwDurationInSeconds;
        [SerializeField] float _slerpOffset;

        CombatEntity _owner;

        Vector2 _startPosition;
        Vector2 _endPosition;
        Vector2 _centerPoint;
        Vector2 _relativeStart;
        Vector2 _relativeEnd;

        float _currentTime;

        void Update()
        {
            _currentTime += Time.deltaTime;
            float progress = _currentTime / _throwDurationInSeconds;

            transform.position = (Vector3)_centerPoint + Vector3.Slerp(_relativeStart, _relativeEnd, progress);
            if (progress >= 1.0f)
            {
                BreakPotion();
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (_collider.IsTouchingLayers(_structureLayer))
            {
                BreakPotion();
            }
        }

        void BreakPotion()
        {
            Instantiate(_effectZone, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public void SetDirection(Vector2 direction)
        {
            _startPosition = transform.position;
            _endPosition = _startPosition + direction * _distance;

            _centerPoint = (_startPosition + _endPosition) / 2f;
            if (direction.x < 0)
                _centerPoint -= -Vector2.Perpendicular(direction) * _slerpOffset;
            else
                _centerPoint -= Vector2.Perpendicular(direction) * _slerpOffset;

            _relativeStart = _startPosition - _centerPoint;
            _relativeEnd = _endPosition - _centerPoint;
        }

        public void SetOwner(CombatEntity owner)
        {
            _owner = owner;
        }
    }
}
