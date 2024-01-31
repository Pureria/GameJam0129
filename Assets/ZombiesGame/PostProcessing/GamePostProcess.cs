using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zombies.Player;

namespace  ZombiesPostProcess
{
    public class GamePostProcess : MonoBehaviour
    {
        [SerializeField] private PlayerCallEvent _playerCallEvent;
        [SerializeField] private Volume _volume;
        [Header("Vignette")]
        [SerializeField] private float _defaultVignetteIntensity = 0.4f;
        [SerializeField] private float _lastStandVignetteIntensity = 0.8f;
        [Header("ColorAdjustments")]
        [SerializeField] private float _defaultSaturation = 0f;
        [SerializeField] private float _lastStandSaturation = -100f;
        
        private Vignette _vignette;
        private ColorAdjustments _colorAdjustments;

        private void OnEnable()
        {
            _playerCallEvent.OnGamePlayEvent += GamePlayPostProcess;
            _playerCallEvent.OnLastStandEvent += LastStandPostProcess;
        }

        private void OnDisable()
        {
            _playerCallEvent.OnGamePlayEvent -= GamePlayPostProcess;
            _playerCallEvent.OnLastStandEvent -= LastStandPostProcess;
        }

        private void Start()
        {
            _volume.profile.TryGet(out _vignette);
            _volume.profile.TryGet(out _colorAdjustments);
        }

        public void GamePlayPostProcess()
        {
            SetVignetteIntensity(_defaultVignetteIntensity);
            SetColorAdjustmentsSaturation(_defaultSaturation);
        }

        public void LastStandPostProcess()
        {
            SetVignetteIntensity(_lastStandVignetteIntensity);
            SetColorAdjustmentsSaturation(_lastStandSaturation);
        }

        private void SetVignetteIntensity(float value)
        {
            if (_vignette == null) return;
            _vignette.intensity.value = value;
        }
        
        private void SetColorAdjustmentsSaturation(float value)
        {
            if (_colorAdjustments == null) return;
            _colorAdjustments.saturation.value = value;
        }
    }
}
