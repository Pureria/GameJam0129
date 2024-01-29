using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Input;
using Zombies.State;

namespace Zombies.Player.State
{
    public class IdleState : BasePlayerState
    {
        public IdleState(Animator anim, string animName, PlayerStateInfo stateInfo, PlayerStateEvent stateEventSO, InputSO inputSO) : base(anim, animName, stateInfo, stateEventSO, inputSO)
        {
        }

        public override void Enter()
        {
            _stateEvent.MoveEvent?.Invoke(Vector2.zero, 0f);
        }

        public override void Exit()
        {
            
        }

        public override void LogicUpdate()
        {
            if (_inputSO.MoveInput != Vector2.zero)
            {
                _endState = true;
            }
        }

        public override void FixedUpdate()
        {
            
        }

        public override void AnimationTrigger()
        {
            
        }
    }
}
