using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Input;
using Zombies.State;

namespace Zombies.Player.State
{
    public class RunState : BasePlayerState
    {
        public RunState(Animator anim, string animName, PlayerStateInfo stateInfo, PlayerStateEvent stateEventSO, InputSO inputSO) : base(anim, animName, stateInfo, stateEventSO, inputSO)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if(_inputSO.MoveInput == Vector2.zero) _endState = true;
            if (!_inputSO.DashInput) _endState = true;
            
            if (_inputSO.InteractInput)
            {
                //_inputSO.UseInteractInput();
                _stateEvent.InteractEvent?.Invoke();
            }
            
            if (_inputSO.ChangeNextWeaponInput)
            {
                _stateEvent.ChangeWeaponEvent?.Invoke();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            _stateEvent.MoveEvent?.Invoke(_inputSO.MoveInput, _stateInfo.RunSpeed);
        }
    }
}