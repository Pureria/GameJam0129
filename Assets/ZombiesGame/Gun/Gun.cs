using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Core;
using Zombies.Perk;

namespace Zombies.Gun
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private GunInfoSO _gunInfo;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private GameObject _ammoPrefab;
        [SerializeField] private GunEventSO _gunEventSO;
        
        /*マガジン内の弾*/ private int _currentMagazine;
        /*残りの弾*/ private int _currentAmmo;
        private bool _isReload;
        private float _shotTime;
        private float _reloadStartTime;

        public bool GetIsFullAuto() => _gunInfo.IsFullAuto;
        public int GetCurrentMagazine() => _currentMagazine;
        public int GetCurrentAmmo() => _currentAmmo;
        public GunInfoSO GetGunInfo() => _gunInfo;

        public Action OnShotEvent;

        private void Update()
        {
            if (_isReload && _reloadStartTime + _gunInfo.ReloadTime <= Time.time)
            {
                _isReload = false;
                Reload();
            }
            else if(_isReload)
            {
                _gunEventSO.OnReloadingEnvent?.Invoke((Time.time - _reloadStartTime) / _gunInfo.ReloadTime);
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

            float damage = _gunInfo.Damage;

            if (pCore.GetCoreComponentBool<PerkInventory>(out PerkInventory perkInventory))
            {
                if (perkInventory.CheckPerk<DoubleTapPerk>())
                {
                    damage = damage * 2;
                }
            }
            
            _shotTime = Time.time;
            _currentMagazine--;
            GameObject ammo = Instantiate(_ammoPrefab, _muzzle.position, Quaternion.identity);
            Ammo ammoScript = ammo.GetComponent<Ammo>();
            ammoScript.SetParam(to, _gunInfo.AmmoSpeed, damage, pCore);
            _gunEventSO.OnChangeCurrentMagazineEvent?.Invoke(_currentMagazine);
            
            OnShotEvent?.Invoke();
        }

        public void StartReload()
        {
            if (_isReload || _currentAmmo <= 0 || _currentMagazine ==  _gunInfo.MagazineSize) return;

            _isReload = true;
            _reloadStartTime = Time.time;
            
            _gunEventSO.OnReloadStartEvent?.Invoke();
        }

        public void ReloadCancel()
        {
            _isReload = false;
            _gunEventSO.OnReloadEndEvent?.Invoke();
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
            
            _gunEventSO.OnReloadEndEvent?.Invoke();
            _gunEventSO.OnChangeCurrentMagazineEvent?.Invoke(_currentMagazine);
            _gunEventSO.OnChangeCurrentAmmoEvent?.Invoke(_currentAmmo);
        }

        public void Initialize(Action shotEvent)
        {
            _currentAmmo = _gunInfo.MaxAmmo;
            _currentMagazine = _gunInfo.MagazineSize;
            _isReload = false;

            OnShotEvent = shotEvent;
        }
        
        public GunEventSO GetGunEventSO() => _gunEventSO;
    }
}
