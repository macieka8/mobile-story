using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StateMachine
    {
        GameObject _owner;
        IState _currentState;

        public GameObject Owner => _owner;
        public IState CurrentState => _currentState;

        public StateMachine(GameObject owner, IState InitalState)
        {
            _owner = owner;
            _currentState = InitalState;
            _currentState.Enter();
        }

        public void ChangeState(IState newState)
        {
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Tick()
        {
            var nextState = _currentState.ProcessTransitions();
            if (nextState == null) return;
            ChangeState(nextState);
        }
    }
}
