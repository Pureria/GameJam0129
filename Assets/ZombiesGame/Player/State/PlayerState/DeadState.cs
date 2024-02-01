using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Input;
using Zombies.State;

namespace Zombies.Player.State
{
    public class DeadState : BasePlayerState
    {
        public DeadState(Animator anim, string animName, PlayerStateInfo stateInfo, PlayerStateEvent stateEventSO, InputSO inputSO) : base(anim, animName, stateInfo, stateEventSO, inputSO)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _stateEvent.MoveEvent?.Invoke(Vector2.zero, 0f);
        }
    }
}
