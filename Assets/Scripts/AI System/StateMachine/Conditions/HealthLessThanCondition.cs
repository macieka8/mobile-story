using UnityEngine;

namespace Game
{
    public class HealthLessThanCondition : StateTransitionCondition
    {
        [Range(0, 1)]
        [SerializeField] float _healthFraction;

        IHealthEntity _healthEntity;

        void Awake()
        {
            _healthEntity = GetComponentInParent<IHealthEntity>();
        }

        public override bool IsMet()
        {
            return _healthEntity.Health / _healthEntity.MaxHealth.Value <= _healthFraction;
        }
    }
}
