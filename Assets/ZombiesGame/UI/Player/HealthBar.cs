using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zombies.Player;

namespace Zombies.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private PlayerProgressSO _progressSO;

        private void Awake()
        {
            _progressSO.ChangeHealthEvent += ChangeHealth;
        }

        private void OnDisable()
        {
            _progressSO.ChangeHealthEvent -= ChangeHealth;
        }

        private void ChangeHealth(float maxHealth, float currentHealth)
        {
            _healthSlider.maxValue = maxHealth;
            _healthSlider.value = currentHealth;
        }
    }
}
