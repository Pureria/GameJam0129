using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Input
{
    [CreateAssetMenu(fileName = "InputSO", menuName = "Zombies/InputSO", order = 0)]
    public class InputSO : ScriptableObject
    {
        private Vector2 _moveInput;
        private Vector2 _viewPoint;
        private bool _shotInput;
        private bool _interact;

        public Vector2 MoveInput => _moveInput;
        public Vector2 ViewPoint => _viewPoint;
        public bool InteractInput => _interact;
        public bool ShotInput => _shotInput;
    }
}
