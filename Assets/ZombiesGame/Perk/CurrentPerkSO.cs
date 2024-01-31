using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Perk;

namespace Zombies.Perk
{
    [CreateAssetMenu(fileName = "CurrentPerkSO", menuName = "Zombies/Perk/CurrentPerkSO")]
    public class CurrentPerkSO : ScriptableObject
    {
        public Action<List<BasePerk>> RefleshPerkEvent;
        
        public void RefleshPerk(List<BasePerk> perks)
        {
            Debug.Log("RefleshPerk");
            RefleshPerkEvent?.Invoke(perks);
        }
    }
}
