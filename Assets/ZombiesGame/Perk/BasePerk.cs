using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Zombies.Perk
{
    public class BasePerk
    {
        public virtual void EnterPerk(Core.Core core){}
        public virtual void ExitPerk(Core.Core core){}
    }
}
