using UnityEngine;

namespace Game
{
    public class TimedSpeedBuff : TimedBuff
    {
        CharacterMovement _characterMovement;
        AttributeModifier _modifier;

        TimedSpeedBuffData TimedSpeedBuffData => BuffData as TimedSpeedBuffData;

        public TimedSpeedBuff(TimedBuffData buff, GameObject owner) : base(buff, owner)
        {
            _characterMovement = owner.GetComponent<CharacterMovement>();
            _modifier = new AttributeModifier(TimedSpeedBuffData.BonusProcentSpeed, AttributeModiferType.ProcentAdd, this);
        }

        public override void End()
        {
            _characterMovement.Speed.RemoveModifier(_modifier);
        }

        protected override void ApplyEffect()
        {
            _characterMovement.Speed.AddModifier(_modifier);
        }
    }
}
