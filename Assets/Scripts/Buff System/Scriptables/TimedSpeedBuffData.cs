using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Buffs/SpeedChange")]
    public class TimedSpeedBuffData : TimedBuffData
    {
        [SerializeField] float _bonusProcentSpeed;
        public float BonusProcentSpeed => _bonusProcentSpeed;

        public override Buff InitializeBuff(GameObject obj)
        {
            return new TimedSpeedBuff(this, obj);
        }
    }
}
