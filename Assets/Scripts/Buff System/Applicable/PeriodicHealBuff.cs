using UnityEngine;

namespace Game
{
    public class PeriodicHealBuff : PeriodicBuff
    {
        IHealthEntity _healthEntity;
        PeriodicHealBuffData PeriodicHealBuffData => BuffData as PeriodicHealBuffData;

        public PeriodicHealBuff(PeriodicBuffData buff, GameObject owner) : base(buff, owner)
        {
            _healthEntity = owner.GetComponent<IHealthEntity>();
        }

        protected override void ApplyEffect()
        {
            _healthEntity.ModifyHealth(PeriodicHealBuffData.HealAmount * _effectStacks);
        }

        public override void End()
        {
            _effectStacks = 0;
        }
    }
}
