using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Zombies.Input;
using Zombies.Core;
using Zombies.Manager;
using Zombies.Perk;
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
        [SerializeField] private Transform _gunRootTran;
        [SerializeField] private PlayerProgressSO _playerProgressSO;
        [SerializeField] private InventoryProgressSO _inventoryProgressSO;
        
        [Header("Current Data")]
        [SerializeField] private InputSO _inputSO;
        [SerializeField] private PlayerStateEvent _stateEventSO;
        [SerializeField] private CurrentPerkSO _currentPerkSO;
        [SerializeField] private PlayerCallEvent _callEvent;
        [SerializeField] private GameManageSO _manageSo;

        [Header("Component")]
        [SerializeField] private Animator _anim;

        private bool _isRight;
        private bool _isGamePlay;
        
        private Core.Core _core;
        private Movement _movement;
        private Interact _interact;
        private States _states;
        private Inventory _inventory;
        private PerkInventory _perkInventory;

        private StateMachine _stateMachine;

        private State.IdleState _idleState;
        private State.MoveState _moveState;
        private State.LastStandState _lastStandState;
        private State.DeadState _deadState;
        private State.RunState _runState;

        public PlayerCallEvent CallEvent => _callEvent;
        
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
            _inventory = _core.GetCoreComponent<Inventory>();
            _perkInventory = _core.GetCoreComponent<PerkInventory>();

            _states.Initialize(_stateInfo.InitHealth, _stateInfo.HealInterval, true, Dead, Damage, ChangeHealth);
            _inventory.Initialize(_stateInfo.InitMoney, _stateInfo.InitGun, _gunRootTran, _inventoryProgressSO);
            _interact.UseInteractEvent += _inputSO.UseInteractInput;
            _perkInventory.RefleshPerkEvent += _currentPerkSO.RefleshPerk;
            
            _idleState = new IdleState(_anim,"idle", _stateInfo, _stateEventSO, _inputSO);
            _moveState = new MoveState(_anim, "move", _stateInfo, _stateEventSO, _inputSO);
            _lastStandState = new LastStandState(_anim, "lastStand", _stateInfo, _stateEventSO, _inputSO);
            _deadState = new DeadState(_anim, "lastStand", _stateInfo, _stateEventSO, _inputSO);
            _runState = new RunState(_anim, "run", _stateInfo, _stateEventSO, _inputSO);
            
            _stateMachine = new StateMachine(_idleState);
            
            if(_stateMachine == null) Debug.LogError("StateMachineが存在しません。");
            _isRight = true;

            _playerProgressSO.Health = _states.Health;
            _manageSo.IsPlayerInit = true;
        }
        
        private void OnEnable()
        {
            _stateEventSO.MoveEvent += Move;
            _stateEventSO.InteractEvent += Interact;
            _stateEventSO.ShotEvent += Shot;
            _stateEventSO.ReloadEvent += Reload;
            _stateEventSO.ChangeWeaponEvent += ChangeWeapon;
            _stateEventSO.GamePlayEvent += CallGamePlayEvent;
            _stateEventSO.LastStandEvent += CallLastStandEvent;
            _manageSo.OnGameStart += GameStart;
            _manageSo.OnGameEnd += GameEnd;
        }

        private void OnDisable()
        {
            _stateEventSO.MoveEvent -= Move;
            _stateEventSO.InteractEvent -= Interact;
            _stateEventSO.ShotEvent -= Shot;
            _stateEventSO.ReloadEvent -= Reload;
            _stateEventSO.ChangeWeaponEvent -= ChangeWeapon;
            _interact.UseInteractEvent -= _inputSO.UseInteractInput;
            _perkInventory.RefleshPerkEvent -= _currentPerkSO.RefleshPerk;
            _stateEventSO.GamePlayEvent -= CallGamePlayEvent;
            _stateEventSO.LastStandEvent -= CallLastStandEvent;
            _manageSo.OnGameStart -= GameStart;
            _manageSo.OnGameEnd -= GameEnd;
        }

        private void Update()
        {
            if (!_isGamePlay) return;
            
            _stateMachine.LogicUpdate();
            
            if (_stateMachine.CurrentState.EndState)
            {
                ChangeState();
            }
            
            CheckRotation();

            //_gunRootTranをマウスの方向に向ける
            Vector3 lookPos = GetMouseToWorldPoint();
            Vector3 gunPos = _gunRootTran.position;
            Vector3 gunLookPos = lookPos - gunPos;
            float angle = Mathf.Atan2(gunLookPos.y, gunLookPos.x) * Mathf.Rad2Deg;
            //_gunRootTran.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Transform tran = _inventory.GetActiveGun().transform;
            tran.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            //インタラクトできるものがあるか
            FindCanInteract();
        }

        private void FixedUpdate()
        {
            if (!_isGamePlay) return;
            
            _stateMachine.FixedUpdate();
        }
        
        private void OnDrawGizmosSelected()
        {
            #if UNITY_EDITOR
            Gizmos.color = Color.red;
            Vector3 pos = transform.position;

            if (EditorApplication.isPlaying)
            {
                Vector3 lookPos = GetMouseToWorldPoint();
                
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
            Vector3 lookPos = GetMouseToWorldPoint();
            
            _interact.FindInteract(_core, pos, ((Vector2)lookPos - (Vector2)pos).normalized , _stateInfo.InteractDistance,
                _interactLayer);
        }

        private void FindCanInteract()
        {
            Vector2 pos = transform.position;
            Vector3 lookPos = GetMouseToWorldPoint();
            if (_interact.CanInteract(_core, pos, ((Vector2)lookPos - (Vector2)pos).normalized,
                    _stateInfo.InteractDistance,
                    _interactLayer, out string text))
            {
                _playerProgressSO.CanInteract = true;
                _playerProgressSO.InteractText = text;
            }
            else
            {
                _playerProgressSO.CanInteract = false;
                _playerProgressSO.InteractText = "";
            }
        }

        private void CheckRotation()
        {
            Vector2 pos = transform.position;
            Vector3 lookPos = GetMouseToWorldPoint();
            
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
            Transform gunRoot = _inventory.GetActiveGun().transform;
            scale = gunRoot.localScale;
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            gunRoot.localScale = scale;
            _isRight = !_isRight;

            _anim.SetBool("isPlayerRight", _isRight);
        }

        private void ChangeHealth()
        {
            _playerProgressSO.Health = _states.Health;
            _playerProgressSO.ChangeHealth(_states.GetMaxHealth());
        }

        private void Damage() { Debug.Log($"{transform.name} : Damage"); }

        private void Dead()
        {
            Debug.Log($"{transform.name} : Dead");

            if (_perkInventory.CheckPerk<RevivePerk>())
            {
                _perkInventory.AllDelPerk();
                _stateMachine.ChangeState(_lastStandState);
            }
            else
            {
                //ゲームオーバーイベント
                Debug.Log("ゲームオーバー");
                _stateMachine.ChangeState(_deadState);
                _callEvent.OnGameOverEvent?.Invoke();
            }
            
        }

        private void Shot()
        {
            Gun.Gun gunScript = _inventory.GetActiveGun();
            gunScript.Shot(GetMouseToWorldPoint() - transform.position, _core);
            if (!gunScript.GetIsFullAuto() || gunScript.GetCurrentMagazine() <= 0) _inputSO.UseShotInput();
        }

        private void Reload()
        {
            _inputSO.UseReloadInput();
            Gun.Gun gunScript = _inventory.GetActiveGun();
            gunScript.StartReload();
        }

        private void ChangeWeapon()
        {
            _inputSO.UseChangeNextWeaponInput();
            _inventory.ChangeNextWeapon();
        }
        
        private void CallGamePlayEvent() => _callEvent.OnGamePlayEvent?.Invoke();
        private void CallLastStandEvent() => _callEvent.OnLastStandEvent?.Invoke();

        private Vector3 GetMouseToWorldPoint()
        {
            Vector3 lookPos = _inputSO.ViewPoint;
            lookPos.z = 1;
            lookPos = Camera.main.ScreenToWorldPoint(lookPos);
            return lookPos;
        }
        
        private void ChangeState()
        {
            switch (_stateMachine.CurrentState)
            {
                case IdleState:
                    _stateMachine.ChangeState(_moveState);
                    break;
                
                case MoveState:
                    if(_inputSO.MoveInput == Vector2.zero)
                        _stateMachine.ChangeState(_idleState);
                    else if(_inputSO.DashInput)
                        _stateMachine.ChangeState(_runState);
                    break;
                
                case LastStandState:
                    _states.SetInvisible(false);
                    _stateMachine.ChangeState(_idleState);
                    break;
                
                case RunState:
                    if(_inputSO.MoveInput == Vector2.zero) _stateMachine.ChangeState(_idleState);
                    else if(!_inputSO.DashInput) _stateMachine.ChangeState(_moveState);
                    break;
                
                default:
                    break;
            }
        }

        private void GameStart()
        {
            _isGamePlay = true;
            _stateMachine.ChangeState(_idleState);
        }

        private void GameEnd() { _isGamePlay = false;}
    }
}
