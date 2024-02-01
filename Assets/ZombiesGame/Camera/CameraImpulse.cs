using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Zombies.GameCamera
{
    public class CameraImpulse : MonoBehaviour
    {
        [SerializeField] private CameraShakeSO _cameraShakeSO;
        [SerializeField] private CinemachineImpulseSource _source;
        [SerializeField] private float _lowPoer;
        [SerializeField] private float _middlePower;
        [SerializeField] private float _highPower;

        private void OnEnable()
        {
            _cameraShakeSO.OnLowPowerShake += LowPowerShake;
            _cameraShakeSO.OnMiddlePowerShake += MiddlePowerShake;
            _cameraShakeSO.OnHighPowerShake += HighPowerShake;
        }

        private void OnDisable()
        {
            _cameraShakeSO.OnLowPowerShake -= LowPowerShake;
            _cameraShakeSO.OnMiddlePowerShake -= MiddlePowerShake;
            _cameraShakeSO.OnHighPowerShake -= HighPowerShake;
        }

        private void LowPowerShake()
        {
            //Amplitude DainをlowPowerにする
            _source.m_ImpulseDefinition.m_AmplitudeGain = _lowPoer;
            _source.GenerateImpulse();
        }
        
        private void MiddlePowerShake()
        {
            //Amplitude DainをmiddlePowerにする
            _source.m_ImpulseDefinition.m_AmplitudeGain = _middlePower;
            _source.GenerateImpulse();
        }
        
        private void HighPowerShake()
        {
            //Amplitude DainをhighPowerにする
            _source.m_ImpulseDefinition.m_AmplitudeGain = _highPower;
            _source.GenerateImpulse();
        }
    }
}
