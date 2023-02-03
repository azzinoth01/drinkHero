using System;
using UnityEngine;

public class CharacterCardPreviewController : MonoBehaviour
{
    [SerializeField] private CharacterCardPreview[] _cardPreviews;

    private void Awake()
    {
        foreach (var preview in _cardPreviews)
        {
            preview.OnZoomIn += HandleZoom;
        }
    }

    private void OnDestroy()
    {
        foreach (var preview in _cardPreviews)
        {
            preview.OnZoomIn -= HandleZoom;
        }
    }

    public void HandleZoom(CharacterCardPreview cardPreview)
    {
        foreach (var preview in _cardPreviews)
        {
            if (preview != cardPreview)
            {
                preview.ZoomOut();
            }
        }
    }
}
