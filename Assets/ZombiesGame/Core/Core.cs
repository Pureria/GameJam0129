using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Core
{
    public class Core : MonoBehaviour
    {
        private List<CoreComponent> _components = new List<CoreComponent>();

        private void Update()
        {
            foreach (CoreComponent comp in _components)
            {
                comp.LogicUpdate();
            }
        }

        private void FixedUpdate()
        {
            foreach (CoreComponent comp in _components)
            {
                comp.FixedUpdate();
            }
        }

        public void AddComponent(CoreComponent component)
        {
            if (!_components.Contains(component))
            {
                _components.Add(component);
            }
        }
        
        public void RemoveComponent(CoreComponent component)
        {
            if (_components.Contains(component))
            {
                _components.Remove(component);
            }
        }
        
        public T GetCoreComponent<T>() where T : CoreComponent
        {
            foreach (CoreComponent comp in _components)
            {
                if (comp is T)
                {
                    return (T) comp;
                }
            }
            Debug.Log($"{typeof(T)} : 指定したコンポーネントが存在しません。");
            return null;
        }

        public T GetCoreComponent<T>(ref T comp) where T : CoreComponent
        {
            comp = GetComponent<T>();
            return comp;
        }

        public bool GetCoreComponentBool<T>(out T comp) where T : CoreComponent
        {
            comp = GetComponent<T>();
            if(comp == null) return false;
            return true;
        }
    }
}
