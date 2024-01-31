using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zombies.Core;
using Zombies.Zombie;
using Cysharp.Threading.Tasks;

namespace Zombies.Gimmick
{
    public class Barricade : MonoBehaviour
    {
        [SerializeField] private Transform _spawnTran;
        [SerializeField] private Animator _anim;
        [SerializeField] private float _reepairInterval = 2.0f;
        [SerializeField] private float _spawnInterval = 2.0f;

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
            _interact.SetCanHoldInteract(true);
            _states.Initialize(4, 0, false, Dead, Damage, ChangeHealth);
            _states.SetDamageOne(true);
            _interact.InteractEvent += Repair;
            ChangeHealth();
            
            SpawnZombieTask(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private void OnDisable()
        {
            _interact.InteractEvent -= Repair;
        }

        private void SpawnZombie()
        {
            //ZombiesController zombie = Instantiate(_zombiePrefab, _spawnTran.position, Quaternion.identity).GetComponent<ZombiesController>();
            //zombie.Setup(this.transform);

            if (ZombieManager.Instance.ZombieInstantiate(_spawnTran, out ZombiesController zombie))
            {
                zombie.Setup(this.transform);
            }
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
        
        //スポーンインターバル毎にゾンビをスポーンさせる
        //UniTaskを使う
        private async UniTask SpawnZombieTask(CancellationToken token)
        {
            //ゲーム中だったら
            bool NowGame = true;
            
            while (NowGame)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_spawnInterval), cancellationToken: token);
                SpawnZombie();
            }
        }
    }
}
