using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Perk;
using Zombies.Core;

namespace Zombies.InteractObject
{
    public class DoubleTapPerkMachine : BasePerkMachine
    {
        protected override void Buy(Core.Core tCore)
        {
            if (!CheckCanPerkBuy<DoubleTapPerk>(tCore)) return;
            if (!tCore.GetCoreComponentBool<PerkInventory>(out PerkInventory tPerkInventory)) return;
            tPerkInventory.AddPeark(new DoubleTapPerk());
        }
    }
}
