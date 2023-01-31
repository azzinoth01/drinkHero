using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Movement Tween Values")] [SerializeField]
    private float dragDampingSpeed = .05f;

    [SerializeField] private float returnMoveDuration = 0.25f;
    [SerializeField] private Ease returnMoveEaseMode;

    private CardView _cardView;
    private RectTransform _cardTransform;
    private Vector3 _velocity;

    private Transform _handCardContainer;
    private Transform _battleView;

    private Vector2 _initialAnchoredPosition;
    private bool _firstDrag;

    private void Awake()
    {
        _cardView = GetComponent<CardView>();
        _cardTransform = GetComponent<RectTransform>();
        _handCardContainer = _cardTransform.parent;
        _battleView = _handCardContainer.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_firstDrag)
        {
            _initialAnchoredPosition = _cardTransform.anchoredPosition;
            _cardView.InitializePosition(_initialAnchoredPosition);
            _firstDrag = true;
        }

        _cardView.UnparentCardView();
        _cardView.ZoomIn();

        _cardView.ClickCardSound();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_cardTransform, eventData.position,
                eventData.pressEventCamera, out var globalPointerPosition))
            _cardTransform.position = Vector3.SmoothDamp(_cardTransform.position, globalPointerPosition,
                ref _velocity, dragDampingSpeed);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!eventData.pointerCurrentRaycast.gameObject.CompareTag("CardDropZone"))
            _cardView.ReturnCardToHand();
        else
            Debug.Log("Pointer Over Dropzone!");

        _cardView.ZoomOut();
        _cardView.ResetCardViewParent();
    }
}