using UnityEngine;
using System.Collections.Generic;
using System;

namespace Game
{
    public class AttackingEntity : MonoBehaviour
    {
        class FloatCooldown
        {
            public float Value;
            public FloatCooldown(float value) => Value = value;
        }

        IMovementStopper _movementStopper;
        IHealthEntity _healthEntity;

        Dictionary<AttackData, FloatCooldown> _attackCooldowns = new Dictionary<AttackData, FloatCooldown>();
        AttackData _currentAttack;

        Vector2 _attackDirection = Vector2.up;

        public event Action<AttackData> OnAttackStart;

        public Vector2 AttackDirection { get => _attackDirection; set => _attackDirection = value.normalized; }
        public IMovementStopper MovementStopper => _movementStopper;

        void Awake()
        {
            _movementStopper = GetComponent<IMovementStopper>();
            _healthEntity = GetComponent<IHealthEntity>();
        }

        void OnEnable() => _healthEntity.OnEntityDead += HandleEntityDead;
        void OnDisable() => _healthEntity.OnEntityDead -= HandleEntityDead;

        void Update()
        {
            foreach (var entry in _attackCooldowns)
            {
                if (entry.Value.Value > 0f)
                    _attackCooldowns[entry.Key].Value -= Time.deltaTime;
            }
        }

        void HandleEntityDead()
        {
            _currentAttack?.StopExecution(this);
        }

        public bool PerformAttack(AttackData attack)
        {
            if (attack == null) return false;
            if (_currentAttack != null) return false;
            if (_attackCooldowns.TryGetValue(attack, out var cooldown))
            {
                if (cooldown.Value > 0f) return false;
                _attackCooldowns[attack].Value = attack.CooldownInSeconds;
            }
            else
            {
                _attackCooldowns.Add(attack, new FloatCooldown(attack.CooldownInSeconds));
            }

            _currentAttack = attack;
            attack.StartExecution(this);
            OnAttackStart?.Invoke(attack);
            return true;
        }

        public void StopAttack(AttackData attack)
        {
            if (_currentAttack != attack) return;
            _currentAttack = null;
            attack.StopExecution(this);
        }

        public bool IsAttackOnCooldown(AttackData attackData)
        {
            if (_attackCooldowns.TryGetValue(attackData, out var cooldown))
            {
                return cooldown.Value > 0f;
            }
            return false;
        }
    }
}
