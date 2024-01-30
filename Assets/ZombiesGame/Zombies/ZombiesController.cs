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
        [SerializeField] private ZombieStateEventSO _stateEventSO;
        [SerializeField] private AllZombieInfo _allZombieInfo;

        [Header("Component")]
        [SerializeField] private NavMeshAgent2D _agent;
        [SerializeField] private Animator _anim;

        //バリケードがターゲットになっているか?
        private bool _isBarricade;
        
        private Transform _targetTran;

        private Core.Core _core;
        private Core.States _states;

        private StateMachine _stateMachine;

        private Zombie_IdleState _idleState;
        private Zombie_MoveState _moveState;

        private void OnEnable()
        {
            _stateEventSO.SetCanMoveEvent += SetCanMove;
            _stateEventSO.MoveEvent += Move;
        }

        private void OnDisable()
        {
            _stateEventSO.SetCanMoveEvent -= SetCanMove;
            _stateEventSO.MoveEvent -= Move;
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
            _states.Initialize(ZombieManager.Instance.GetMaxHealth(), 0, false, Dead, Damage, ChangeHealth);
            
            _agent.SetCanMove(true);
            _agent.SetSpeed(_infoSo.WalkSpeed);
            
            _idleState = new Zombie_IdleState(_anim, "idle", _infoSo, _stateEventSO);
            _moveState = new Zombie_MoveState(_anim, "move", _infoSo, _stateEventSO);
            _stateMachine = new StateMachine(_idleState);
        }

        private void Update()
        {
            if(_isBarricade)
                CheckBaricade();
            else
                CheckTarget();

            if (_stateMachine.CurrentState.EndState)
            {
                ChangeState();
            }
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
                //canMoveをfalseにする
                //ダメージステータスを抜けたらTrueにする
                //_stateMachine.ChangeState(_damageState);
                //SetCanMove(false);
                tDamage.IsDamage(_infoSo.DamageAmount);
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

                    tDamage.IsDamage(_infoSo.DamageAmount);
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
                
                default:
                    break;
            }
        }

        private void Move()
        {
            if (_targetTran == null) return;

            _agent.SetDestination(_targetTran.position);
        }
        
        private void SetCanMove(bool canMove) => _agent.SetCanMove(canMove);
        private void SetTarget(Transform tran) => _targetTran = tran;
        private void ChangeHealth(){}
        private void Damage(){ Debug.Log(($"{transform.name} :ダメージ"));}
        private void Dead(){ Debug.Log(($"{transform.name} :死亡"));}
    }
}
