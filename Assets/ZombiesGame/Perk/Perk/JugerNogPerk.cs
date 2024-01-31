using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies.Core;

namespace Zombies.Perk
{
    [CreateAssetMenu(fileName = "JugerNogPerk", menuName = "Zombies/Perk/JugerNogPerk")]
    public class JugerNogPerk : BasePerk
    {
        private States _tStates;
        
        public override void EnterPerk(Core.Core core)
        {
            if (!core.GetCoreComponentBool<States>(out _tStates)) return;

            _tStates.SetJugerNog(true);
        }
        
        public override void ExitPerk(Core.Core core)
        {
            if (_tStates == null) return;
            _tStates.SetJugerNog(false);
        }
    }
}
