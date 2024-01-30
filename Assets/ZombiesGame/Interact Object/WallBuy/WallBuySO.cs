using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zombies.InteractObject
{
    [CreateAssetMenu(fileName = "WallBuySO", menuName = "Zombies/Interact/WallBuySO")]
    public class WallBuySO : ScriptableObject
    {
        [FormerlySerializedAs("_buyWeaponPrefab")] [SerializeField] private GameObject buyGunPrefab;
        
        public GameObject BuyGunPrefab => buyGunPrefab;
    }
}
