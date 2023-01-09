using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public abstract class State : MonoBehaviour, IState
    {
        [SerializeField] int _priority;
        [SerializeField] List<StateTransition> _transitions = new List<StateTransition>();

        public int Priority => _priority;

        public virtual void Enter() => gameObject.SetActive(true);
        public virtual void Exit() => gameObject.SetActive(false);

        public IState ProcessTransitions()
        {
            foreach (var transition in _transitions)
            {
                if (transition.ShouldTransition()) return transition.NextState;
            }
            return null;
        }
    }
}
