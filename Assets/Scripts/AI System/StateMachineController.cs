using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game
{
    public class StateMachineController : MonoBehaviour
    {
        [SerializeField] State _initalState;
        [SerializeField] List<StateTransition> _globalTransitions = new List<StateTransition>();

        StateMachine _stateMachine;
        public StateMachine StateMachine => _stateMachine;

        void Awake()
        {
            _stateMachine = new StateMachine(gameObject, _initalState);
        }

        void Update()
        {
            foreach (var transition in _globalTransitions)
            {
                if (transition.ShouldTransition()
                    && transition.NextState.Priority > _stateMachine.CurrentState.Priority)
                {
                    _stateMachine.ChangeState(transition.NextState);
                }
            }
            _stateMachine.Tick();
        }

        public void ChangeState(State newState)
        {
            _stateMachine.ChangeState(newState);
        }
    }
}
