using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Zombies.Gun
{
    [CreateAssetMenu(fileName = "GunEventSO", menuName = "Zombies/Gun/GunEventSO")]
    public class GunEventSO : ScriptableObject
    {
        //すべての銃から呼ばれるイベント群
        public Action OnReloadStartEvent;
        public Action<float> OnReloadingEnvent;
        public Action OnReloadEndEvent;
        public Action<int> OnChangeCurrentMagazineEvent;
        public Action<int> OnChangeCurrentAmmoEvent;
    }
}
