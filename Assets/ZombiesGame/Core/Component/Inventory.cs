using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Gun;

namespace Zombies.Core
{
    public class Inventory : CoreComponent
    {
        //インベントリに機能持たせすぎ！！
        private List<Gun.Gun> _guns = new List<Gun.Gun>();
        private int _nowMoney;
        private int _nowWeaponIndex;
        private Transform _gunRootTransform;
        private InventoryProgressSO _progressSO;
        private GunEventSO _gunEventSo;

        private PerkInventory _perkInventory;
        
        public int Money => _nowMoney;
        public Func<Vector3> GetGunLocalScaleEvent; 

        private void Start()
        {
            //_nowMoney = 0;
            //_nowWeaponIndex = 0;
            
            _core.GetCoreComponent<PerkInventory>(ref _perkInventory);
        }

        private void OnDestroy()
        {
            foreach(Gun.Gun gun in _guns)
            {
                gun.OnShotEvent -= SetProgressData;
            }
        }

        public void AddMoney(int amount)
        {
            _nowMoney += amount;
            SetProgressData();
        }


        public void UseMoney(int amount)
        {
            _nowMoney -= amount;
            SetProgressData();
        }

        //次の武器へ切り替え
        public void ChangeNextWeapon()
        {
            int index = _nowWeaponIndex;
            
            if (index + 1 >= _guns.Count) index = 0;
            else index++;

            SetActiveGun(index);
        }
        
        public void AddGun(GameObject getGun)
        {
            GameObject gun = Instantiate(getGun, _gunRootTransform);
            gun.transform.localPosition = getGun.transform.localPosition;
            gun.transform.parent = _gunRootTransform;
            if (gun.TryGetComponent<Gun.Gun>(out Gun.Gun gunScript))
            {
                //ミュールキックは実装する予定なし！！
                if (_guns.Count >= 2)
                {
                    //武器が2つ存在する場合は現在装備している武器を新しい武器に切り替える
                    GameObject delGunObj = _guns[_nowWeaponIndex].gameObject;
                    _guns[_nowWeaponIndex] = gunScript;
                    Destroy(delGunObj);
                    _guns[_nowWeaponIndex].Initialize(SetProgressData);
                    SetActiveGun(_nowWeaponIndex);
                }
                else
                {
                    //追加したうえでその武器に切り替える
                    _guns.Add(gunScript);
                    gunScript.Initialize(SetProgressData);
                    SetActiveGun(_nowWeaponIndex + 1);
                }
                
            }
            else
            {
                Debug.LogError("武器にGunコンポーネントが存在しません。");
            }
            
            SetProgressData();
        }

        public Gun.Gun GetActiveGun() { return _guns[_nowWeaponIndex]; }

        public void SetActiveGun(int index)
        {
            _guns[_nowWeaponIndex].ReloadCancel();
            
            //valueが_gunsの要素数を超えないように
            if (index > _guns.Count) index = 0;
            else if (index < 0) index = _guns.Count - 1;
                
            _nowWeaponIndex = index;
            ChangeWeapon(_nowWeaponIndex);
        }

        public void Initialize(int initMoney, GameObject initGun, Transform gunRootTran, InventoryProgressSO progressSO)
        {
            _progressSO = progressSO;
            _progressSO.ResetProgressData();
            _gunRootTransform = gunRootTran;
            _guns.Clear();
            
            GameObject gun = Instantiate(initGun, _gunRootTransform);
            gun.transform.localPosition = initGun.transform.localPosition;
            gun.transform.parent = _gunRootTransform;
            if (gun.TryGetComponent<Gun.Gun>(out Gun.Gun gunScript))
            {
                _guns.Add(gunScript);
                gunScript.Initialize(SetProgressData);
                SetActiveGun(0);
            }
            
            AddMoney(initMoney);
            _gunEventSo = _guns[_nowWeaponIndex].GetGunEventSO();
            _gunEventSo.OnReloadStartEvent += ActiveGunReloadStart;
            _gunEventSo.OnReloadingEnvent += ActiveGunReloading;
            _gunEventSo.OnReloadEndEvent += ActiveGunReloadEnd;
            _gunEventSo.OnChangeCurrentMagazineEvent += ChangeCurrentMagazine;
            _gunEventSo.OnChangeCurrentAmmoEvent += ChangeCurrentAmmo;
        }
        
        private void AllWeaponHide()
        {
            foreach (Gun.Gun gun in _guns)
            {
                gun.gameObject.SetActive(false);
            }
        }
        
        private void ChangeWeapon(int index)
        {
            AllWeaponHide();
            _guns[index].gameObject.SetActive(true);
            
            Vector3 scale = Vector3.one;
            if(GetGunLocalScaleEvent != null) scale = GetGunLocalScaleEvent();
            _guns[index].transform.localScale = scale;
            
            SetProgressData();
        }

        private void SetProgressData()
        {
            _progressSO.NowMoney = _nowMoney;
            
            _progressSO.NowGunName = _guns[_nowWeaponIndex].GetGunInfo().GunName;
            _progressSO.CurrentMagazine = _guns[_nowWeaponIndex].GetCurrentMagazine();
            _progressSO.CurrentAmmo = _guns[_nowWeaponIndex].GetCurrentAmmo();
            
            _progressSO.GunList.Clear();
            foreach (Gun.Gun gun in _guns)
            {
                _progressSO.GunList.Add(gun.GetGunInfo().GunName);
            }
            
            _progressSO.RefleshInventoryEvent?.Invoke();
        }

        private void OnDisable()
        {
            _gunEventSo.OnReloadStartEvent -= ActiveGunReloadStart;
            _gunEventSo.OnReloadingEnvent -= ActiveGunReloading;
            _gunEventSo.OnReloadEndEvent -= ActiveGunReloadEnd;
            _gunEventSo.OnChangeCurrentMagazineEvent -= ChangeCurrentMagazine;
            _gunEventSo.OnChangeCurrentAmmoEvent -= ChangeCurrentAmmo;
        }

        private void ActiveGunReloadStart()
        {
            _progressSO.NowReload = true;
            _progressSO.ReloadProgress = 0;
        }
        
        private void ActiveGunReloadEnd()
        {
            _progressSO.NowReload = false;
            SetProgressData();
        }
        
        private void ActiveGunReloading(float progress)
        {
            _progressSO.ReloadProgress = progress;
            _progressSO.RefleshInventoryEvent?.Invoke();
        }
        
        private void ChangeCurrentMagazine(int currentMagazine)
        {
            _progressSO.CurrentMagazine = currentMagazine;
            _progressSO.RefleshInventoryEvent?.Invoke();
        }

        private void ChangeCurrentAmmo(int currentAmmo)
        {
            _progressSO.CurrentAmmo = currentAmmo;
            _progressSO.RefleshInventoryEvent?.Invoke();
        }
    }
}
