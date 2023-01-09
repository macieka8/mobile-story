using UnityEngine;

namespace Game
{
    public class InvulnerableHealthEntity : BaseHealthEntity
    {
        [SerializeField] float _health;
        [SerializeField] Attribute _maxHealth;

        public override float Health => _health;
        public override Attribute MaxHealth => _maxHealth;

        public override void ModifyHealth(float healAmount)
        {
        }
    }
}
