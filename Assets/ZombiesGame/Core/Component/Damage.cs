using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Core
{
    public class Damage : CoreComponent
    {
        private States _states;
        public Action<Core> _damageEvent;

        private void Start()
        {
            _states = _core.GetCoreComponent<States>();
        }

        public void IsDamage(float damage, Core tCore)
        {
            if (_states == null) return;
            
            _states.Damage(damage);
            _damageEvent?.Invoke(tCore);
        }
    }
}
