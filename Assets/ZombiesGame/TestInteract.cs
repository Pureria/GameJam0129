using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Core;

namespace Zombies
{
    public class TestInteract : MonoBehaviour
    {
        private Core.Core _core;
        private Interact _interact;
        
        private void Start()
        {
            _core = GetComponentInChildren<Core.Core>();
            _interact = _core.GetCoreComponent<Interact>();

            _interact.InteractEvent += Interact;
        }

        private void Interact(Core.Core targetCore)
        {
            Debug.Log($"{transform.name} : {targetCore.transform.root.name}にインタラクトされました。");
        }
    }
}

