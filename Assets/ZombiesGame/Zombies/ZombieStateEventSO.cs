using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Zombies.Zombie
{
    [CreateAssetMenu(fileName = "ZombieStateEventSO", menuName = "Zombies/Zombie/ZombieStateEvent")]
    public class ZombieStateEventSO : ScriptableObject
    {
        public Action MoveEvent;
        public Action<bool> SetCanMoveEvent;
        public Action CheckAttackEvent;
    }
}
