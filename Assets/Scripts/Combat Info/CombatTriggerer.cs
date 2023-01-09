using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CombatTriggerer : MonoBehaviour
    {
        [SerializeField] float _maxEnemyDistance;
        [SerializeField] ContactFilter2D _scanFilter;
        [SerializeField] float _timeBetweenScansInSeconds;

        CombatEntity _combatEntity;
        IHealthEntity _healthEntity;
        WaitForSeconds _waitForNextScan;

        float _prevHealth;

        void Awake()
        {
            _combatEntity = GetComponent<CombatEntity>();
            _healthEntity = GetComponent<IHealthEntity>();
            _waitForNextScan = new WaitForSeconds(_timeBetweenScansInSeconds);
        }

        void Start()
        {
            StartCoroutine(ScanCoroutine());
            _prevHealth = _healthEntity.Health;
        }

        void OnEnable()
        {
            CombatSystem.Instance.OnEntityAttacked += HandleEntityAttacked;
            _healthEntity.OnHealthChanged += HandleHealthChanged;
        }

        void OnDisable()
        {
            CombatSystem.Instance.OnEntityAttacked -= HandleEntityAttacked;
            _healthEntity.OnHealthChanged -= HandleHealthChanged;
        }

        void HandleHealthChanged()
        {
            if (_healthEntity.Health < _prevHealth)
                CombatSystem.Instance.TriggerCombat();

            _prevHealth = _healthEntity.Health;
        }

        void HandleEntityAttacked(CombatEntity attacked, CombatEntity attacking)
        {
            if (attacked == _combatEntity || attacking == _combatEntity)
            {
                CombatSystem.Instance.TriggerCombat();
            }
        }

        IEnumerator ScanCoroutine()
        {
            while (true)
            {
                List<Collider2D> colliders = new List<Collider2D>();
                Physics2D.OverlapCircle(_combatEntity.Position, _maxEnemyDistance, _scanFilter, colliders);
                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent<CombatEntity>(out var foundCombatEntity) &&
                        !_combatEntity.Fraction.IsAllied(foundCombatEntity.Fraction) &&
                        foundCombatEntity.IsVisibleBy(_combatEntity))
                    {
                        CombatSystem.Instance.TriggerCombat();
                    }
                }
                yield return _waitForNextScan;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _maxEnemyDistance);
        }
    }
}
