using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Core;

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
            if (_zombieLayer == (_zombieLayer | (1 << other.gameObject.layer)))
            {
                Core.Core zCore = other.GetComponentInChildren<Core.Core>();
                if (zCore == null) return;
                
                Core.Damage damage = zCore.GetCoreComponent<Damage>();
                if (damage == null) return;

                damage.IsDamage(_damageAmount, _pCore);
                Destroy(this.gameObject);
            }
            else if (_wallLayer == (_wallLayer | (1 << other.gameObject.layer)))
            {
                Destroy(this.gameObject);
            }
        }
    }
}