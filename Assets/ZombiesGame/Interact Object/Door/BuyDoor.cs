using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zombies.Gimmick;

namespace Zombies.InteractObject
{
    public class BuyDoor : BaseBuyObject
    {
        [SerializeField] private Animator _anim;
        [SerializeField] private List<int> _sendID = new List<int>();
        [SerializeField] private ZombieSpawnListenerSO _zombieSpawnListenerSo;

        protected override bool Buy(Core.Core tCore)
        {
            base.Buy(tCore);
            
            Open(this.GetCancellationTokenOnDestroy()).Forget();
            return true;
        }

        private async UniTask Open(CancellationToken token)
        {
            //Openのアニメーションを開始して終了したらオブジェクト削除
            _anim.Play("Open");
            await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f,
                cancellationToken: token);

            //_zombieSpawnListenerSo.OnSetSpawnZombieEvent(_sendID, true);
            _zombieSpawnListenerSo.OnSetSpawnZombie(_sendID, true);
            
            Destroy(this.gameObject);
        }
    }
}
