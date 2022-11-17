using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardUI : MonoBehaviour {
    private Card _card;

    [SerializeField] private TextMeshProUGUI _costText;
    //public TextMeshProUGUI CostText => _costText;

    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _shieldText;
    [SerializeField] private SimpleAudioEvent _clickOnCardSound;

    private Image _cardImage;
    private int _id;
    private Sprite _sprite;

    public void SetDisplayValues(Card card) {
        if (card == null) {
            return;
        }
        _card = card;
        _sprite = _card.Sprite;
        _costText.SetText(_card.Costs.ToString());



        _attackText.SetText(_card.Attack.ToString());


        _shieldText.SetText(_card.Shield.ToString());


        _healthText.SetText(_card.Health.ToString());
    }


    public void ClickCard() {
        GlobalAudioManager.Instance.Play(_clickOnCardSound);
    }
}
