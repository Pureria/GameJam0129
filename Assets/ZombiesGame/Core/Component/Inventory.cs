using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies.Core
{
    public class Inventory : CoreComponent
    {
        private int _nowMoney;
        public int Money => _nowMoney;

        private void Start()
        {
            _nowMoney = 0;
        }

        public void AddMoney(int amount) => _nowMoney += amount;
    }
}
