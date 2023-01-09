using System.Collections;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "AttackData/HitAttack")]
    public class HitAttackData : AttackData
    {
        [SerializeField] float _damage;
        [SerializeField] float _attackDiameter;
        [Min(0f)] [SerializeField] float _distanceFromCaster;
        [SerializeField] ContactFilter2D _attackFilter;
        [SerializeField] BaseHitAttack _hitAttackPrefab;

        public BaseHitAttack HitAttack => _hitAttackPrefab;
        public float Damage => _damage;
        public float AttackDiameter => _attackDiameter;
        public float DistanceFromCaster => _distanceFromCaster;
        public ContactFilter2D AttackFilter => _attackFilter;

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
            InitAttack();
            owner.MovementStopper.StopMovement(CastTime);
            yield return new WaitForSeconds(CastTime);
            owner.StopAttack(this);

            void InitAttack()
            {
                var hitObject = Instantiate(HitAttack);
                hitObject.Setup(owner, this);
            }
        }
    }
}
