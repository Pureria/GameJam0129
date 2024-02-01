using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zombies.Manager;

namespace Zombies.UI
{
    public class RoundUI : MonoBehaviour
    {
        [SerializeField] private GameManageSO _gameManageSO;
        [SerializeField] private Text _text;

        private void Update()
        {
            _text.text = _gameManageSO.NowWaveCount.ToString();
        }
    }
}
