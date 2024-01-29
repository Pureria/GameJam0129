using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Core
{
    public class Interact : CoreComponent
    {
        public Action<Core> InteractEvent;
        
        public void CallInteractEvent(Core targetCore)
        {
            InteractEvent?.Invoke(targetCore);
        }
        
        public void FindInteract(Core core, Vector2 origin, Vector2 direction, float distance, LayerMask interactLayer)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance, interactLayer);
            
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform == null) continue;

                Core tCore = hit.transform.root.GetComponentInChildren<Core>();
                if (tCore == null) continue;
                
                Interact tInteract = tCore.GetCoreComponent<Interact>();
                if (tInteract == null) continue;

                tInteract.CallInteractEvent(core);
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
