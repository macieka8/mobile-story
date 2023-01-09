using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class GroupCombatTargetingHelper : MonoBehaviour, ITargetingHelper
    {
        static System.Action<int, CombatEntity> OnGroupFoundTarget;

        [SerializeField] int _groupNumber;

        [SerializeField] float _loseTargetDistance;

        [Header("Scan Options")]
        [SerializeField] float _targetDetectionRange;
        [SerializeField] float _timeBetweenScansInSeconds;
        [SerializeField] ContactFilter2D _scanFilter;

        CombatEntity _combatEntity;
        WaitForSeconds _waitForNextScan;
        CombatEntity _target;

        public CombatEntity Target => _target;

        void Awake()
        {
            _combatEntity = GetComponent<CombatEntity>();
            _waitForNextScan = new WaitForSeconds(_timeBetweenScansInSeconds);
        }

        void Start()
        {
            StartCoroutine(ScanCoroutine());
        }

        void OnEnable() => OnGroupFoundTarget += HandleTargetFound;

        void OnDisable() => OnGroupFoundTarget -= HandleTargetFound;

        void Update()
        {
            if (_target == null) return;
            if (!_target.IsVisibleBy(_combatEntity))
            {
                var distanceToTarget = Vector2.Distance(_target.Position, _combatEntity.Position);
                if (distanceToTarget > _loseTargetDistance)
                {
                    _target = null;
                }
            }
        }

        void HandleTargetFound(int groupId, CombatEntity foundTarget)
        {
            if (groupId != _groupNumber) return;
            if (_target == null)
            {
                _target = foundTarget;
            }
        }

        IEnumerator ScanCoroutine()
        {
            while (true)
            {
                List<Collider2D> colliders = new List<Collider2D>();
                Physics2D.OverlapCircle(_combatEntity.Position, _targetDetectionRange, _scanFilter, colliders);
                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent<CombatEntity>(out var foundCombatEntity) &&
                        !_combatEntity.Fraction.IsAllied(foundCombatEntity.Fraction) &&
                        foundCombatEntity.IsVisibleBy(_combatEntity))
                    {
                        OnTargetDetected(foundCombatEntity);
                    }
                }
                yield return _waitForNextScan;
            }
        }

        void OnTargetDetected(CombatEntity foundTarget)
        {
            OnGroupFoundTarget?.Invoke(_groupNumber, foundTarget);
            if (_target == null)
            {
                _target = foundTarget;
            }
            else
            {
                var distanceToCurrent = (_target.Position - _combatEntity.Position).sqrMagnitude;
                var distanceToFound = (foundTarget.Position - _combatEntity.Position).sqrMagnitude;
                if (distanceToFound < distanceToCurrent)
                {
                    _target = foundTarget;
                }
            }
        }
    }
}
