using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Player
{
    [CreateAssetMenu(fileName = "PlayerProgressSO", menuName = "Zombies/Player/PlayerProgress")]
    public class PlayerProgressSO : ScriptableObject
    {
        public float Health = 0;
        public float Stamina = 0;

        public Action<float,float> ChangeHealthEvent;

        public void ChangeHealth(float maxHealth)
        {
            ChangeHealthEvent?.Invoke(maxHealth, Health);
        }
    }
}
