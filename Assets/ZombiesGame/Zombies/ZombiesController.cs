using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.AI;

namespace Zombies.Zombie
{
    public class ZombiesController : MonoBehaviour
    {
        [Header("info")]
        [SerializeField] private ZombieInfoSO _info;

        [Header("CurrentData")] 
        [SerializeField] private ZombieStateEventSO _stateEventSO;

        [Header("Component")]
        [SerializeField] private NavMeshAgent2D _agent;
        [SerializeField] private Animator _anim;

        private Transform _targetTran;

        private void OnEnable()
        {
            _stateEventSO.SetCanMoveEvent += SetCanMove;
        }

        private void OnDisable()
        {
            _stateEventSO.SetCanMoveEvent -= SetCanMove;
        }

        private void Start()
        {
            _agent.SetCanMove(true);
        }

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            if (_targetTran == null) return;

            _agent.SetDestination(_targetTran.position);
        }
        
        private void SetCanMove(bool canMove) => _agent.SetCanMove(canMove);
        private void SetTarget(Transform tran) => _targetTran = tran;
    }
}
