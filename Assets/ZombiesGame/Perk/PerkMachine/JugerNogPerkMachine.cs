using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Core;
using Zombies.InteractObject;
using Zombies.Perk;

namespace Zombies.InteractObject
{
    public class JugerNogPerkMachine : BasePerkMachine
    {
        protected override void Buy(Core.Core tCore)
        {
            if (!CheckCanPerkBuy<Perk.JugerNogPerk>(tCore)) return;
            if (!tCore.GetCoreComponentBool<PerkInventory>(out PerkInventory tPerkInventory)) return;
            tPerkInventory.AddPeark(new JugerNogPerk());
        }
    }
}
