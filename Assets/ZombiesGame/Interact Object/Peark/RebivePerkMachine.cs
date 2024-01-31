using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Zombies.InteractObject
{
    public class RebivePerkMachine : PerkMachine
    {
        [SerializeField] private Animator _anim;
        [SerializeField] private int _canBuyCount = 3;
        private int _currentBuyCount = 0;
        
        protected override bool Buy(Core.Core tCore)
        {
            if (!base.Buy(tCore)) return false;
            _currentBuyCount++;

            if (_currentBuyCount >= _canBuyCount)
            {
                _interact.SetCanInteract(false);
                DestroyPerkMachine(this.GetCancellationTokenOnDestroy()).Forget();
            }

            return true;
        }

        private async UniTask DestroyPerkMachine(CancellationToken token)
        {
            _anim.Play("Destroy");
            await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f,
                cancellationToken: token);

            Destroy(this.gameObject);
        }
    }
}
