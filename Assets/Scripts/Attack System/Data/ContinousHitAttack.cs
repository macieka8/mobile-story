using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ContinousHitAttack : BaseHitAttack
    {
        [SerializeField] Animator _animator;
        List<Collider2D> _alreadyHittedTargets = new List<Collider2D>();

        HitAttackData _data;
        AttackingEntity _owner;
        CombatEntity _combatEntity;
        Vector2 _attackDirection;

        Animator _casterAnimator;

        IEnumerator PerformAttackCoroutine()
        {
            _casterAnimator.speed = _data.CastTime;
            yield return new WaitForSeconds(_data.CastTime);
            _casterAnimator.speed = 1f;
            Destroy(gameObject);
        }
        void OnTriggerEnter2D(Collider2D foundCollider)
        {
            if (foundCollider.gameObject == _owner.gameObject) return;
            if (_alreadyHittedTargets.Contains(foundCollider)) return;
            if (foundCollider.TryGetComponent<CombatEntity>(out var target))
            {
                if (_combatEntity.Fraction.IsAllied(target.Fraction)) return;

                // Damage found target
                target.HealthEntity.ModifyHealth(-_data.Damage);
                CombatSystem.Instance.InvokeOnEntityAttacked(target, _combatEntity);
                _alreadyHittedTargets.Add(foundCollider);
            }
        }

        public override void Setup(AttackingEntity owner, HitAttackData data)
        {
            _owner = owner;
            _data = data;
            _combatEntity = _owner.GetComponent<CombatEntity>();
            _casterAnimator = _owner.GetComponent<Animator>();
            _attackDirection = owner.AttackDirection;

            transform.SetPositionAndRotation(
                owner.transform.position + (Vector3)_attackDirection * _data.DistanceFromCaster,
                Quaternion.Euler(0f, 0f, Mathf.Atan2(_attackDirection.y, _attackDirection.x) * Mathf.Rad2Deg));
            transform.SetParent(owner.transform);

            _animator.speed = 1f / _data.CastTime;
            transform.localScale = new Vector3(_data.AttackDiameter, _data.AttackDiameter);
            _animator.Play("Attack");

            StartCoroutine(PerformAttackCoroutine());
        }
    }
}
