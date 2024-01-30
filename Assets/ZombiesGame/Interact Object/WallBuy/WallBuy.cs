using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Core;

namespace Zombies.InteractObject
{
    public class WallBuy : BaseBuyObject
    {
        [SerializeField] private WallBuySO _wallBuySo;
        
        protected override void Buy(Core.Core tCore)
        {
            base.Buy(tCore);
            if (!tCore.GetCoreComponentBool<Inventory>(out Inventory tInventory)) return;
            tInventory.AddGun(_wallBuySo.BuyGunPrefab);
        }
    }
}
