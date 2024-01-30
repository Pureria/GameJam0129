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

        private int _nowWeaponIndex
        {
            get => _nowWeaponIndex;
            set
            {
                //valueが_gunsの要素数を超えないように
                if (value >= _guns.Count) value = _guns.Count - 1;
                else if (value < 0) value = 0;
                
                _nowWeaponIndex = value;
                ChangeWeapon(value);
            }
        }
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

        //番号の武器へ切り替え
        private void ChangeWeapon(int index)
        {
            AllWeaponHide();
            _guns[index].gameObject.SetActive(true);
        }

        public void GetNewWeapon(GameObject getGun)
        {
            GameObject gun = Instantiate(getGun, _gunRootTransform);
            gun.transform.parent = _gunRootTransform;
            if (gun.TryGetComponent<Gun.Gun>(out Gun.Gun gunScript))
            {
                gunScript.Initialize();
                _guns.Add(gunScript);
            }
            else
            {
                Debug.LogError("武器にGunコンポーネントが存在しません。");
            }
        }

        public Gun.Gun GetActiveGun() { return _guns[_nowWeaponIndex]; }
        public void SetActiveGun(int index) { _nowWeaponIndex = index; }
        
        private void AllWeaponHide()
        {
            foreach (Gun.Gun gun in _guns)
            {
                gun.gameObject.SetActive(false);
            }
        }
    }
}
