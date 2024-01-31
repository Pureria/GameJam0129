using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Core;

namespace Zombies.InteractObject
{
    public class BaseBuyObject : MonoBehaviour
    {
        [SerializeField] protected BuyObjectInfoSO _buyObjectInfoSO;
        protected Core.Core _core;
        protected Interact _interact;
        protected string _interactText;

        protected virtual void Start()
        {
            _core = GetComponentInChildren<Core.Core>();
            _interact = _core.GetCoreComponent<Core.Interact>();

            _interactText = $"Buy {_buyObjectInfoSO.Name} : {_buyObjectInfoSO.Price}";
            _interact.SetInteractText(_interactText);
            _interact.InteractEvent += CheckCanBuy;
            
        }

        private void OnDisable()
        {
            _interact.InteractEvent -= CheckCanBuy;
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

        protected virtual bool Buy(Core.Core tCore)
        {
            return true;
        }
    }
}
