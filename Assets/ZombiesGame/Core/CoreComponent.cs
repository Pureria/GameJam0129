using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Core
{
    public class CoreComponent : MonoBehaviour
    {
        protected Core _core;

        protected virtual void Awake()
        {
            _core = this.GetComponentInParent<Core>();
            if (_core == null)
            {
                Debug.LogError("Coreが親オブジェクトに存在しません。");
                return;
            }

            _core.AddComponent(this);
        }

        public virtual void LogicUpdate()
        {
            
        }

        public virtual void FixedUpdate()
        {
            
        }
    }
}
