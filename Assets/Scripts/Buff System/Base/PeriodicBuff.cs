using System;
using UnityEngine;

namespace Game
{
    public abstract class PeriodicBuffData : BuffData
    {
        [SerializeField] bool _isPermanent;
        [SerializeField] int _numberOfTicks;
        [SerializeField] float _tickCooldownInSeconds;
        [SerializeField] bool _isTickStackable;
        [SerializeField] bool _isEffectStackable;

        public bool IsPermanent { get => _isPermanent; }
        public int NumberOfTicks { get => _numberOfTicks; }
        public float TickCooldownInSeconds { get => _tickCooldownInSeconds; }
        public bool IsTickStackable { get => _isTickStackable; }
        public bool IsEffectStackable { get => _isEffectStackable; }
    }

    public abstract class PeriodicBuff : Buff
    {
        protected int _ticksLeft = 0;
        protected float _timeToNextTick;
        protected int _effectStacks = 0;

        protected PeriodicBuffData PeriodicBuffData => BuffData as PeriodicBuffData;

        public PeriodicBuff(PeriodicBuffData buffData, GameObject owner) : base(buffData, owner)
        {
        }

        public override void Tick(float deltaTime)
        {
            _timeToNextTick -= deltaTime;
            // If next tick ready
            if (_timeToNextTick <= 0)
            {
                // Reset tick cooldown and decrease tick count
                _timeToNextTick = PeriodicBuffData.TickCooldownInSeconds;
                if (!PeriodicBuffData.IsPermanent)
                {
                    _ticksLeft--;
                }

                ApplyEffect();
            }

            // If buff is ending
            if (_ticksLeft <= 0)
            {
                End();

                // Handle OnEnd event
                InvokeOnEnd();

                IsFinished = true;
            }
        }

        public override void Activate()
        {
            // Check if buff effect is stackable / First Activation
            if (PeriodicBuffData.IsEffectStackable || _ticksLeft <= 0)
            {
                _effectStacks++;
            }

            // Check if buff ticks are stackable / First Activation
            if (PeriodicBuffData.IsTickStackable || _ticksLeft <= 0)
            {
                _ticksLeft += PeriodicBuffData.NumberOfTicks;
            }

            // Set/Reset tick duration
            _timeToNextTick = PeriodicBuffData.TickCooldownInSeconds;
        }
        protected override abstract void ApplyEffect();
        public override abstract void End();
    }

    public class PeriodicBuffEventArgs : EventArgs
    {
        public int ticks;

        public PeriodicBuffEventArgs(int ticks) => this.ticks = ticks;
    }
}
