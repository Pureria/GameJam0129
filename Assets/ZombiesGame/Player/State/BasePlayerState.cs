using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zombies.Input;
using Zombies.Player;

namespace Zombies.State
{
    public class BasePlayerState : BaseState
    {
        protected PlayerStateInfo _stateInfo;
        protected PlayerStateEvent _stateEvent;
        protected InputSO _inputSO;
        
        public BasePlayerState(Animator anim, string animName, PlayerStateInfo stateInfo, PlayerStateEvent stateEventSO, InputSO inputSO) : base(anim, animName)
        {
            this._stateInfo = stateInfo;
            this._stateEvent = stateEventSO;
            this._inputSO = inputSO;
        }
    }
}
