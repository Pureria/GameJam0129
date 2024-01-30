using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies
{
    [CreateAssetMenu(fileName = "InventoryProgressSO", menuName = "Zombies/Inventory/InventoryProgressSO")]
    public class InventoryProgressSO : ScriptableObject
    {
        [Header("お金")]
        public float NowMoney = 0;
        [Header("銃関連")]
        public string NowGunName = "";
        public int CurrentMagazine = 0;
        public int CurrentAmmo = 0;
        public bool NowReload = false;
        //0~1
        public float ReloadProgress = 0;
        
        public void ResetProgressData()
        {
            NowMoney = 0;
            NowGunName = "";
            CurrentMagazine = 0;
            CurrentAmmo = 0;
            NowReload = false;
            ReloadProgress = 0;
        }
    }
}
