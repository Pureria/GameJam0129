using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Core;
using Zombies.Perk;

namespace Zombies.InteractObject
{
    public class BasePerkMachine : BaseBuyObject
    {
        /// <summary>
        /// パークを購入することができるか？
        /// </summary>
        /// <param name="tCore">購入するオブジェクトのコア</param>
        /// <typeparam name="T">チェックするパークの種類</typeparam>
        /// <returns>TRUE : 買える　FALSE : 買えない</returns>
        protected bool CheckCanPerkBuy<T>(Core.Core tCore) where T : BasePerk
        {
            if(!tCore.GetCoreComponentBool<PerkInventory>(out PerkInventory tPerkInventory)) return false;
            if (tPerkInventory.CheckPerk<T>()) return false;

            if (tCore.GetCoreComponentBool<Inventory>(out Inventory tInventory))
            {
                tInventory.UseMoney(_buyObjectInfoSO.Price);
            }
            return true;
        }

        protected override void CheckCanBuy(Core.Core tCore)
        {
            if(!tCore.GetCoreComponentBool(out Inventory tInventory)) return;
            
            if (tInventory.Money >= _buyObjectInfoSO.Price)
            {
                Buy(tCore);
            }
        }
    }
}
