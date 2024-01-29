using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.State
{
    public class StateMachine
    {
        private BaseState _currentState;
        private BaseState _oldState;

        public BaseState CurrentState => _currentState;
        public BaseState OldState => _oldState;

        public StateMachine(BaseState initState)
        {
            _currentState = initState;
            _oldState = initState;
            
            _currentState.Enter();
        }

        public void LogicUpdate()
        {
            _currentState.LogicUpdate();
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }

        public void ChangeState(BaseState newState)
        {
            _currentState.Exit();
            _oldState = _currentState;
            _currentState = newState;
            _currentState.Enter();
        }
    }
}
