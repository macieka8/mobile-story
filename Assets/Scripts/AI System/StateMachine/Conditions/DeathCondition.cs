using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DeathCondition : StateTransitionCondition
    {
        IHealthEntity _healthEntity;

        void Awake()
        {
            _healthEntity = GetComponentInParent<IHealthEntity>();
        }

        public override bool IsMet()
        {
            return _healthEntity.Health == 0;
        }
    }
}
