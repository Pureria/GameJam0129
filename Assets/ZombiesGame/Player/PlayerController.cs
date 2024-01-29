using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Input;
using Zombies.Core;

namespace Zombies.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private InputSO _inputSO;

        [SerializeField] private PlayerStateInfo _stateInfo;

        private Core.Core _core;
        private Movement _movement;

        private void Start()
        {
            _core = this.GetComponentInChildren<Core.Core>();
            if (_core == null)
            {
                Debug.LogError("Coreが子オブジェクトに存在しません。");
                return;
            }

            _movement = _core.GetCoreComponent<Movement>();
        }

        private void Update()
        {
            //仮処理
            _movement.Move(_inputSO.MoveInput, _stateInfo.MoveSpeed);
        }
    }
}
