using UnityEngine;

namespace Game
{
    public abstract class GoalData : ScriptableObject
    {
        [SerializeField] string _descripton;
        [SerializeField] VoidGameEvent[] _onCompletedEvents;

        public string Description => _descripton;
        public VoidGameEvent[] OnCompletedEvents => _onCompletedEvents;

        public abstract Goal InitializeGoal();
    }
}
