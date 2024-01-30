using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Core;

namespace Zombies.InteractObject
{
    public class BaseBuyObject : MonoBehaviour
    {
        [SerializeField] private BuyObjectInfoSO _buyObjectInfoSO;
        private Core.Core _core;
        private Interact _interact;

        protected virtual void Start()
        {
            _core = GetComponentInChildren<Core.Core>();
            _interact = _core.GetCoreComponent<Core.Interact>();

            _interact.InteractEvent += CheckCanBuy;
        }

        protected virtual void CheckCanBuy(Core.Core tCore)
        {
            if(!tCore.GetCoreComponentBool(out Inventory tInventory)) return;
            
            if (tInventory.Money >= _buyObjectInfoSO.Price)
            {
                tInventory.UseMoney(_buyObjectInfoSO.Price);
                Buy(tCore);
            }
        }

        protected virtual void Buy(Core.Core tCore)
        {
        }
    }
}