using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Core
{
    public class States : CoreComponent
    {
        private float _initMaxHealth;
        private float _maxHealth;
        private float _health;
        private float _healInterval;
        private float _damageTime;
        private Action _deadEvent;
        private Action _damageEvent;
        private Action _changeHealthEvent;
        private bool _isAutoHeal;
        private bool _isHealthMax;
        private bool _isDamageOne;
        private bool _isDead;
        private bool _isInvisible;
        private bool _isJeagerNog;

        public float Health => _health;
        public bool IsDead => _isDead;

        public void Initialize(float maxHealth, float healInterval, bool isAutoHeal, Action deadEvent, Action damageEvent, Action changeHealthEvent)
        {
            _initMaxHealth = maxHealth;
            _maxHealth = maxHealth;
            _health = maxHealth;
            _deadEvent = deadEvent;
            _damageEvent = damageEvent;
            _changeHealthEvent = changeHealthEvent;
            _healInterval = healInterval;
            _isAutoHeal = isAutoHeal;

            _isHealthMax = true;
            _damageTime = 0;
            _isDamageOne = false;
            _isDead = false;
            _isInvisible = false;
        }

        public void Damage(float damage)
        {
            if (_isInvisible) return;
            if (_isDamageOne) damage = 1;
            
            _damageTime = Time.time;
            _health -= damage;
            _damageEvent?.Invoke();
            _changeHealthEvent?.Invoke();
            _isHealthMax = false;
            
            if (_health <= 0)
            {
                _health = 0;
                _deadEvent?.Invoke();
                _isDead = true;
                _isInvisible = true;
            }
        }

        public void Heal(float heal)
        {
            _isDead = false;
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

        public void SetDamageOne(bool isDamageOne)
        {
            _isDamageOne = isDamageOne;
        }
        
        public void SetJugerNog(bool isJeagerNog)
        {
            _isJeagerNog = isJeagerNog;
            if (_isJeagerNog) SetHealth(_initMaxHealth * 2);
            else SetHealth(_initMaxHealth);
        }

        public void SetInvisible(bool invisible) => _isInvisible = invisible;

        private void SetHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
            _health = maxHealth;
        }
    }
}
