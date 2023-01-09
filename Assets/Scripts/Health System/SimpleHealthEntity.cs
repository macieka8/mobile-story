using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class SimpleHealthEntity : BaseHealthEntity, IPersistant
    {
        [Serializable]
        public struct SimpleHealthEntityData
        {
            public float health;
            public Attribute maxHealth;
        }

        [SerializeField] float _health;
        [SerializeField] Attribute _maxHealth;

        public override float Health => _health;
        public override Attribute MaxHealth => _maxHealth;

        public override void ModifyHealth(float healAmount)
        {
            var prevHealth = _health;
            _health = Mathf.Clamp(_health + healAmount, 0, MaxHealth.Value);
            if (prevHealth != _health)
            {
                InvokeOnHealthChanged();
                if (_health == 0f) InvokeOnEntityDead();
            }
        }

        public object Save()
        {
            return new SimpleHealthEntityData()
            {
                health = _health,
                maxHealth = _maxHealth
            };
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            var healthData = dataHandler.ToObject<SimpleHealthEntityData>(data);
            _maxHealth = healthData.maxHealth;
            _health = healthData.health;
        }
    }
}
