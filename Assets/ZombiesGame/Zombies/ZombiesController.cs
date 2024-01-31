using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;
using Zombies.AI;
using Zombies.Core;
using Zombies.State;

namespace Zombies.Zombie
{
    public class ZombiesController : MonoBehaviour
    {
        [Header("info")]
        [SerializeField] private ZombieInfoSO _infoSo;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private LayerMask _barricadeLayer;

        [Header("CurrentData")] 
        [SerializeField] private AllZombieInfo _allZombieInfo;
        private ZombieStateEventSO _stateEventSO;

        [Header("Component")]
        [SerializeField] private NavMeshAgent2D _agent;
        [SerializeField] private Animator _anim;

        //バリケードがターゲットになっているか?
        private bool _isBarricade;
        
        private Transform _targetTran;

        private Core.Core _core;
        private Core.States _states;
        private Core.Damage _damage;

        private StateMachine _stateMachine;

        private Zombie_IdleState _idleState;
        private Zombie_MoveState _moveState;
        private Zombie_AttackState _attackState;
        private Zombie_DeadState _deadState;

        public Action OnDeadEvent;
        public Action<ZombiesController> OnDestroyEvent;
        
        private void Awake()
        {
            _stateEventSO = new ZombieStateEventSO();
        }

        /*
        private void OnEnable()
        {
            _stateEventSO.SetCanMoveEvent += SetCanMove;
            _stateEventSO.MoveEvent += Move;
            _stateEventSO.CheckAttackEvent += CheckAnyTarget;
        }
        */

        private void OnDestroy()
        {
            _stateEventSO.SetCanMoveEvent -= SetCanMove;
            _stateEventSO.MoveEvent -= Move;
            _stateEventSO.CheckAttackEvent -= CheckAnyTarget;

            if (_damage != null)
            {
                _damage._damageEvent -= Damage;
            }
            
            OnDestroyEvent?.Invoke(this);
        }

        public void Initialize()
        {
            _states.Initialize(ZombieManager.Instance.GetMaxHealth(), 0, false, Dead, Damage, ChangeHealth);
            _agent.SetCanMove(true);
            _agent.SetSpeed(_infoSo.WalkSpeed);
            _isBarricade = true;

            _stateMachine.ChangeState(_idleState);
        }
        
        private void Start()
        {
            _core = GetComponentInChildren<Core.Core>();
            if (_core == null)
            {
                Debug.LogError("Coreが子オブジェクトに存在しません。");
                return;
            }
            _states = _core.GetCoreComponent<States>();
            _damage = _core.GetCoreComponent<Damage>();
            
            _stateEventSO.SetCanMoveEvent += SetCanMove;
            _stateEventSO.MoveEvent += Move;
            _stateEventSO.CheckAttackEvent += CheckAnyTarget;
            _damage._damageEvent += Damage;
            
            _idleState = new Zombie_IdleState(_anim, "idle", _infoSo, _stateEventSO);
            _moveState = new Zombie_MoveState(_anim, "move", _infoSo, _stateEventSO);
            _attackState = new Zombie_AttackState(_anim, "attack", _infoSo, _stateEventSO);
            _deadState = new Zombie_DeadState(_anim, "dead", _infoSo, _stateEventSO);
            _stateMachine = new StateMachine(_idleState);
            
            //Initialize();
            this.gameObject.SetActive(false);
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _infoSo.AttackDistance);
        }

        private void CheckAnyTarget()
        {
            if(_isBarricade)
                CheckBaricade();
            else
                CheckTarget();
        }

        /// <summary>
        /// 攻撃可能範囲に攻撃可能なものがあるか確認
        /// </summary>
        private void CheckTarget()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _infoSo.AttackDistance, _playerLayer);;
            
            foreach (Collider2D hit in hits)
            {
                Core.Core tcore = hit.GetComponentInChildren<Core.Core>();
                if (tcore == null) continue;

                Core.Damage tDamage = tcore.GetCoreComponent<Core.Damage>();
                if(tDamage == null) continue;

                //ダメージステータスに遷移
                _stateMachine.ChangeState(_attackState);
                //canMoveをfalseにする
                //ダメージステータスを抜けたらTrueにする
                tDamage.IsDamage(_infoSo.DamageAmount, _core);
                return;
            }
        }
        
        private void CheckBaricade()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _infoSo.AttackDistance, _barricadeLayer);
            
            foreach (Collider2D hit in hits)
            {
                Core.Core tcore = hit.GetComponentInChildren<Core.Core>();
                if (tcore == null) continue;

                Core.States tStates = tcore.GetCoreComponent<Core.States>();
                if(tStates == null) continue;

                if (tStates.Health <= 0)
                {
                    //バリケードが壊れているため
                    //ターゲットをプレイヤーに変更
                    SetTarget(_allZombieInfo.GetPlayerTransform());
                    _isBarricade = false;
                    return;
                }
                else
                {
                    Core.Damage tDamage = tcore.GetCoreComponent<Core.Damage>();
                    if(tDamage == null) continue;

                    _stateMachine.ChangeState(_attackState);
                    tDamage.IsDamage(_infoSo.DamageAmount, _core);
                    return;
                }
            }
        }

        private void ChangeState()
        {
            switch (_stateMachine.CurrentState)
            {
                case Zombie_IdleState:
                    _stateMachine.ChangeState(_moveState);
                    break;
                
                case Zombie_MoveState:
                    break;
                
                case Zombie_AttackState:
                    _stateMachine.ChangeState(_idleState);
                    break;
                
                case Zombie_DeadState:
                    OnDeadEvent?.Invoke();
                    break;
                
                default:
                    break;
            }
        }

        private void Move()
        {
            if (_targetTran == null) return;

            _agent.SetDestination(_targetTran.position);
        }

        //攻撃された際の処理
        private void Damage(Core.Core tCore)
        {
            Inventory tInventory = tCore.GetCoreComponent<Inventory>();
            if (tInventory == null) return;

            if (_states.IsDead) tInventory.AddMoney(_infoSo.KillMoney);
            else tInventory.AddMoney(_infoSo.HitMoney);
        }
        
        private void SetCanMove(bool canMove) => _agent.SetCanMove(canMove);
        public void SetTarget(Transform tran) => _targetTran = tran;
        private void ChangeHealth(){}
        private void Damage(){ Debug.Log(($"{transform.name} :ダメージ"));}
        private void Dead(){ _stateMachine.ChangeState(_deadState); }

        public void Setup(Transform target)
        {
            SetTarget(target);
        }

        public void AnimationFinishTrigger()
        {
            _stateMachine.CurrentState.AnimationFinishTrigger();
        }
    }
}
