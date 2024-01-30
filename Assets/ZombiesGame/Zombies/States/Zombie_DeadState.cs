using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Zombie
{
    public class Zombie_DeadState : ZombieBaseState
    {
        public Zombie_DeadState(Animator anim, string animName, ZombieInfoSO infoSO, ZombieStateEventSO stateEventSo) : base(anim, animName, infoSO, stateEventSo)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _stateEventSO.SetCanMoveEvent?.Invoke(false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (_animationFinished)
            {
                _endState = true;
            }
        }
    }
}
