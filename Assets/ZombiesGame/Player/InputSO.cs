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
        
        public void UseShotInput() => ShotInput = false;
    }
}
