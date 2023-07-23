using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour {
    [Header("Text Labels")]
    [SerializeField]
    private TextMeshProUGUI costText;

    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;

    [Header("Card Zoom Values")]
    [SerializeField]
    private Ease dragScaleEaseMode;

    [SerializeField] private float dragScaleFactor = 1.5f;
    [SerializeField] private float dragScaleDuration = 0.25f;
    private IAssetLoader _loadCardSprite;

    [Header("Card Return Movement Values")]
    [SerializeField]
    private float returnMoveDuration;

    [SerializeField] private Ease returnMoveEaseMode;

    private int _handIndex;



    private Image[] _images;

    private Sprite _cardSprite;
    private Sprite _cardLevelBorder;

    private RectTransform _cardTransform;
    private Vector2 _lastAnchoredPosition;

    private Transform _dragParent;
    private Transform _baseParent;


    private ICardDisplay _cardDisplay;

    public ICardDisplay CardDisplay {
        get {
            return _cardDisplay;
        }
    }

    public int HandIndex {
        get {
            return _handIndex;
        }

        set {
            _handIndex = value;
        }
    }

    private void Awake() {
        _cardTransform = GetComponent<RectTransform>();
        _baseParent = _cardTransform.parent;
        _dragParent = _baseParent.parent;

        _loadCardSprite = GetComponent<IAssetLoader>();

        _images = GetComponentsInChildren<Image>();


    }

    private void OnDisable() {
        ResetCardView();
        ResetCardViewParent();
    }

    public void InitializePosition(Vector2 position) {
        _lastAnchoredPosition = position;
        Debug.Log($"Card Position: {_lastAnchoredPosition}");
    }

    public void SetDisplayValues(ICardDisplay card, int index) {
        _handIndex = index;
        _cardDisplay = card;
        if (card == null) {
            return;

        }

        Debug.Log(card.GetIconPath());
        _loadCardSprite.LoadNewSprite(card.GetIconPath());
        costText.SetText(card.CostText());
        cardDescription.SetText(card.CardText());
        cardName.SetText(card.CardName());


    }

    public void ClickCardSound() {
        //PlayCardSoundEffect(clickOnCardSound);
    }


    public void DisableAllRayCastTargets() {
        foreach (var target in _images)
            target.raycastTarget = false;
    }

    public void EnableAllRaycastTargets() {
        foreach (var target in _images)
            target.raycastTarget = true;
    }

    public void ReturnCardToHand() {

        _cardTransform.anchoredPosition = _lastAnchoredPosition;
    }

    public void ZoomIn() {
        DisableAllRayCastTargets();
        transform.DOScale(Vector3.one * dragScaleFactor, dragScaleDuration).SetEase(dragScaleEaseMode);
    }

    public void ZoomOut() {
        EnableAllRaycastTargets();
        transform.DOScale(Vector3.one, dragScaleDuration).SetEase(dragScaleEaseMode);
    }

    public void ResetCardView() {
        _cardTransform.localScale = Vector3.one;
        EnableAllRaycastTargets();
    }

    public void ResetCardViewParent() {
        _cardTransform.parent = _baseParent;
    }

    public void UnparentCardView() {
        _cardTransform.parent = _dragParent;
    }
}