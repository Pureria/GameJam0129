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

        public bool CanInteractHold => _canInteractHold;

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
                
                Interact tInteract = tCore.GetCoreComponent<Interact>();
                if (tInteract == null) continue;

                tInteract.CallInteractEvent(core);
                
                if(!tInteract.CanInteractHold) UseInteractEvent?.Invoke();
                break;
            }
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
    }
}
