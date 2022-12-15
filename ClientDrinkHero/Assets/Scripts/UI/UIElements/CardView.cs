using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [Header("Text Labels")] 
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;

    [Header("Sound Effect(s)")] 
    [SerializeField] private SimpleAudioEvent clickOnCardSound;
    
    [Header("Card Zoom Settings")] 
    [SerializeField] private Ease dragScaleEaseMode;
    [SerializeField] private float dragScaleFactor = 1.5f;
    [SerializeField] private float dragScaleDuration = 0.25f;

    private int _handIndex;
    public int HandIndex => _handIndex;

    private Image[] _images;
    
    private Sprite _cardSprite;
    private Sprite _cardLevelBorder;

    private void Awake()
    {
        _images = GetComponentsInChildren<Image>();
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.one;
    }

    public void SetDisplayValues(ICardDisplay card, int index)
    {
        if (card == null)
        {
            return;
        }

        _cardSprite = card.SpriteDisplay();
        costText.SetText(card.CostText());
        cardDescription.SetText(card.CardText());

        _handIndex = index;
    }
    
    public void ClickCard()
    {
        GlobalAudioManager.Instance.Play(clickOnCardSound);
    }

    public void DisableAllRayCastTargets()
    {
        foreach (var target  in _images)
        {
            target.raycastTarget = false;
        }
    }

    public void EnableAllRaycastTargets()
    {
        foreach (var target  in _images)
        {
            target.raycastTarget = true;
        }
    }
    
    public void ZoomIn()
    {
        transform.DOScale(Vector3.one * dragScaleFactor, dragScaleDuration).SetEase(dragScaleEaseMode);
    }
    
    public void ZoomOut()
    {
        transform.DOScale(Vector3.one, dragScaleDuration).SetEase(dragScaleEaseMode);
    }
}