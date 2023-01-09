using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Buffs/PeriodicHeal")]
    public class PeriodicHealBuffData : PeriodicBuffData
    {
        [SerializeField] float _healAmount;
        public float HealAmount => _healAmount;

        public override Buff InitializeBuff(GameObject obj)
        {
            return new PeriodicHealBuff(this, obj);
        }
    }
}
