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
        
        [Header("Zombie Spawn Info")]
        [SerializeField] private ZombieSpawnListenerSO _zombieSpawnListenerSO;
        [SerializeField] private int _listnerID = 0;
        [SerializeField] private bool _canStartSpawn = true;
        [SerializeField] private float _reepairInterval = 2.0f;
        [SerializeField] private float _spawnInterval = 2.0f;

        private Core.Core _core;
        private Core.States _states;
        private Core.Interact _interact;
        private float _repairStartTime;
        private CancellationTokenSource _cts;
        private bool _canSpawn;
        
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
            _interact.SetInteractText("Press Hold F to Repair");
            ChangeHealth();

            _canSpawn = _canStartSpawn;

            if (_canSpawn)
            {
                _cts = new CancellationTokenSource();
                SpawnZombieTask(_cts.Token).Forget();
            }
        }

        private void OnEnable()
        {
            _zombieSpawnListenerSO.OnSetSpawnZombieEvent += SetCanSpawn;
        }

        private void OnDisable()
        {
            _interact.InteractEvent -= Repair;
            _zombieSpawnListenerSO.OnSetSpawnZombieEvent -= SetCanSpawn;
            _cts?.Cancel();
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
            _states.SetInvisible(false);
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

        public void SetCanSpawn(bool canSpawn)
        {
            _canSpawn = canSpawn;
            
            if(_canSpawn)
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();
                SpawnZombieTask(_cts.Token).Forget();
            }
            else
            {
                _cts?.Cancel();
            }
        }
        
        public void SetCanSpawn(int listnerID, bool canSpawn)
        {
            if(_listnerID != listnerID) return;
            SetCanSpawn(canSpawn);
        }
    }
}
