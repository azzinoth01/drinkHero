using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    private Image _loadingProgressBar;

    private void Awake()
    {
        PulseTextTween(loadingText);
        _loadingProgressBar = GetComponent<Image>();
    }

    private void Update()
    {
        if (_loadingProgressBar.fillAmount < SceneLoader.GetLoadingProgress()) _loadingProgressBar.fillAmount += 0.1f;
    }
    
    private void PulseTextTween(TextMeshProUGUI textMeshProUGUI)
    {
        Sequence sequence = DOTween.Sequence();
        RectTransform rectTransform = textMeshProUGUI.GetComponent<RectTransform>();

        sequence.Append(rectTransform.DOScale(0.85f,0.5f))
            .SetEase(Ease.InBounce)
            .Append(rectTransform.DOScale(1,0.5f))
            .SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
    }
}