using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Zombie
{
    public class Zombie_MoveState : ZombieBaseState
    {
        public Zombie_MoveState(Animator anim, string animName, ZombieInfoSO infoSO, ZombieStateEventSO stateEventSo) : base(anim, animName, infoSO, stateEventSo)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _stateEventSO.SetCanMoveEvent?.Invoke(true);   
        }
    }
}
