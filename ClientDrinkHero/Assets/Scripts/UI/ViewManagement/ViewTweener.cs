using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ViewTweener
{

    public static UnityAction ButtonClickTween(Button button, Sprite clickedSprite, TweenCallback callback, AudioType type=AudioType.SFXButtonYes)
    {
        UnityAction action = () =>
        {
            AudioController.Instance.PlayAudio(type);

            button.image.sprite = clickedSprite;

            Sequence sequence = DOTween.Sequence();
            RectTransform rectTransform = button.GetComponent<RectTransform>();

            sequence.Append(rectTransform.DOScale(0.85f, 0.1f))
                .SetEase(Ease.InBounce)
                .Append(rectTransform.DOScale(1, 0.1f))
                .SetEase(Ease.OutSine).OnComplete(callback);
        };

        return action;
    }

    public static void PulseTextTween(TextMeshProUGUI textMeshProUGUI)
    {
        Sequence sequence = DOTween.Sequence();
        RectTransform rectTransform = textMeshProUGUI.GetComponent<RectTransform>();

        sequence.Append(rectTransform.DOScale(0.85f,0.5f))
            .SetEase(Ease.InBounce)
            .Append(rectTransform.DOScale(1,0.5f))
            .SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
    }
    
}