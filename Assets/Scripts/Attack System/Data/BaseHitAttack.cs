using UnityEngine;

namespace Game
{
    public abstract class BaseHitAttack : MonoBehaviour
    {
        public abstract void Setup(AttackingEntity owner, HitAttackData data);
    }
}
