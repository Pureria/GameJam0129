using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.State;

namespace Zombies.Zombie
{
    public class ZombieBaseState : BaseState
    {
        protected ZombieInfoSO _infoSO;
        protected ZombieStateEventSO _stateEventSO;
        
        public ZombieBaseState(Animator anim, string animName, ZombieInfoSO infoSO, ZombieStateEventSO stateEventSo) : base(anim, animName)
        {
            _infoSO = infoSO;
            _stateEventSO = stateEventSo;
        }
    }
}
