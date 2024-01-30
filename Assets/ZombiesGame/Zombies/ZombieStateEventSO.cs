using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Zombies.Zombie
{
    public class ZombieStateEventSO : ScriptableObject
    {
        public Action<bool> SetCanMoveEvent;
    }
}
