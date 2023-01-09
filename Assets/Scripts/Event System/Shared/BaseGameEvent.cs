using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class BaseGameEvent<T> : ScriptableObject
    {
        List<IGameEventListener<T>> _eventListeners = new List<IGameEventListener<T>>();

        public void RegisterListener(IGameEventListener<T> listener)
        {
            if (!_eventListeners.Contains(listener)) _eventListeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener<T> listener)
        {
            _eventListeners.Remove(listener);
        }

        public void RaiseEvent(T value)
        {
            foreach (var listener in _eventListeners)
            {
                listener.OnEventRaised(value);
            }
        }
    }
}
