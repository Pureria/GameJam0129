using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Zombies.Input
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private InputSO _inputSO;
        
        public void OnMove(InputAction.CallbackContext context)
        {
            var inputValue = context.ReadValue<Vector2>();
            _inputSO.MoveInput = inputValue;
        }

        public void OnShot(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _inputSO.ShotInput = true;
            }
            else if (context.canceled)
            {
                _inputSO.ShotInput = false;
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _inputSO.InteractInput = true;
            }
            else if (context.canceled)
            {
                _inputSO.InteractInput = false;
            }
        }

        public void OnViewPoint(InputAction.CallbackContext context)
        {
            var inputValue = context.ReadValue<Vector2>();

            _inputSO.ViewPoint = inputValue;
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _inputSO.ReloadInput = true;
            }
            else if (context.canceled)
            {
                _inputSO.ReloadInput = false;
            }
        }

        public void OnChangeNextWeapon(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _inputSO.ChangeNextWeaponInput = true;
            }
            else if (context.canceled)
            {
                _inputSO.ChangeNextWeaponInput = false;
            }
        }

        public void OnDashInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _inputSO.DashInput = true;
            }
            else if (context.canceled)
            {
                _inputSO.DashInput = false;
            }
        }
    }
}
