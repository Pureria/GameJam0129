using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Core
{
    public class States : CoreComponent
    {
        private float _maxHealth;
        private float _health;
        private float _healInterval;
        private float _damageTime;
        private Action _deadEvent;
        private Action _damageEvent;
        private Action _changeHealthEvent;
        private bool _isAutoHeal;
        private bool _isHealthMax;

        public float Health => _health;

        public void Initialize(float maxHealth, float healInterval, bool isAutoHeal, Action deadEvent, Action damageEvent, Action changeHealthEvent)
        {
            _maxHealth = maxHealth;
            _health = maxHealth;
            _deadEvent = deadEvent;
            _damageEvent = damageEvent;
            _changeHealthEvent = changeHealthEvent;
            _healInterval = healInterval;
            _isAutoHeal = isAutoHeal;

            _isHealthMax = true;
            _damageTime = 0;
        }

        public void Damage(float damage)
        {
            _damageTime = Time.time;
            _health -= damage;
            _damageEvent?.Invoke();
            _changeHealthEvent?.Invoke();
            _isHealthMax = false;
            
            if (_health <= 0)
            {
                _health = 0;
                _deadEvent?.Invoke();
            }
        }

        public void Heal(float heal)
        {
            _health += heal;
            _changeHealthEvent?.Invoke();
            if (_health > _maxHealth)
            {
                _health = _maxHealth;
                _isHealthMax = true;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_isHealthMax || !_isAutoHeal) return;
            
            if (Time.time >= _damageTime + _healInterval)
            {
                Heal(1f);
            }
        }
    }
}
