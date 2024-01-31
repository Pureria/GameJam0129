using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zombies.Player;

namespace Zombies.UI
{
    public class CanInteractTextUI : MonoBehaviour
    {
        [SerializeField] private Text _canInteractText = null;
        [SerializeField] private PlayerProgressSO _progressSO = null;

        private bool _nowShow = false;

        private void Start()
        {
            _canInteractText.gameObject.SetActive(false);
            _nowShow = false;
        }

        private void Update()
        {
            Reflesh();
        }

        private void Reflesh()
        {
            if (_progressSO.CanInteract)
            {
                if (!_nowShow)
                {
                    _nowShow = true;
                    _canInteractText.gameObject.SetActive(true);
                    _canInteractText.text = _progressSO.InteractText;
                }
                else
                {
                    _canInteractText.text = _progressSO.InteractText;
                }
            }
            else
            {
                if (_nowShow)
                {
                    _nowShow = false;
                    _canInteractText.gameObject.SetActive(false);
                    _canInteractText.text = "";
                }
            }
        }
    }
}
