using UnityEngine;

namespace Game
{
    public class OnEntityDeathRaiseEvent : MonoBehaviour
    {
        [SerializeField] VoidGameEvent _eventToRaise;
        IHealthEntity _healthEntity;

        void Awake()
        {
            _healthEntity = GetComponent<IHealthEntity>();
        }

        void OnEnable()
        {
            _healthEntity.OnEntityDead += HandleEntityDead;
        }

        void OnDisable()
        {
            _healthEntity.OnEntityDead += HandleEntityDead;
        }

        void HandleEntityDead()
        {
            _eventToRaise.RaiseEvent();
        }
    }
}
