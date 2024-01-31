using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Zombies.Perk
{
    public class BasePerk : ScriptableObject
    {
        [SerializeField] private Sprite _perkIcon;
        
        public virtual void EnterPerk(Core.Core core){}
        public virtual void ExitPerk(Core.Core core){}

        public Sprite PerkIcon => _perkIcon;
    }
}
