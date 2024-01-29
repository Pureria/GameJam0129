using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zombies.Player
{
[CreateAssetMenu(fileName = "PlayerStateInfo", menuName = "Zombies/PlayerStateInfo")]
    public class PlayerStateInfo : ScriptableObject
    {
        [SerializeField] private float _initHealth;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _stamina;
        [SerializeField] private float _interactDistance;

        public float InitHealth => _initHealth;
        public float MoveSpeed => _moveSpeed;
        public float RunSpeed => _runSpeed;
        public float Stamina => _stamina;
        public float InteractDistance => _interactDistance;
    }
}
