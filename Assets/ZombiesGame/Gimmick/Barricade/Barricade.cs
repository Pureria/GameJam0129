using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Zombie;

namespace Zombies.Gimmick
{
    public class Barricade : MonoBehaviour
    {
        [SerializeField] private Transform _spawnTran;
        [SerializeField] private GameObject _zombiePrefab;
        [SerializeField] private Animator _anim;

        private Core.Core _core;
        private Core.States _states;
        
        private void Start()
        {
            _core = this.GetComponentInChildren<Core.Core>();
            if (_core == null)
            {
                Debug.LogError("Coreが子オブジェクトに存在しません。");
                return;
            }
            
            _states = _core.GetCoreComponent<Core.States>();
            _states.Initialize(4, 0, false, Dead, Damage, ChangeHealth);
            _states.SetDamageOne(true);
            ChangeHealth();
            
            SpawnZombie();
        }

        private void SpawnZombie()
        {
            ZombiesController zombie = Instantiate(_zombiePrefab, _spawnTran.position, Quaternion.identity).GetComponent<ZombiesController>();
            zombie.Setup(this.transform);
        }
        
        private void Dead() {}
        private void Damage() {}

        private void ChangeHealth()
        {
            _anim.SetInteger("health", (int)_states.Health);
        }
    }
}
