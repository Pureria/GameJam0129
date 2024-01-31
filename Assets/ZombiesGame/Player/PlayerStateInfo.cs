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
        [SerializeField] private float _healInterval = 2.0f;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _stamina;
        [SerializeField] private float _interactDistance;
        [SerializeField] private float _lastStandTime = 5.0f;
        [SerializeField] private int _initMoney = 500;
        [SerializeField] private GameObject _initGun;

        public float InitHealth => _initHealth;
        public float MoveSpeed => _moveSpeed;
        public float RunSpeed => _runSpeed;
        public float Stamina => _stamina;
        public float InteractDistance => _interactDistance;
        public float HealInterval => _healInterval;
        public float LastStandTime => _lastStandTime;
        public int InitMoney => _initMoney;
        public GameObject InitGun => _initGun;
    }
}
