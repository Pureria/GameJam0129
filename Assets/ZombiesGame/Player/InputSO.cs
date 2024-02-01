using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Input
{
    [CreateAssetMenu(fileName = "InputSO", menuName = "Zombies/InputSO", order = 0)]
    public class InputSO : ScriptableObject
    {
        public Vector2 MoveInput;
        public Vector2 ViewPoint;
        public bool InteractInput;
        public bool ShotInput;
        public bool ReloadInput;
        public bool ChangeNextWeaponInput;
        public bool DashInput;
        
        public void UseShotInput() => ShotInput = false;
        public void UseReloadInput() => ReloadInput = false;
        public void UseInteractInput() => InteractInput = false;
        public void UseChangeNextWeaponInput() => ChangeNextWeaponInput = false;
    }
}
