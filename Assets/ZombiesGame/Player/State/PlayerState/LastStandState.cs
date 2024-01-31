using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Input;
using Zombies.State;

namespace Zombies.Player.State
{
    public class LastStandState : BasePlayerState
    {
        public LastStandState(Animator anim, string animName, PlayerStateInfo stateInfo, PlayerStateEvent stateEventSO, InputSO inputSO) : base(anim, animName, stateInfo, stateEventSO, inputSO)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _stateEvent.MoveEvent?.Invoke(Vector2.zero, 0f);
            _stateEvent.LastStandEvent?.Invoke();
        }

        public override void Exit()
        {
            base.Exit();
            
            _stateEvent.GamePlayEvent?.Invoke();
        }

        public override void LogicUpdate()
        {
            if(_startTime + _stateInfo.LastStandTime < Time.time)
            {
                _endState = true;
            }
        }
    }
}
