using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Zombies
{
    [CreateAssetMenu(fileName = "InventoryProgressSO", menuName = "Zombies/Inventory/InventoryProgressSO")]
    public class InventoryProgressSO : ScriptableObject
    {
        public Action RefleshInventoryEvent;
        
        [Header("お金")]
        public int NowMoney = 0;
        [Header("現在取得している銃の情報")]
        public List<string> GunList = new List<string>();
        [Header("現在アクティブな銃情報")]
        public string NowGunName = "";
        public int CurrentMagazine = 0;
        public int CurrentAmmo = 0;
        public bool NowReload = false;
        //0~1
        public float ReloadProgress = 0;
        
        public void ResetProgressData()
        {
            GunList.Clear();
            NowMoney = 0;
            NowGunName = "";
            CurrentMagazine = 0;
            CurrentAmmo = 0;
            NowReload = false;
            ReloadProgress = 0;
        }
    }
}
