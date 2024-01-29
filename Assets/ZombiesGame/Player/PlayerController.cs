using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Input;
using Zombies.Core;
using Zombies.Player.State;
using Zombies.State;

namespace Zombies.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private PlayerStateInfo _stateInfo;
        
        [Header("Current Data")]
        [SerializeField] private InputSO _inputSO;
        [SerializeField] private PlayerStateEvent _stateEventSO;

        [Header("Component")]
        [SerializeField] private Animator _anim;

        private Core.Core _core;
        private Movement _movement;
        private Interact _interact;

        private StateMachine _stateMachine;

        private State.IdleState _idleState;
        private State.MoveState _moveState;

        private void Start()
        {
            _core = this.GetComponentInChildren<Core.Core>();
            if (_core == null)
            {
                Debug.LogError("Coreが子オブジェクトに存在しません。");
                return;
            }

            _movement = _core.GetCoreComponent<Movement>();
            _interact = _core.GetCoreComponent<Interact>();
            
            _idleState = new IdleState(_anim,"idle", _stateInfo, _stateEventSO, _inputSO);
            _moveState = new MoveState(_anim, "move", _stateInfo, _stateEventSO, _inputSO);
            
            _stateMachine = new StateMachine(_idleState);
        }

        private void OnEnable()
        {
            _stateEventSO.MoveEvent += Move;
            _stateEventSO.InteractEvent += Interact;
        }

        private void OnDisable()
        {
            _stateEventSO.MoveEvent -= Move;
            _stateEventSO.InteractEvent -= Interact;
        }

        private void Update()
        {
            _stateMachine.LogicUpdate();
            
            if (_stateMachine.CurrentState.EndState)
            {
                ChangeState();
            }
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 pos = transform.position;
            Gizmos.DrawLine(pos, pos + transform.up * _stateInfo.InteractDistance);
        }

        private void Move(Vector2 moveInput, float speed)
        {
            _movement.Move(moveInput, speed);
        }

        private void Interact()
        {
            _interact.FindInteract(this.transform, transform.position, transform.up, _stateInfo.InteractDistance);
        }

        private void ChangeState()
        {
            switch (_stateMachine.CurrentState)
            {
                case IdleState:
                    _stateMachine.ChangeState(_moveState);
                    break;
                
                case MoveState:
                    _stateMachine.ChangeState(_idleState);
                    break;
                
                default:
                    break;
            }
        }
    }
}
