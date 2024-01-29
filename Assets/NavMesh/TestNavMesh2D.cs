using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.AI
{
    public class TestNavMesh2D : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent2D _agent;
        [SerializeField] private Transform _target;

        private void Update()
        {
            _agent.SetDestination(_target.position);
        }
    }
}
