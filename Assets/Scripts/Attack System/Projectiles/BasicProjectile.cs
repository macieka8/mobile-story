using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Game
{
    public class BasicProjectile : MonoBehaviour, IProjectile
    {
        [SerializeField] Rigidbody2D _rigidbody;
        [SerializeField] Collider2D _collider;
        [SerializeField] LayerMask _structureLayer;
        [SerializeField] float _velocity;
        [SerializeField] float _damage;

        CombatEntity _owner;

        public void SetDirection(Vector2 direction)
        {
            _rigidbody.velocity = direction * _velocity;
        }

        public void SetOwner(CombatEntity owner)
        {
            _owner = owner;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<CombatEntity>(out var foundCombatEntity))
            {
                if (_owner == null || !_owner.Fraction.IsAllied(foundCombatEntity.Fraction))
                {
                    foundCombatEntity.HealthEntity.ModifyHealth(-_damage);
                    CombatSystem.Instance.InvokeOnEntityAttacked(foundCombatEntity, _owner);
                }
            }
            else if (_collider.IsTouchingLayers(_structureLayer))
            {
                Destroy(gameObject);
            }
        }
    }
}
