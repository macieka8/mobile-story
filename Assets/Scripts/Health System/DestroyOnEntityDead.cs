using UnityEngine;

namespace Game
{
    public class DestroyOnEntityDead : MonoBehaviour
    {
        [SerializeField] BaseHealthEntity _healthEntity;
        [SerializeField] float _timeToDestroyInSeconds;

        void OnEnable()
        {
            _healthEntity.OnEntityDead += HandleObjectDestruction;
        }

        void OnDisable()
        {
            _healthEntity.OnEntityDead += HandleObjectDestruction;
        }

        void HandleObjectDestruction()
        {
            Destroy(_healthEntity.gameObject, _timeToDestroyInSeconds);
        }
    }
}
