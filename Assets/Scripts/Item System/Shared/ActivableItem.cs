using System;
using UnityEngine;

namespace Game
{
    public abstract class ActivableItem : Item, IActivable
    {
        [SerializeField] float _cooldownInSeconds;
        public float CooldownInSeconds => _cooldownInSeconds;

        public abstract void Activate(GameObject target);

        public event Action<IActivable> OnActivableRemove;
        public event Action OnActivate;
        protected void InvokeOnActivableRemove() => OnActivableRemove?.Invoke(this);
        protected void InvokeOnActivate() => OnActivate?.Invoke();
    }
}