using System;
using UnityEngine;

namespace Game
{
    public abstract class AttackData : ScriptableObject, IActivable
    {
        [SerializeField] Sprite _icon;
        [TextArea]
        [SerializeField] string _description;
        [SerializeField] float _cooldownInSeconds;
        [SerializeField] float _castTime;
        [SerializeField] string _animationName;

        public event Action<IActivable> OnActivableRemove;
        public event Action OnActivate;

        public Sprite Icon => _icon;
        public string Description => _description;
        public float CooldownInSeconds => _cooldownInSeconds;
        public float CastTime => _castTime;
        public string AnimationName => _animationName;

        public abstract void StartExecution(AttackingEntity owner);
        public abstract void StopExecution(AttackingEntity owner);

        public void Activate(GameObject target)
        {
            if (target.TryGetComponent<AttackingEntity>(out var attackingEntity))
            {
                if (attackingEntity.PerformAttack(this))
                {
                    OnActivate?.Invoke();
                }
            }
        }

    }
}
