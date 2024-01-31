using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zombies.UI
{
    public class GunInfoUI : MonoBehaviour
    {
        [SerializeField] private Text _currentMagazineText = null;
        [SerializeField] private Text _currentAmmoText = null;
        [SerializeField] private Text _currentGunNameText = null;
        [SerializeField] private InventoryProgressSO _inventoryProgressSO = null;

        private void OnEnable()
        {
            _inventoryProgressSO.RefleshInventoryEvent += RefleshUI;
        }

        private void OnDisable()
        {
            _inventoryProgressSO.RefleshInventoryEvent -= RefleshUI;
        }

        private void RefleshUI()
        {
            _currentGunNameText.text = _inventoryProgressSO.NowGunName;
            _currentMagazineText.text = _inventoryProgressSO.CurrentMagazine.ToString();
            _currentAmmoText.text = _inventoryProgressSO.CurrentAmmo.ToString();
        }
    }
}
