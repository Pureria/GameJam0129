using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Zombies.AI
{
    public class NavMeshAgent2D : MonoBehaviour
    {
        [Header("Steering")]
        public float _speed = 1.0f;
        public float stoppingDistance = 0;

        private bool _canMove;

        [HideInInspector]//常にUnityエディタから非表示
        private Vector2 trace_area=Vector2.zero;
        public Vector2 destination
        {
            get { return trace_area; }
            set
            {
                trace_area = value;
                Trace(transform.position, value);
            }
        }

        private void Start()
        {
            _canMove = true;
        }


        private void Trace(Vector2 current,Vector2 target)
        {
            if (Vector2.Distance(current,target) <= stoppingDistance || !_canMove)
            {
                return;
            }

            // NavMesh に応じて経路を求める
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(current, target, NavMesh.AllAreas, path);

            Vector2 corner = path.corners[0];

            if (Vector2.Distance(current, corner) <= 0.05f)
            {
                corner = path.corners[1];
            }

            transform.position = Vector2.MoveTowards(current, corner, _speed * Time.deltaTime);
        }
        public bool SetDestination(Vector2 target)
        {
            destination = target;
            return true;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void SetCanMove(bool canMove)
        {
            _canMove = canMove;
        }
    }
}