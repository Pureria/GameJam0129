using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Gun
{
    public class Ammo : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _myRb;
        [SerializeField] private LayerMask _zombieLayer;
        [SerializeField] private LayerMask _wallLayer;

        private Core.Core _pCore;
        private Vector2 _direction;
        private float _speed;
        private float _damageAmount;
        
        public void SetParam(Vector2 dir, float speed, float damage, Core.Core pCore)
        {
            _direction = dir;
            _speed = speed;
            _pCore = pCore;
            _damageAmount = damage;
        }

        private void FixedUpdate()
        {
            //directionの方向に_speedの速さで移動する
            _myRb.velocity = _direction.normalized * _speed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == _zombieLayer)
            {
                Core.Core zCore = other.GetComponentInChildren<Core.Core>();
                if (zCore == null) return;
                
                Core.Damage damage = this.GetComponent<Core.Damage>();
                if (damage == null) return;

                damage.IsDamage(_damageAmount, _pCore);
            }
            else if (other.gameObject.layer == _wallLayer)
            {
                Destroy(this.gameObject);
            }
        }
    }
}