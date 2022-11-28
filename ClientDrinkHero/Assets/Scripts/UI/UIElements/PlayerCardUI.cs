using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardUI : MonoBehaviour {

    //removed card object from calls because it was not needed
    //added interface abstraction for card values

    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _shieldText;
    [SerializeField] private SimpleAudioEvent _clickOnCardSound;

    private Image _cardImage;
    private int _id;
    private Sprite _sprite;

    public void SetDisplayValues(ICardDisplay card) {
        if (card == null) {
            return;
        }

        _sprite = card.SpriteDisplay();
        _costText.SetText(card.CostText());



        _attackText.SetText(card.AttackText());


        _shieldText.SetText(card.ShieldText());


        _healthText.SetText(card.HealthText());
    }


    public void ClickCard() {
        GlobalAudioManager.Instance.Play(_clickOnCardSound);
    }
}
