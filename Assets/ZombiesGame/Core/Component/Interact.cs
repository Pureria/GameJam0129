using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Core
{
    public class Interact : CoreComponent
    {
        public Action<Transform> InteractEvent;
        
        public void CallInteractEvent(Transform target)
        {
            InteractEvent?.Invoke(target);
        }
        
        public void FindInteract(Transform me, Vector2 origin, Vector2 direction, float distance, LayerMask interactLayer)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, distance, interactLayer);
            
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform == null) continue;

                Core tCore = hit.transform.root.GetComponentInChildren<Core>();
                if (tCore == null) continue;
                
                Interact tInteract = tCore.GetCoreComponent<Interact>();
                if (tInteract == null) continue;

                tInteract.CallInteractEvent(me);
                break;
            }
            
            /*
            if (hit.transform == null) return;

            Core tCore = hit.transform.root.GetComponentInChildren<Core>();
            if (tCore == null) return;
            
            Interact tInteract = tCore.GetCoreComponent<Interact>();
            if (tInteract == null) return;

            tInteract.CallInteract(me);
            */
        }
    }
}
