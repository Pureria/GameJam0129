using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

namespace Zombies.UI
{
    public class MoneyInfoUI : MonoBehaviour
    {
        [SerializeField] private Text _currentMoneyText = null;
        [SerializeField] private float _changeTime = 0.5f;
        [SerializeField] private InventoryProgressSO _inventoryProgressSO = null;

        private int _nowDispMoney = 0;
        
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
            _currentMoneyText.DOCounter(_nowDispMoney, _inventoryProgressSO.NowMoney, _changeTime, true).Play();
            _nowDispMoney = _inventoryProgressSO.NowMoney;
        }
    }
}
