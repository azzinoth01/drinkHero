using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardPreview : MonoBehaviour {
    [SerializeField] private int cost;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private TextMeshProUGUI cardCostText;
    [SerializeField] private Image cardPortrait;



    private Button _cardPreviewButton;

    [Header("Card Zoom Values")]
    [SerializeField]
    private Ease clickScaleEaseMode;

    [SerializeField] private float clickScaleFactor = 1.5f;
    [SerializeField] private float clickScaleDuration = 0.25f;

    [SerializeField] private Transform zoomParent;
    private Transform _baseParent;

    private bool _currentlyZoomed;
    private LoadSprite _loadSprite;

    public event Action<CharacterCardPreview> OnZoomIn;
    private void Awake() {
        _cardPreviewButton = GetComponent<Button>();
        _cardPreviewButton.onClick.AddListener(CheckZoom);

        _baseParent = transform.parent;
        _loadSprite = cardPortrait.GetComponent<LoadSprite>();
    }

    public void SetData(CardData data, string spritePath) {
        cardCostText.SetText(data.cost.ToString());
        cardName.SetText(data.name);
        cardDescription.SetText(data.description);
        //cardPortrait = data.portrait;
        if (_loadSprite == null) {
            _loadSprite = cardPortrait.GetComponent<LoadSprite>();
        }
        _loadSprite.LoadNewSprite(spritePath);
    }

    private void CheckZoom() {
        if (_currentlyZoomed) {
            ZoomOut();
        }
        else {
            ZoomIn();
        }
    }

    private void ZoomIn() {
        _currentlyZoomed = true;
        transform.parent = zoomParent;
        transform.DOScale(Vector3.one * clickScaleFactor, clickScaleDuration).SetEase(clickScaleEaseMode);

        OnZoomIn?.Invoke(this);
    }

    public void ZoomOut() {
        _currentlyZoomed = false;
        transform.parent = _baseParent;
        transform.DOScale(Vector3.one, clickScaleDuration).SetEase(clickScaleEaseMode);
    }
}