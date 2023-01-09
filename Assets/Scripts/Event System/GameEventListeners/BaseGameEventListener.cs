using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour, IGameEventListener<T>
        where E : BaseGameEvent<T> where UER : UnityEvent<T>
    {
        [Tooltip("GameEvent to listen to.")]
        [SerializeField] E _gameEvent;
        [Tooltip("Invoked when GameEvent is raised.")]
        [SerializeField] UER _responseEvent;

        void OnEnable()
        {
            _gameEvent.RegisterListener(this);
        }

        void OnDisable()
        {
            _gameEvent.UnregisterListener(this);
        }

        public virtual void OnEventRaised(T value)
        {
            _responseEvent?.Invoke(value);
        }
    }
}
