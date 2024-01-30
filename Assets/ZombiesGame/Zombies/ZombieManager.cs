using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Zombies.Zombie
{
    public class ZombieManager : MonoBehaviour
    {
        public static ZombieManager Instance { get; private set; }

        //体力はbaseHealth + (baseHealth * (multipleHealth * (wave - 1)))にする
        [SerializeField] private float _baseHealth;
        [SerializeField] private float _multipleHealth;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);
        }

        public float GetMaxHealth()
        {
            //TODO::GameManagerからWabeを取得する
            //現在は仮でWabeを1としている
            return _baseHealth + (_baseHealth * (_multipleHealth * (1 - 1)));
        }
    }
}
