using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Perk;

namespace Zombies.Core
{
    public class PerkInventory : CoreComponent
    {
        //パークをリストで待つ
        private List<BasePerk> _pearkList = new List<BasePerk>();

        private void Awake()
        {
            base.Awake();
            
            _pearkList.Clear();
        }
        
        public void AddPeark<T>(T newPerk) where T : BasePerk
        {
            //同じ型のパークがなければ新しく追加
            foreach (BasePerk perk in _pearkList)
            {
                if (perk.GetType() == typeof(T))
                {
                    return;
                }
            }

            newPerk.EnterPerk(_core);
            _pearkList.Add(newPerk);
        }

        public void DelPeark<T>() where T : BasePerk
        {
            foreach (BasePerk perk in _pearkList)
            {
                if (perk.GetType() == typeof(T))
                {
                    perk.ExitPerk(_core);
                    _pearkList.Remove(perk);
                    return;
                }
            }
        }

        public void AllDelPerk()
        {
            foreach (BasePerk perk in _pearkList)
            {
                perk.ExitPerk(_core);
            }
            
            _pearkList.Clear();
        }

        /// <summary>
        /// 同じ型のパークがあるかチェック
        /// </summary>
        /// <typeparam name="T">確認するパークの方</typeparam>
        /// <returns>TRUE : 存在する False : 存在しない</returns>
        public bool CheckPerk<T>()
        {
            foreach (BasePerk perk in _pearkList)
            {
                if (perk.GetType() == typeof(T))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
