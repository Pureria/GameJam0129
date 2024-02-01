using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zombies.Manager;

namespace Zombies.UI
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField] private GameManageSO _manageSO;
        [SerializeField] private GameObject _popupObject;
        [SerializeField] private Text _timeText;
        [SerializeField] private Text _wabeText;
        [SerializeField] private Animator _anim;

        private bool _isClickButton;

        private void Start()
        {
            _popupObject.SetActive(false);
            _isClickButton = false;
        }
        
        private void OnEnable()
        {
            _manageSO.OnGameEnd += ShowResult;
        }

        private void OnDisable()
        {
            _manageSO.OnGameEnd -= ShowResult;
        }

        private void ShowResult()
        {
            ShowResult(this.GetCancellationTokenOnDestroy()).Forget();
        }
        
        private async UniTask ShowResult(CancellationToken token)
        {
            _popupObject.SetActive(true);
            _isClickButton = false;
            
            //結果を表示
            _timeText.text = $"{_manageSO.GameTime.ToString("F2")}";
            _wabeText.text = $"{_manageSO.NowWaveCount}";
            
            _anim.Play("Open");
            await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f,
                cancellationToken: token);
            
            //クリックされるまで待つ
            await UniTask.WaitUntil(() => _isClickButton, cancellationToken: token);

            /*
            _anim.Play("Close");
            
            await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f,
                cancellationToken: token);
                */

            _popupObject.SetActive(false);
            SceneChangeTitle();
        }

        public void OnClickTitle()
        {
            _isClickButton = true;
        }

        private void SceneChangeTitle()
        {
            SceneManager.ChangeSceneWait(1, SceneChangeEffect.Fade, 0.5f);
        }
    }
}
