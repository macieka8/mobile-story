using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    [CreateAssetMenu(menuName = "AttackData/DashAttack")]
    public class DashAttackData : AttackData
    {
        [SerializeField] float _travelDistance;
        [SerializeField] float _travelTimeInSeconds;
        [SerializeField] float _damage;
        [SerializeField] ParticleSystem _dashParticlesPrefab;
        [SerializeField] ContactFilter2D _attackFilter;

        Dictionary<AttackingEntity, Coroutine> _performedAttacks = new Dictionary<AttackingEntity, Coroutine>();

        public float TravelDistance => _travelDistance;
        public float TravelTimeInSeconds => _travelTimeInSeconds;
        public float Damage => _damage;
        public ParticleSystem DashParticlesPrefab => _dashParticlesPrefab;

        public override void StartExecution(AttackingEntity owner)
        {
            var attackCoroutine = owner.StartCoroutine(PerformDash(owner));
            if (_performedAttacks.ContainsKey(owner))
                _performedAttacks[owner] = attackCoroutine;
            else
                _performedAttacks.Add(owner, attackCoroutine);
        }

        public override void StopExecution(AttackingEntity owner)
        {
            if (_performedAttacks.TryGetValue(owner, out var attackCoroutine))
            {
                _performedAttacks.Remove(owner);
                owner.StopCoroutine(attackCoroutine);
            }
            else
            {
                Debug.LogError("Could not find attack Coroutine");
            }
        }

        IEnumerator PerformDash(AttackingEntity owner)
        {
            var rigidbody = owner.GetComponent<Rigidbody2D>();
            var collider = owner.GetComponent<Collider2D>();
            var combatEntity = owner.GetComponent<CombatEntity>();
            var direction = owner.AttackDirection;

            var alreadyDamaged = new List<CombatEntity>();

            var dashParticles = Instantiate(_dashParticlesPrefab, rigidbody.position, Quaternion.identity, owner.transform);
            dashParticles.Play();
            Destroy(dashParticles.gameObject, dashParticles.main.duration + dashParticles.main.startLifetime.constant);
            owner.MovementStopper.StopMovement(CastTime + _travelTimeInSeconds);

            yield return new WaitForSeconds(CastTime);
            yield return DashCoroutine();

            owner.StopAttack(this);

            IEnumerator DashCoroutine()
            {
                var speed = _travelDistance / _travelTimeInSeconds;
                var velocity = speed * direction;
                
                var travelTimeLeft = _travelTimeInSeconds;
                while (travelTimeLeft > 0f)
                {
                    travelTimeLeft -= Time.deltaTime;
                    rigidbody.velocity = velocity;
                    //rigidbody.MovePosition(rigidbody.position + movementDelta);
                    CheckCollider();
                    yield return null;
                }
            }

            void CheckCollider()
            {
                var colliders = new List<Collider2D>();
                collider.OverlapCollider(_attackFilter, colliders);

                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent<CombatEntity>(out var target))
                    {
                        if (alreadyDamaged.Contains(target)) continue;
                        if (combatEntity.Fraction.IsAllied(target.Fraction)) continue;
                     
                        // Damage found target
                        target.HealthEntity.ModifyHealth(-_damage);
                        alreadyDamaged.Add(target);
                        CombatSystem.Instance.InvokeOnEntityAttacked(target, combatEntity);
                    }
                }
            }
        }
    }
}
