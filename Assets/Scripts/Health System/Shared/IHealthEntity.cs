using System;

namespace Game
{
    public interface IHealthEntity
    {
        public float Health { get; }
        public Attribute MaxHealth { get; }

        public event Action OnHealthChanged;
        public event Action OnEntityDead;

        public void ModifyHealth(float healAmount);
    }
}
