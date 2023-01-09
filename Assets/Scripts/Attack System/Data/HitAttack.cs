using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class HitAttack : BaseHitAttack
    {
        [SerializeField] Animator _animator;
        List<Collider2D> _alreadyHittedTargets = new List<Collider2D>();

        HitAttackData _data;
        AttackingEntity _owner;
        CombatEntity _combatEntity;
        Vector2 _attackDirection;

        Collider2D _collider;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        IEnumerator PerformAttackCoroutine()
        {
            yield return new WaitForSeconds(_data.CastTime);
            FindTarget();
            Destroy(gameObject);
        }

        void FindTarget()
        {
            var foundColliders = new List<Collider2D>();
            _collider.OverlapCollider(_data.AttackFilter, foundColliders);
            foreach (var foundCollider in foundColliders)
            {
                if (foundCollider.gameObject == _owner.gameObject) continue;
                if (_alreadyHittedTargets.Contains(foundCollider)) continue;
                if (foundCollider.TryGetComponent<CombatEntity>(out var target))
                {
                    if (_combatEntity.Fraction.IsAllied(target.Fraction)) continue;

                    // Damage found target
                    target.HealthEntity.ModifyHealth(-_data.Damage);
                    CombatSystem.Instance.InvokeOnEntityAttacked(target, _combatEntity);
                    _alreadyHittedTargets.Add(foundCollider);
                }
            }
        }

        public override void Setup(AttackingEntity owner, HitAttackData data)
        {
            _owner = owner;
            _data = data;
            _combatEntity = _owner.GetComponent<CombatEntity>();
            _attackDirection = owner.AttackDirection;

            transform.SetPositionAndRotation(
                owner.transform.position + (Vector3)_attackDirection * _data.DistanceFromCaster,
                Quaternion.Euler(0f, 0f, Mathf.Atan2(_attackDirection.y, _attackDirection.x) * Mathf.Rad2Deg));

            _animator.speed = 1f / _data.CastTime;
            transform.localScale = new Vector3(_data.AttackDiameter, _data.AttackDiameter);
            _animator.Play("Attack");

            StartCoroutine(PerformAttackCoroutine());
        }
    }
}
