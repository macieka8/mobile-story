using UnityEngine;

namespace Game
{
    public class AttackOnCooldownCondition : StateTransitionCondition
    {
        [SerializeField] AttackData _attack;
        AttackingEntity _attackingEntity;

        void Awake()
        {
            _attackingEntity = GetComponentInParent<AttackingEntity>();
        }

        public override bool IsMet()
        {
            return _attackingEntity.IsAttackOnCooldown(_attack);
        }
    }
}
