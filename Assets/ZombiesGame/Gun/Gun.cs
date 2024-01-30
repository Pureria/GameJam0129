using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Gun
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private GunInfoSO _gunInfo;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private GameObject _ammoPrefab;
        
        /*マガジン内の弾*/ private int _currentMagazine;
        /*残りの弾*/ private int _currentAmmo;
        private bool _isReload;
        private float _shotTime;
        private float _reloadStartTime;

        public bool GetIsFullAuto() => _gunInfo.IsFullAuto;
        public int GetCurrentMagazine() => _currentMagazine;
        public int GetCurrentAmmo() => _currentAmmo;

        private void Update()
        {
            if (_isReload && _reloadStartTime + _gunInfo.ReloadTime <= Time.time)
            {
                _isReload = false;
                Reload();
            }
        }

        public void Shot(Vector2 to, Core.Core pCore)
        {
            if (_currentMagazine <= 0 ||
                _isReload ||
                _shotTime + _gunInfo.FireRate > Time.time)
            {
                return;
            }

            _shotTime = Time.time;
            _currentMagazine--;
            GameObject ammo = Instantiate(_ammoPrefab, _muzzle.position, Quaternion.identity);
            Ammo ammoScript = ammo.GetComponent<Ammo>();
            ammoScript.SetParam(to, _gunInfo.AmmoSpeed, _gunInfo.Damage, pCore);
        }

        public void StartReload()
        {
            if (_isReload || _currentAmmo <= 0) return;

            _isReload = true;
            _reloadStartTime = Time.time;
        }

        private void Reload()
        {
            _currentAmmo -= _gunInfo.MagazineSize - _currentMagazine;
            _currentMagazine = _gunInfo.MagazineSize;
            if (_currentAmmo < 0)
            {
                _currentMagazine += _currentAmmo;
                _currentAmmo = 0;
            }
        }

        public void Initialize()
        {
            _currentAmmo = _gunInfo.MaxAmmo;
            _currentMagazine = _gunInfo.MagazineSize;
            _isReload = false;
        }
    }
}
