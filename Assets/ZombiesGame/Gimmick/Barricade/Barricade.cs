using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Core;
using Zombies.Zombie;

namespace Zombies.Gimmick
{
    public class Barricade : MonoBehaviour
    {
        [SerializeField] private Transform _spawnTran;
        [SerializeField] private GameObject _zombiePrefab;
        [SerializeField] private Animator _anim;
        [SerializeField] private float _reepairInterval = 2.0f;

        private Core.Core _core;
        private Core.States _states;
        private Core.Interact _interact;
        private float _repairStartTime;
        
        private void Start()
        {
            _core = this.GetComponentInChildren<Core.Core>();
            if (_core == null)
            {
                Debug.LogError("Coreが子オブジェクトに存在しません。");
                return;
            }
            
            _states = _core.GetCoreComponent<Core.States>();
            _interact = _core.GetCoreComponent<Interact>();
            _states.Initialize(4, 0, false, Dead, Damage, ChangeHealth);
            _states.SetDamageOne(true);
            _interact.InteractEvent += Repair;
            ChangeHealth();
            
            SpawnZombie();
        }

        private void OnDisable()
        {
            _interact.InteractEvent -= Repair;
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

        private void Repair(Core.Core tCore)
        {
            if(_repairStartTime + _reepairInterval > Time.time) return;

            _repairStartTime = Time.time;
            _states.Heal(1);
            ChangeHealth();
            Inventory tInventory = tCore.GetCoreComponent<Inventory>();
            if (tInventory == null) return;

            tInventory.AddMoney(10);
        }
    }
}
