using System;
using UnityEngine;

public class CharacterCardPreviewController : MonoBehaviour
{
    [SerializeField] private CharacterCardPreview[] _cardPreviews;

    private void Awake()
    {
        CharacterCardView.OnZoomReset += ResetZoom;
        
        foreach (var preview in _cardPreviews)
        {
            preview.OnZoomIn += HandleZoom;
        }
    }

    private void OnDestroy()
    {
        CharacterCardView.OnZoomReset -= ResetZoom;
        
        foreach (var preview in _cardPreviews)
        {
            preview.OnZoomIn -= HandleZoom;
        }
    }

    private void HandleZoom(CharacterCardPreview cardPreview)
    {
        foreach (var preview in _cardPreviews)
        {
            if (preview != cardPreview)
            {
                preview.ZoomOut();
            }
        }
    }

    private void ResetZoom()
    {
        foreach (var preview in _cardPreviews)
        {
            preview.ZoomOut();
        }
    }
}