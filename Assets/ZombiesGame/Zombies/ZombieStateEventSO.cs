using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Zombies.Zombie
{
    public class ZombieStateEventSO
    {
        public Action MoveEvent;
        public Action<bool> SetCanMoveEvent;
        public Action CheckAttackEvent;
    }
}
