using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Perk;
using System;

namespace Zombies.Core
{
    public class PerkInventory : CoreComponent
    {
        //パークをリストで待つ
        private List<BasePerk> _pearkList = new List<BasePerk>();

        public Action<List<BasePerk>> RefleshPerkEvent;

        protected override void Awake()
        {
            base.Awake();
            
            _pearkList.Clear();
        }

        public void AddPerk(BasePerk addPerk)
        {
            if (!_pearkList.Contains(addPerk))
            {
                addPerk.EnterPerk(_core);
                _pearkList.Add(addPerk);
                RefleshPerkEvent?.Invoke(_pearkList);
            }
        }

        public void DelPeark<T>() where T : BasePerk
        {
            foreach (BasePerk perk in _pearkList)
            {
                if (perk.GetType() == typeof(T))
                {
                    perk.ExitPerk(_core);
                    _pearkList.Remove(perk);
                    RefleshPerkEvent?.Invoke(_pearkList);
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
            RefleshPerkEvent?.Invoke(_pearkList);
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

        /// <summary>
        /// 同じ型のパークがあるかチェック
        /// </summary>
        /// <typeparam name="checkPerk">確認するパークの方</typeparam>
        /// <returns>TRUE : 存在する False : 存在しない</returns>
        public bool CheckPerk(BasePerk checkPerk)
        {
            foreach (BasePerk perk in _pearkList)
            {
                if (perk == checkPerk)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
