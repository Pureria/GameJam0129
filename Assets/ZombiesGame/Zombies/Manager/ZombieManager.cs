using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using Zombies.Manager;
using Zombies.Player;

namespace Zombies.Zombie
{
    public class ZombieManager : MonoBehaviour
    {
        public static ZombieManager Instance { get; private set; }

        //体力はbaseHealth + (baseHealth * (multipleHealth * (wave - 1)))にする
        [SerializeField] private float _baseHealth;
        [SerializeField] private float _multipleHealth;
        [SerializeField] private AllZombieInfo _allZombieInfo;
        [SerializeField] private Transform _lastStandMovePoint;
        
        [Header("Zombie Spawn Info")]
        [SerializeField] private GameObject _zombiePrefab;
        [SerializeField] private int _zombieSpawnCount;
        [SerializeField] private Transform _currentZombies;

        [Header("Event")]
        [SerializeField] private PlayerCallEvent _playerCallEvent;
        [SerializeField] private GameManageSO _gameManageSO;

        private int _deadCount;
        private bool _initialize;
        private bool _canInstantiate;
        private bool _lastStand;
        
        private List<ZombieListInfo> _zombieList = new List<ZombieListInfo>();

        [HideInInspector] public Transform TargetTransform;
        public Func<Transform> OnGetPlayerTransform;
        public int DeadCount => _deadCount;
        public bool LastStand => _lastStand;
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);

            _initialize = false;
            _canInstantiate = false;
            _lastStand = false;
        }

        private void Start()
        {
            for(int i = 0; i< _zombieSpawnCount; i++)
            {
                GameObject zombie = Instantiate(_zombiePrefab, _currentZombies);
                if (!zombie.TryGetComponent<ZombiesController>(out ZombiesController controller)) continue;
                controller.OnDeadEvent += Dead;
                ZombieListInfo zombieListInfo = new ZombieListInfo(zombie, i, controller);
                _zombieList.Add(zombieListInfo);
            }
            
            _deadCount = 0;
            TargetTransform = OnGetPlayerTransform?.Invoke();
            _initialize = true;
        }

        private void OnEnable()
        {
            _allZombieInfo.GetPlayerTransform += GetPlayerTran;
            
            _playerCallEvent.OnGamePlayEvent += SetTargetPlayer;
            _playerCallEvent.OnGameOverEvent += SetTargetLastStand;
            _playerCallEvent.OnLastStandEvent += SetTargetLastStand;
            
            _gameManageSO.OnGameStart += GameStart;
            _gameManageSO.OnGameEnd += GameEnd;
        }

        private void OnDisable()
        {
            _allZombieInfo.GetPlayerTransform -= GetPlayerTran;
            
            _playerCallEvent.OnGamePlayEvent -= SetTargetPlayer;
            _playerCallEvent.OnGameOverEvent -= SetTargetLastStand;
            _playerCallEvent.OnLastStandEvent -= SetTargetLastStand;

            _gameManageSO.OnGameStart -= GameStart;
            _gameManageSO.OnGameEnd -= GameEnd;

            foreach (var zombie in _zombieList)
            {
                zombie.OnDeadEvent -= Dead;
            }
        }

        private Transform GetPlayerTran()
        {
            return OnGetPlayerTransform?.Invoke();
        }

        public float GetMaxHealth()
        {
            //TODO::GameManagerからWabeを取得する
            //現在は仮でWabeを1としている
            return _baseHealth + (_baseHealth * (_multipleHealth * (_gameManageSO.NowWaveCount - 1)));
        }

        public bool ZombieInstantiate(Transform spawnPos, out ZombiesController controller)
        {
            controller = null;
            if (!_initialize) return false;
            foreach (var zombie in _zombieList)
            {
                if (zombie.IsActive) continue;
                zombie.Instantiate(spawnPos);
                controller = zombie.Controller;
                return true;
            }

            return false;
        }
        
        public void SetCanInstantiate(bool canInstantiate)
        {
            _canInstantiate = canInstantiate;
        }

        private void Dead()
        {
            _deadCount++;
        }

        private void SetTargetPlayer()
        {
            _lastStand = false;
            TargetTransform = OnGetPlayerTransform?.Invoke();
        }

        private void SetTargetLastStand()
        {
            _lastStand = true;
            TargetTransform = _lastStandMovePoint;
        }

        private void GameStart()
        {
            _deadCount = 0;
            SetCanInstantiate(true);
        }

        private void GameEnd()
        {
            SetCanInstantiate(false);
        }
    }

    public class ZombieListInfo
    {
        private bool _isActive;
        
        public GameObject ZombieObj { get; private set; }
        public ZombiesController Controller;
        public int ZombieID { get; private set; }

        public Action OnDeadEvent;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                ZombieObj.SetActive(value);
            }
        }

        public ZombieListInfo(GameObject zombieObj, int zombieID, ZombiesController controller)
        {
            ZombieObj = zombieObj;
            ZombieID = zombieID;
            Controller = controller;
            _isActive = false;
            
            controller.OnDeadEvent += Dead;
        }
        
        ~ZombieListInfo()
        {
            Controller.OnDeadEvent -= Dead;
        }

        public void Instantiate(Transform spawnPos)
        {
            IsActive = true;
            Controller.Initialize();
            
            //取得したローカル座標をワールド座標に変換して代入
            ZombieObj.transform.position = spawnPos.position;
        }
        
        public void Dead()
        {
            IsActive = false;
            OnDeadEvent?.Invoke();
        }
    }
}
