using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StoryVariableChangeGameEventListener : MonoBehaviour, IGameEventListener<Void>
    {
        [Tooltip("GameEvent to Listen to.")]
        [SerializeField] VoidGameEvent _gameEvent;
        [SerializeField] GameStory _story;
        [SerializeReference] List<StoryVariableChange> _variableChanges = new List<StoryVariableChange>();

        void OnEnable()
        {
            _gameEvent.RegisterListener(this);
        }

        void OnDisable()
        {
            _gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised() => OnEventRaised(new Void { });
        public void OnEventRaised(Void value)
        {
            foreach (var setter in _variableChanges)
            {
                setter.ApplyChange(_story);
            }
        }

        public void AddVariableChange(StoryVariableChange variableChange)
        {
            _variableChanges.Add(variableChange);
        }
    }
}
