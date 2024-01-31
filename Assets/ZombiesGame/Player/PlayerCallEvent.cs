using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Zombies.Player
{
    [CreateAssetMenu(fileName = "PlayerEvent", menuName = "Zombies/Player/PlayerEvent")]
    public class PlayerCallEvent : ScriptableObject
    {
        public Action OnGamePlayEvent;
        public Action OnLastStandEvent;
        public Action OnGameOverEvent;
    }
}