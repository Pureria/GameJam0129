using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Zombies.InteractObject
{
    public class BuyDoor : BaseBuyObject
    {
        [SerializeField] private Animator _anim;

        protected override void Buy(Core.Core tCore)
        {
            base.Buy(tCore);
            
            Open(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask Open(CancellationToken token)
        {
            //Openのアニメーションを開始して終了したらオブジェクト削除
            _anim.Play("Open");
            await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f,
                cancellationToken: token);

            Destroy(this.gameObject);
        }
    }
}
