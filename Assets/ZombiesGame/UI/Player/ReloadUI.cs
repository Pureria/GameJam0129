using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zombies;

public class ReloadUI : MonoBehaviour
{
    [SerializeField] private GameObject _reloadUI;
    [SerializeField] private Slider _slider;
    [SerializeField] private InventoryProgressSO _inventoryProgressSO;

    private bool _isNowShow;

    private void Start()
    {
        _isNowShow = false;
        _reloadUI.SetActive(false);
    }

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
        if (!_isNowShow)
        {
            if (_inventoryProgressSO.NowReload)
            {
                _reloadUI.SetActive(true);
                _isNowShow = true;
                _slider.value = _inventoryProgressSO.ReloadProgress;
            }
        }
        else
        {
            if (!_inventoryProgressSO.NowReload)
            {
                _reloadUI.SetActive(false);
                _isNowShow = false;
            }
            else
            {
                _slider.value = _inventoryProgressSO.ReloadProgress;
            }
        }
    }
}
