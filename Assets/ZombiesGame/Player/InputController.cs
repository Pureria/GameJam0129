using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Zombies.Input
{
    public class InputController : MonoBehaviour
    {
        public void OnMove(InputAction.CallbackContext context)
        {
            var inputValue = context.ReadValue<Vector2>();
        }

        public void OnShot(InputAction.CallbackContext context)
        {
            if (context.started)
            {
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
            }
            else if (context.canceled)
            {
            }
        }

        public void OnViewPoint(InputAction.CallbackContext context)
        {
            var inputValue = context.ReadValue<Vector2>();
        }
    }
}
