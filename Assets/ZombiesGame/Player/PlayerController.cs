using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Zombies.Input;
using Zombies.Core;
using Zombies.Player.State;
using Zombies.State;
using StateMachine = Zombies.State.StateMachine;

namespace Zombies.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private PlayerStateInfo _stateInfo;

        [SerializeField] private LayerMask _interactLayer;
        
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
            
            if(_stateMachine == null) Debug.LogError("StateMachineが存在しません。");
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
            #if UNITY_EDITOR
            Gizmos.color = Color.red;
            Vector3 pos = transform.position;

            if (EditorApplication.isPlaying)
            {
                Vector3 lookPos = _inputSO.ViewPoint;
                lookPos.z = 1;
                lookPos = Camera.main.ScreenToWorldPoint(lookPos);
                
                //DrawLineでposからlookPosの方向に線を引く、この時、線の長さは必ずInteractDistanceになる
                Gizmos.DrawLine((Vector2)pos, (Vector2)pos + ((Vector2)lookPos - (Vector2)pos).normalized * _stateInfo.InteractDistance);
                
                Gizmos.DrawSphere(lookPos, 0.5f);
            }
            else
            {
                Gizmos.DrawLine(pos, pos + transform.up * _stateInfo.InteractDistance);
            }
            #endif
        }

        private void Move(Vector2 moveInput, float speed)
        {
            _movement.Move(moveInput, speed);
        }

        private void Interact()
        {
            Vector2 pos = transform.position;
            _interact.FindInteract(_core, pos, (_inputSO.ViewPoint - pos).normalized, _stateInfo.InteractDistance,
                _interactLayer);
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
