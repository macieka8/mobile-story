using UnityEngine;

namespace Game
{
    public class CombatEntity : MonoBehaviour
    {
        [SerializeField] CombatEntityIdentifier _entityIdentifier;
        [SerializeField] LayerMask _structureLayer;

        Transform _transform;
        IHealthEntity _healthEntity;

        CombatEntity _lastAttacker;

        public CombatFraction Fraction => _entityIdentifier.Fraction;
        public CombatEntityIdentifier Identifier => _entityIdentifier;
        public IHealthEntity HealthEntity => _healthEntity;
        public Transform Transform => _transform;
        public Vector2 Position => _transform.position;

        public CombatEntity LastAttacker { get => _lastAttacker; set => _lastAttacker = value; }

        void Awake()
        {
            _transform = GetComponent<Transform>();
            _healthEntity = GetComponent<IHealthEntity>();
        }

        void OnEnable()
        {
            _healthEntity.OnEntityDead += HandleEntityDead;
        }

        void OnDisable()
        {
            _healthEntity.OnEntityDead -= HandleEntityDead;
        }

        void HandleEntityDead()
        {
            CombatSystem.Instance.InvokeOnEntityKilled(this);
        }

        public bool IsVisibleBy(CombatEntity combatEntity)
        {
            if (_healthEntity.Health <= 0) return false;
            return IsVisibleBy(combatEntity.Position);
        }

        public bool IsVisibleBy(Vector2 position)
        {
            if (!gameObject.activeInHierarchy) return false;
            var hit = Physics2D.Linecast(position, Position, _structureLayer);
            return !hit;
        }
    }
}
