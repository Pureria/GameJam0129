using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Core
{
    public class Interact : CoreComponent
    {
        public Action<Core> InteractEvent;
        public Action UseInteractEvent;
        
        private bool _canInteract;
        private bool _canInteractHold;
        private bool _showInteractText;
        private string _interactText;
        public bool CanInteractHold => _canInteractHold;
        public bool ShowInteractText => _showInteractText;
        public string InteractText => _interactText;

        protected override void Awake()
        {
            base.Awake();
            _canInteractHold = false;
        }
        
        private void Start()
        {
            _canInteract = true;
        }

        //インタラクトされたときに呼ばれる
        public void CallInteractEvent(Core targetCore)
        {
            if (!_canInteract) return;
            
            InteractEvent?.Invoke(targetCore);
        }
        
        //インタラクトできるモノを検索する
        public void FindInteract(Core core, Vector2 origin, Vector2 direction, float distance, LayerMask interactLayer)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance, interactLayer);
            Debug.DrawRay(origin, direction);
            
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform == null) continue;

                Core tCore = hit.transform.root.GetComponentInChildren<Core>();
                if (tCore == null) continue;
                if (!tCore.GetCoreComponentBool<Interact>(out Interact tInteract)) continue;
                tInteract.CallInteractEvent(core);
                
                if(!tInteract.CanInteractHold) UseInteractEvent?.Invoke();
                break;
            }
        }
        
        public bool CanInteract(Core core, Vector2 origin, Vector2 direction, float distance, LayerMask interactLayer, out string text)
        {
            text = "";
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance, interactLayer);
            Debug.DrawRay(origin, direction);
            
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform == null) continue;

                Core tCore = hit.transform.root.GetComponentInChildren<Core>();
                if (tCore == null) continue;
                if (!tCore.GetCoreComponentBool<Interact>(out Interact tInteract)) continue;
                if(!tInteract.ShowInteractText) continue;
                text = tInteract.InteractText;
                return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            if (Application.isPlaying)
            {
                //Gizmos.DrawLine(_origin, _direction);
                //Gizmos.DrawRay(_origin, _direction);
            }
        }

        public void SetCanInteract(bool canInteract) => _canInteract = canInteract;
        public void SetCanHoldInteract(bool canInteractHold) => _canInteractHold = canInteractHold;

        public void SetInteractText(string text)
        {
            _showInteractText = true;
            _interactText = text;
        }
    }
}
