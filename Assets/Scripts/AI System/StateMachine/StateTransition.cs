using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [System.Serializable]
    public class ConditionEntry
    {
        public StateTransitionCondition Condition;
        public bool IsNegated;
    }


    [System.Serializable]
    public class StateTransition
    {
        [SerializeField] List<ConditionEntry> _conditions;
        [SerializeField] State _nextState;

        public State NextState => _nextState;

        public bool ShouldTransition()
        {
            foreach (var entry in _conditions)
            {
                var isMet = entry.IsNegated ? !entry.Condition.IsMet() : entry.Condition.IsMet();
                if (!isMet) return false;
            }
            return true;
        }
    }
}
