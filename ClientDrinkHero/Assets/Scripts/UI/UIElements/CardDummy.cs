using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDummy : MonoBehaviour
{
    [Header("Text Labels")]
    [SerializeField]
    private TextMeshProUGUI costText;

    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;

    [SerializeField] private float hideAfterSeconds;

    [SerializeField] private DisolveCard _disolveCard;
    [SerializeField] private IAssetLoader _assetLoader; 
    
    private void OnEnable()
    {
        StartCoroutine(HideAfterDelay(hideAfterSeconds));
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetDummyData(string cost, string title, string description, string path)
    {
        _assetLoader.LoadNewSprite(path);
        costText.SetText(cost);
        cardName.SetText(title);
        cardDescription.SetText(description);
    }
    
    private IEnumerator HideAfterDelay(float delay)
    {
        //_disolveCard.enabled = true;
        yield return new WaitForSeconds(delay);
        Hide();
    }

    public void Show()
    {
        _disolveCard.ResetEffect();
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
