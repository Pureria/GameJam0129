using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Gun
{
    [CreateAssetMenu(fileName = "GunInfoSO", menuName = "Zombies/Gun/GunInfo")]
    public class GunInfoSO : ScriptableObject
    {
        [SerializeField] private string _gunName;
        [SerializeField] private int _magazineSize;
        [SerializeField] private int _maxAmmo;
        [SerializeField] private float _damage;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _reloadTime;
        [SerializeField] private float _ammoSpeed;
        [SerializeField] private bool _isFullAuto;

        public string GunName => _gunName;
        public int MagazineSize => _magazineSize;
        public int MaxAmmo => _maxAmmo;
        public float Damage => _damage;
        public float FireRate => _fireRate;
        public float ReloadTime => _reloadTime;
        public float AmmoSpeed => _ammoSpeed;
        public bool IsFullAuto => _isFullAuto;
    }
}
