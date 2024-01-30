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
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (_inputSO.MoveInput == Vector2.zero) _endState = true;
            
            if (_inputSO.InteractInput)
            {
                //_inputSO.UseInteractInput();
                _stateEvent.InteractEvent?.Invoke();
            }

            if (_inputSO.InteractInput)
            {
                _stateEvent.InteractEvent?.Invoke();
            }
            
            if (_inputSO.ReloadInput)
            {
                _stateEvent.ReloadEvent?.Invoke();
            }

            if (_inputSO.ChangeNextWeaponInput)
            {
                _stateEvent.ChangeWeaponEvent?.Invoke();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            _stateEvent.MoveEvent?.Invoke(_inputSO.MoveInput, _stateInfo.MoveSpeed);
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
        }
    }
}
