using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Zombies.GameCamera
{
    [CreateAssetMenu(fileName = "CameraShake", menuName = "Zombies/Camera/CameraShake")]
    public class CameraShakeSO : ScriptableObject
    {
        public Action OnLowPowerShake;
        public Action OnMiddlePowerShake;
        public Action OnHighPowerShake;
    }
}
