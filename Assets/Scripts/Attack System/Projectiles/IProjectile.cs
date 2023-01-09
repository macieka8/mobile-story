using UnityEngine;

namespace Game
{
    public interface IProjectile
    {
        public void SetDirection(Vector2 direction);
        public void SetOwner(CombatEntity owner);
    }
}
