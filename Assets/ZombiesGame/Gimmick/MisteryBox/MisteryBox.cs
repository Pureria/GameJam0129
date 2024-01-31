using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zombies.Core;
using Random = UnityEngine.Random;

namespace Zombies.InteractObject
{
    public class MisteryBox : BaseBuyObject
    {
        //[SerializeField,Tooltip("ボックスが開いてから閉まるまでの時間")] private float _closeTime = 10f;
        [SerializeField] private List<GameObject> _guns = new List<GameObject>();
        [SerializeField] private Animator _anim;

        private GameObject _selectGun;
        
        private CancellationTokenSource _cts;
        
        protected override bool Buy(Core.Core tCore)
        {
            base.Buy(tCore);
            
            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;
            StartRandomBox(token).Forget();
            return true;
        }

        private async UniTask StartRandomBox(CancellationToken token)
        {
            _interact.SetCanInteract(false);
            _anim.Play("Open");
            //開始アニメーション終了まで待つ
            await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f,
                cancellationToken: token);
            
            _selectGun = _guns[Random.Range(0, _guns.Count)];

            Debug.Log($"MisteryBox: {_selectGun.name}");
            
            _interact.InteractEvent = GetRandomGun;
            _interact.SetCanInteract(true);
            
            //closeTime秒待つ
            _anim.Play("Opened");
            await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f,
                cancellationToken: token);
            
            //イベントをセットしなおして閉じるアニメーションを再生
            _interact.SetCanInteract(false);
            _interact.InteractEvent = base.CheckCanBuy;

            await CloseRandomBox(token);
        }

        private async UniTask CloseRandomBox(CancellationToken token)
        {
            _anim.Play("Close");
            //閉じるアニメーション終了まで待つ
            await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f,
                cancellationToken: token);

            _interact.SetCanInteract(true);
        }

        private void GetRandomGun(Core.Core tCore)
        {
            _interact.SetCanInteract(false);
            _interact.InteractEvent = base.CheckCanBuy;
            
            if (!tCore.GetCoreComponentBool<Inventory>(out Inventory tInteract)) return;
            //if(_selectGun == null) _selectGun = _guns[Random.Range(0, _guns.Count - 1)];

            tInteract.AddGun(_selectGun);

            //タスクの終了処理
            _cts?.Cancel();
            
            _cts = new CancellationTokenSource();
            //閉じるアニメーションタスク
            CloseRandomBox(_cts.Token).Forget();
        }

        private void OnDisable()
        {
            _interact.InteractEvent = base.CheckCanBuy;
            _interact.SetCanInteract(true);
            
            _cts?.Cancel();
        }
    }
}
