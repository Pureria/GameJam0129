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

        private bool _isRight;
        
        private Core.Core _core;
        private Movement _movement;
        private Interact _interact;
        private States _states;

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
            _states = _core.GetCoreComponent<States>();

            _states.Initialize(_stateInfo.InitHealth, _stateInfo.HealInterval, true, Dead, Damage, ChangeHealth);
            
            _idleState = new IdleState(_anim,"idle", _stateInfo, _stateEventSO, _inputSO);
            _moveState = new MoveState(_anim, "move", _stateInfo, _stateEventSO, _inputSO);

            _stateMachine = new StateMachine(_idleState);
            
            if(_stateMachine == null) Debug.LogError("StateMachineが存在しません。");
            _isRight = true;
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
            
            CheckRotation();
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

            if (_inputSO.MoveInput.x > 0) _anim.SetBool("isRightInput", true);
            else _anim.SetBool("isRightInput", false);
        }

        private void Interact()
        {
            Vector2 pos = transform.position;
            Vector3 lookPos = _inputSO.ViewPoint;
            lookPos.z = 1;
            lookPos = Camera.main.ScreenToWorldPoint(lookPos);
            
            _interact.FindInteract(_core, pos, (Vector2)lookPos.normalized, _stateInfo.InteractDistance,
                _interactLayer);
        }

        private void CheckRotation()
        {
            Vector2 pos = transform.position;
            Vector3 lookPos = _inputSO.ViewPoint;
            lookPos.z = 1;
            lookPos = Camera.main.ScreenToWorldPoint(lookPos);
            
            if (_isRight)
            {
                if (lookPos.x < pos.x) Flip();
            }
            else
            {
                if (lookPos.x > pos.x) Flip();
            }
        }

        private void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x = scale.x * -1;
            transform.localScale = scale;
            _isRight = !_isRight;

            _anim.SetBool("isPlayerRight", _isRight);
        }

        private void ChangeHealth()
        {
            Debug.Log($"{transform.name} : ChangeHealth");
        }

        private void Damage()
        {
            Debug.Log($"{transform.name} : Damage");
        }

        private void Dead()
        {
            Debug.Log($"{transform.name} : Dead");
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
