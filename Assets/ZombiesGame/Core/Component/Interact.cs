using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Core
{
    public class Interact : CoreComponent
    {
        public Action<Transform> InteractEvent;

        public void CallInteract(Transform target)
        {
            InteractEvent?.Invoke(target);
        }

        public void FindInteract(Transform me, Vector2 origin, Vector2 direction, float distance)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, distance);
            if (hit == null) return;

            Core tCore = hit.transform.root.GetComponentInChildren<Core>();
            if (tCore == null) return;
            
            Interact tInteract = tCore.GetCoreComponent<Interact>();
            if (tInteract == null) return;

            tInteract.CallInteract(me);
        }
    }
}
