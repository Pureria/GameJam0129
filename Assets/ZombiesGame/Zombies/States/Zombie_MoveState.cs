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

        public override void LogicUpdate()
        {
            base.LogicUpdate();
         
            _stateEventSO.CheckFlipEvent?.Invoke();
            _stateEventSO.CheckAttackEvent?.Invoke();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            _stateEventSO.MoveEvent?.Invoke();
        }
    }
}
