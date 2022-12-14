using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [Header("Drag Damping")]
    [SerializeField] private float dampingSpeed = 0.5f;

    [Header("Drag Scaling")] 
    [SerializeField] private Ease dragScaleEaseMode;
    [SerializeField] private float dragScaleFactor = 1.5f;
    [SerializeField] private float dragScaleDuration = 0.25f;

    private RectTransform _cardTransform;
    private Vector3 _initialPosition;
    private Vector3 _velocity;

    private void OnDisable()
    {
        _cardTransform.transform.localScale = Vector3.one;
    }

    private void Awake()
    {
        _cardTransform = transform as RectTransform;
        _initialPosition = _cardTransform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _cardTransform.DOScale(Vector3.one * dragScaleFactor, dragScaleDuration).SetEase(dragScaleEaseMode);
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
        _cardTransform.DOScale(Vector3.one, dragScaleDuration).SetEase(dragScaleEaseMode);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Card dropped.");
    }
}