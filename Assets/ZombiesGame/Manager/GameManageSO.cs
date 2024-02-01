using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Manager
{
    [CreateAssetMenu(fileName = "GameManageSO", menuName = "Zombies/Manager/GameManageSO")]
    public class GameManageSO : ScriptableObject
    {
        public Action OnGameStart;
        public Action OnGameEnd;

        public int NowWaveCount;

        public bool IsPlayerInit;
        public bool IsZombieInit;

        public float GameTime;
    }
}
