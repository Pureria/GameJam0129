using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Zombies.InteractObject
{
    [CreateAssetMenu(fileName = "BuyObjectInfoSO", menuName = "Zombies/Interact/BuyObjectInfoSO")]
    public class BuyObjectInfoSO : ScriptableObject
    {
        [SerializeField] private string _name = "";
        [SerializeField] private int _priva = 0;

        public string Name => _name;
        public int Price => _priva;
    }
}
