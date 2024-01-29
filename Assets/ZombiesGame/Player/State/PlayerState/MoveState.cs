using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Input;
using Zombies.State;

namespace Zombies.Player.State
{
    public class MoveState : BasePlayerState
    {
        public MoveState(Animator anim, string animName, PlayerStateInfo stateInfo, PlayerStateEvent stateEventSO, InputSO inputSO) : base(anim, animName, stateInfo, stateEventSO, inputSO)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void LogicUpdate()
        {
            if (_inputSO.MoveInput == Vector2.zero) _endState = true;
        }

        public override void FixedUpdate()
        {
            _stateEvent.MoveEvent?.Invoke(_inputSO.MoveInput, _stateInfo.MoveSpeed);
        }

        public override void AnimationTrigger()
        {
            
        }
    }
}
