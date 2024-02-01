using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Player;
using Zombies.Zombie;

namespace Zombies.Manager
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Info")] 
        [SerializeField] private GameManageSO _gameManageSO;
        [SerializeField] private PlayerController _playerController;

        [SerializeField] private ZombieManager _zombieManager;

        private bool _isStartGame;
        private bool _nowGame;
        
        private int _waveCount;

        private float _startTime;
        public int Wave => _waveCount;

        private void Awake()
        {
            _nowGame = false;
            _gameManageSO.IsPlayerInit = false;
            _gameManageSO.IsZombieInit = false;
            _isStartGame = false;
        }

        private void OnEnable()
        {
            _zombieManager.OnGetPlayerTransform += GetPlayerTransform;
            _playerController.CallEvent.OnGameOverEvent += GameEnd;
        }

        private void OnDisable()
        {
            _zombieManager.OnGetPlayerTransform -= GetPlayerTransform;
            _playerController.CallEvent.OnGameOverEvent -= GameEnd;
        }

        private void Start()
        {
            //GameStart();
        }

        private void Update()
        {
            if (!_nowGame)
            {
                if (_isStartGame) return;
                if(_gameManageSO.IsPlayerInit && _gameManageSO.IsZombieInit)
                {
                    GameStart();
                    _isStartGame = true;
                }
                return;
            }
            
            //waveCountは1Wabeはゾンビ10体、2Wabeはゾンビ15体・・・と5体ずつ必要数が増える
            if (_zombieManager.DeadCount == _waveCount * 5 + 5)
            {
                _waveCount++;
                _gameManageSO.NowWaveCount = _waveCount;
            }
        }

        private void GameStart()
        {
            _startTime = Time.time;
            _waveCount = 1;
            _gameManageSO.NowWaveCount = 1;
            _nowGame = true;
            _gameManageSO.OnGameStart?.Invoke();
        }

        private void GameEnd()
        {
            _gameManageSO.GameTime = Time.time - _startTime;
            _nowGame = false;
            _gameManageSO.OnGameEnd?.Invoke();
            _gameManageSO.IsPlayerInit = false;
            _gameManageSO.IsZombieInit = false;
        }

        private Transform GetPlayerTransform()
        {
            return _playerController.transform;
        }
    }
}
