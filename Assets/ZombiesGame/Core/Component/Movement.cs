using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Zombies.Core
{
    public class Movement : CoreComponent
    {
        private bool _canMove;
        private Rigidbody2D _rb;

        private void Start()
        {
            _canMove = true;

            if (!transform.root.transform.TryGetComponent<Rigidbody2D>(out _rb))
            {
                Debug.Log("rootオブジェクトにRigidbody2Dが存在しません。");
            }
        }
        
        public void SetCanMove(bool canMove) { _canMove = canMove; }

        public void Move(Vector2 moveInput, float speed)
        {
            Vector2 moveVelocity = moveInput.normalized * speed;
            _rb.velocity = moveVelocity;
        }
    }
}
