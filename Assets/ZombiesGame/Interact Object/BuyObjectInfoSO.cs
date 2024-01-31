using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace  Zombies.InteractObject
{
    [CreateAssetMenu(fileName = "BuyObjectInfoSO", menuName = "Zombies/Interact/BuyObjectInfoSO")]
    public class BuyObjectInfoSO : ScriptableObject
    {
        [SerializeField] private string _name = "";
        [SerializeField] private int _price = 0;

        public string Name => _name;
        public int Price => _price;
    }
}
