using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Core
{
    public class Inventory : CoreComponent
    {
        private List<Gun.Gun> _guns = new List<Gun.Gun>();
        private int _nowMoney;
        private Transform _gunRootTransform;

        private int _nowWeaponIndex;
        
        public int Money => _nowMoney;

        private void Start()
        {
            _nowMoney = 0;
            _nowWeaponIndex = 0;
        }

        public void AddMoney(int amount) => _nowMoney += amount;

        //次の武器へ切り替え
        public void ChangeWeapon()
        {
            if (_nowWeaponIndex + 1 >= _guns.Count) _nowWeaponIndex = 0;
            else _nowWeaponIndex++;
        }
        
        public void AddGun(GameObject getGun)
        {
            GameObject gun = Instantiate(getGun, _gunRootTransform);
            gun.transform.parent = _gunRootTransform;
            if (gun.TryGetComponent<Gun.Gun>(out Gun.Gun gunScript))
            {
                gunScript.Initialize();

                //ミュールキックは実装する予定なし！！
                if (_guns.Count >= 2)
                {
                    //武器が2つ存在する場合は現在装備している武器を新しい武器に切り替える
                    GameObject delGunObj = _guns[_nowWeaponIndex].gameObject;
                    _guns[_nowWeaponIndex] = gunScript;
                    Destroy(delGunObj);
                    _guns[_nowWeaponIndex].Initialize();
                }
                else
                {
                    //追加したうえでその武器に切り替える
                    _guns.Add(gunScript);
                    gunScript.Initialize();
                    SetActiveGun(_nowWeaponIndex + 1);
                }
                
            }
            else
            {
                Debug.LogError("武器にGunコンポーネントが存在しません。");
            }
        }

        public Gun.Gun GetActiveGun() { return _guns[_nowWeaponIndex]; }

        public void SetActiveGun(int index)
        {
            //valueが_gunsの要素数を超えないように
            if (index > _guns.Count) index = _guns.Count - 1;
            else if (index < 0) index = 0;
                
            _nowWeaponIndex = index;
            ChangeWeapon(_nowWeaponIndex);
        }

        public void Initialize(int initMoney, GameObject initGun, Transform gunRootTran)
        {
            _gunRootTransform = gunRootTran;
            AddMoney(initMoney);
            _guns.Clear();
            
            GameObject gun = Instantiate(initGun, _gunRootTransform);
            gun.transform.parent = _gunRootTransform;
            if (gun.TryGetComponent<Gun.Gun>(out Gun.Gun gunScript))
            {
                _guns.Add(gunScript);
                gunScript.Initialize();
                SetActiveGun(0);
            }
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
        }
    }
}
