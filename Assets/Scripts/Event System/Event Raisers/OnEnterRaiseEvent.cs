using UnityEngine;

namespace Game
{
    public class OnEnterRaiseEvent : MonoBehaviour, IPersistant
    {
        struct PersistantData
        {
            public bool IsEnabled;
        }

        [SerializeField] VoidGameEvent _gameEvent;
        [SerializeField] CombatEntityIdentifier _whoCanTriggerEvent;
        [SerializeField] bool _isEnabled;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isEnabled) return;

            if (collision.TryGetComponent<CombatEntity>(out var foundCombatEntity)
                && foundCombatEntity.Identifier == _whoCanTriggerEvent)
            {
                _isEnabled = false;
                _gameEvent.RaiseEvent();
            }
        }

        public void SetActive(bool enabled)
        {
            _isEnabled = enabled;
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            var dataObj = dataHandler.ToObject<PersistantData>(data);
            _isEnabled = dataObj.IsEnabled;
        }

        public object Save()
        {
            return new PersistantData { IsEnabled = _isEnabled };
        }
    }
}
