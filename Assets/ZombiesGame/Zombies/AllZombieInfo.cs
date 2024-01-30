using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Zombies.Zombie
{
    [CreateAssetMenu(fileName = "AllZombieInfo", menuName = "Zombies/Zombie/AllZombieInfo")]
    public class AllZombieInfo : ScriptableObject
    {
        public Func<Transform> GetPlayerTransform;
    }
}
