using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using Zombies.Perk;

namespace Zombies.UI
{
    public class PerkIconUI : MonoBehaviour
    {
        [SerializeField] private List<Image> _icons;

        [SerializeField] private CurrentPerkSO _currentPerkSO;
        
        private void Awake()
        {
            AllHide();
        }
        
        private void OnEnable()
        {
            _currentPerkSO.RefleshPerkEvent += RefleshIcon;
        }
        
        private void OnDisable()
        {
            _currentPerkSO.RefleshPerkEvent -= RefleshIcon;
        }

        private void RefleshIcon(List<BasePerk> perks)
        {
            AllHide();
            int count = perks.Count;
            for (int i = 0; i < count; i++)
            {
                _icons[i].enabled = true;
                _icons[i].sprite = perks[i].PerkIcon;
            }
        }

        private void AllHide()
        {
            foreach (Image icon in _icons)
            {
                icon.enabled = false;
            }
        }
    }
}
