using UnityEngine;

namespace Game
{
    public class OnEntityKilledRaiseEvent : MonoBehaviour, IPersistant
    {
        struct PersistantData
        {
            public int KillCount;
        }

        [SerializeField] VoidGameEvent _gameEvent;
        [SerializeField] CombatEntityIdentifier _entityToKill;
        [SerializeField] int _numberOfEntitiesToKill;

        int _currentlyKilled = 0;

        void Start()
        {
            _currentlyKilled = CombatSystem.Instance.GetKillCount(_entityToKill);
            CombatSystem.Instance.OnEntityKilled += HandleEntityKilled;
        }

        void HandleEntityKilled(CombatEntity killedEntity)
        {
            if (killedEntity.Identifier == _entityToKill)
            {
                _currentlyKilled++;
                if (_currentlyKilled == _numberOfEntitiesToKill)
                {
                    _gameEvent.RaiseEvent();
                    CombatSystem.Instance.OnEntityKilled -= HandleEntityKilled;
                }
            }
        }

        public object Save()
        {
            return new PersistantData
            {
                KillCount = _currentlyKilled
            };
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            var dataObj = dataHandler.ToObject<PersistantData>(data);
            _currentlyKilled = dataObj.KillCount;
        }
    }
}
