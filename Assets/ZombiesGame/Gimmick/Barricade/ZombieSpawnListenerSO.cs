using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Zombies.Gimmick
{
    [CreateAssetMenu(fileName = "ZombieSpawnListenerSO", menuName = "Zombies/Gimmick/ZombieSpawnListenerSO")]
    public class ZombieSpawnListenerSO : ScriptableObject
    {
        public Action<int,bool> OnSetSpawnZombieEvent;

        public void OnSetSpawnZombie(List<int> sendIDs, bool isSpawn)
        {
            foreach (int sendID in sendIDs)
            {
                OnSetSpawnZombieEvent?.Invoke(sendID, isSpawn);
            }
        }
    }
}
