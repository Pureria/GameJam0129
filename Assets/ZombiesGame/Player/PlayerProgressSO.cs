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
    }
}
