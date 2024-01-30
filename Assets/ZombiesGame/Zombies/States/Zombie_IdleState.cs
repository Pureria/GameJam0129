using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Zombie
{
    public class Zombie_IdleState : ZombieBaseState
    {
        public Zombie_IdleState(Animator anim, string animName, ZombieInfoSO infoSO, ZombieStateEventSO stateEventSo) : base(anim, animName, infoSO, stateEventSo)
        {
        }
        
        public override void Enter()
        {
            base.Enter();

            _stateEventSO.SetCanMoveEvent?.Invoke(false);
        }

        public override void Exit()
        {
            base.Exit();

            _stateEventSO.SetCanMoveEvent?.Invoke(true);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (_startTime + _infoSO.IdleWaitTime < Time.time)
            {
                _endState = true;
            }
        }
    }
}
