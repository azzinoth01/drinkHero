using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Movement Tween Values")]
    [SerializeField]
    private float dragDampingSpeed = .05f;


    [SerializeField] private Ease returnMoveEaseMode;

    private CardView _cardView;
    private RectTransform _cardTransform;
    private Vector3 _velocity;


    private BattleView _battleView;
    public static event Action OnShowDropZone;
    public static event Action OnHideDropZone;
    private void Awake() {
        _battleView = ViewManager.Instance.GetView<BattleView>();

        _cardView = GetComponent<CardView>();
        _cardTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        OnShowDropZone?.Invoke();

        var cview = eventData.pointerDrag.GetComponent<CardView>();

        if(cview.HandIndex == -1) {
            return;
        }


        eventData.pointerDrag.gameObject.GetComponent<Image>().enabled = false;
        foreach(Transform t in eventData.pointerDrag.gameObject.transform) {
            t.gameObject.SetActive(false);
        }

        ICardDisplay cardDisplay = UIDataContainer.Instance.Player.GetHandCards().GetHandCard(cview.HandIndex);

        _battleView = ViewManager.Instance.GetView<BattleView>();
        _battleView.playerCardDummy.gameObject.SetActive(true);

        _battleView.playerCardDummy.SetDisplayValues(cardDisplay,cview.HandIndex);
        _battleView.playerCardDummy.ZoomIn();


        _cardView.ClickCardSound();
    }

    public void OnDrag(PointerEventData eventData) {
        _battleView = ViewManager.Instance.GetView<BattleView>();
        if(RectTransformUtility.ScreenPointToWorldPointInRectangle(_cardTransform,eventData.position,eventData.pressEventCamera,out var globalPointerPosition)) {
            //_cardTransform.position = Vector3.SmoothDamp(_cardTransform.position, globalPointerPosition, ref _velocity, dragDampingSpeed);

            _battleView.playerCardDummy.transform.position = Vector3.SmoothDamp(_battleView.playerCardDummy.transform.position,globalPointerPosition,ref _velocity,dragDampingSpeed);
        }

    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("END DRAG");
        OnHideDropZone?.Invoke();

        if(eventData.pointerCurrentRaycast.gameObject == null) {
            ViewManager.Instance.GetView<BattleView>().UpdateHandCards();
            OnHideDropZone?.Invoke();


            _battleView.playerCardDummy.gameObject.SetActive(false);
            _battleView.playerCardDummy.ZoomOut();
            return;
        }

        if(!eventData.pointerCurrentRaycast.gameObject.CompareTag("CardDropZone")) {
        }
        else {
            Debug.Log("Pointer Over Dropzone!");
        }
        ViewManager.Instance.GetView<BattleView>().UpdateHandCards();


        OnHideDropZone?.Invoke();

        _battleView.playerCardDummy.gameObject.SetActive(false);
        _battleView.playerCardDummy.ZoomOut();
    }
}