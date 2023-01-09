using UnityEngine;

namespace Game
{
    public class AttackState : State
    {
        [SerializeField] AttackData _attack;
        AttackingEntity _attackingEntity;
        ITargetingHelper _targetingHelper;

        void Awake()
        {
            _attackingEntity = GetComponentInParent<AttackingEntity>();
            _targetingHelper = GetComponentInParent<ITargetingHelper>();
        }

        void Update()
        {
            if (_targetingHelper.Target == null) return;
            _attackingEntity.AttackDirection =
                (_targetingHelper.Target.Position - (Vector2)transform.position).normalized;
            _attackingEntity.PerformAttack(_attack);
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
