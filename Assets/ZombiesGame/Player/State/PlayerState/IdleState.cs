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
            base.Enter();
            
            _stateEvent.MoveEvent?.Invoke(Vector2.zero, 0f);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (_inputSO.MoveInput != Vector2.zero)
            {
                _endState = true;
            }

            if (_inputSO.InteractInput)
            {
                _stateEvent.InteractEvent?.Invoke();
            }

            if (_inputSO.ShotInput)
            {
                _stateEvent.ShotEvent?.Invoke();
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
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
        }
    }
}
