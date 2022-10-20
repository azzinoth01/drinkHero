using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardUI : MonoBehaviour {
    private Card _card;

    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private TextMeshProUGUI _typeText;

    private Image _cardImage;
    private int _id;
    private Sprite _sprite;

    public void SetDisplayValues(Card card) {
        _card = card;
        _sprite = _card.Sprite;
        _costText.SetText(_card.Costs.ToString());

        if (_card.Attack != 0) {
            _typeText.SetText("Damage");
            _valueText.SetText(_card.Attack.ToString());
        }
        else if (_card.Shield != 0) {
            _typeText.SetText("Schild");
            _valueText.SetText(_card.Shield.ToString());
        }
        else if (_card.Health != 0) {
            _typeText.SetText("Health");
            _valueText.SetText(_card.Health.ToString());
        }

    }
}
