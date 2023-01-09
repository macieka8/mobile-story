using UnityEngine;
using System;

namespace Game
{
    public abstract class BaseHealthEntity : MonoBehaviour, IHealthEntity
    {
        public abstract float Health { get; }

        public abstract Attribute MaxHealth { get; }

        public event Action OnHealthChanged;
        public event Action OnEntityDead;

        protected void InvokeOnHealthChanged() => OnHealthChanged?.Invoke();
        protected void InvokeOnEntityDead() => OnEntityDead?.Invoke();

        public abstract void ModifyHealth(float healAmount);
    }
}
