using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardUI : MonoBehaviour
{
    private Card _card;

    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _valueText;

    private Image _cardImage;
    private int _id;
    private Sprite _sprite;
    
    public void SetDisplayValues(Card card)
    {
        _card = card;
        _sprite = _card.Sprite;
        _costText.SetText(_card.Costs.ToString());
    }
}
