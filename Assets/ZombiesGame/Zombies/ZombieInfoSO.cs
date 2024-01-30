using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zombies.Zombie
{
    [CreateAssetMenu(fileName = "ZombieInfoSO", menuName = "Zombies/Zombie/ZombieInfo")]
    public class ZombieInfoSO : ScriptableObject
    {
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _damageAmount;
        [SerializeField] private float _attackDistance;

        public float WalkSpeed => _walkSpeed;
        public float RunSPeed => _runSpeed;
        public float DamageAmount => _damageAmount;
        public float AttackDistance => _attackDistance;
    }
}