using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Zombies.UI
{
    public class SelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private Text _text;
        [SerializeField] private Color _rolLOverTextColor;
        private Color _baseTextColor;

        private void Start()
        {
            _fillImage.fillAmount = 0;
            _baseTextColor = _text.color;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _fillImage.DOFillAmount(1f, 0.25f).SetEase(Ease.OutCubic).Play();
            _text.DOColor(_rolLOverTextColor, 0.25f).Play();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _fillImage.DOFillAmount(0, 0.25f).SetEase(Ease.OutCubic).Play();
            _text.DOColor(_baseTextColor, 0.25f).Play();
        }
    }
}