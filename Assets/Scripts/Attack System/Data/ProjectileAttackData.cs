using System.Collections;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "AttackData/ProjectileAttack")]
    public class ProjectileAttackData : AttackData
    {
        [SerializeField] GameObject _projectilePrefab;

        public GameObject ProjectilePrefab => _projectilePrefab;

        public override void StartExecution(AttackingEntity owner)
        {
            owner.StartCoroutine(PerformAttack(owner));
        }

        public override void StopExecution(AttackingEntity owner)
        {
            owner.StopCoroutine(nameof(PerformAttack));
        }

        IEnumerator PerformAttack(AttackingEntity owner)
        {
            var combatEntity = owner.GetComponent<CombatEntity>();
            var attackDirection = owner.AttackDirection;

            owner.MovementStopper.StopMovement(CastTime);
            yield return new WaitForSeconds(CastTime);
            CreateProjectile();
            owner.StopAttack(this);

            void CreateProjectile()
            {
                var spawnedObject = Instantiate(_projectilePrefab);

                spawnedObject.transform.SetPositionAndRotation(
                    combatEntity.Position + attackDirection,
                    Quaternion.Euler(0f, 0f, Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg));

                if (spawnedObject.TryGetComponent<IProjectile>(out var projectile))
                {
                    projectile.SetDirection(attackDirection);
                    projectile.SetOwner(combatEntity);
                }
            }
        }
    }
}
