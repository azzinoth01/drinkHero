using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Drag Damping")]
    [SerializeField] private float dampingSpeed = 0.5f;


    private CardView _cardView;
    private RectTransform _cardTransform;
    private Vector3 _initialPosition;
    private Vector3 _velocity;

    private void Awake()
    {
        _cardView = GetComponent<CardView>();
        
        _cardTransform = transform as RectTransform;
        _initialPosition = _cardTransform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _cardView.ZoomIn();
        _cardView.DisableAllRayCastTargets();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_cardTransform, eventData.position,
                eventData.pressEventCamera, out var globalPointerPosition))
        {
            _cardTransform.position = Vector3.SmoothDamp(_cardTransform.position, globalPointerPosition,
                ref _velocity, dampingSpeed);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _cardView.ZoomOut();
        _cardView.EnableAllRaycastTargets();
    }

    private void ReturnCardToHand()
    {
        
    }
}