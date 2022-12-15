using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour {

    //removed card object from calls because it was not needed
    //added interface abstraction for card values

    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _shieldText;
    [SerializeField] private TextMeshProUGUI _cardText;
    
    [SerializeField] private SimpleAudioEvent _clickOnCardSound;
    
    private int _id;
    private Sprite _cardSprite;
    private Sprite _cardLevelBorder;

    public void SetDisplayValues(ICardDisplay card) {
        if (card == null) {
            return;
        }

        _cardSprite = card.SpriteDisplay();
        _costText.SetText(card.CostText());
        _attackText.SetText(card.AttackText());
        _shieldText.SetText(card.ShieldText());
        _healthText.SetText(card.HealthText());
        _cardText.SetText(card.CardText());
    }


    public void ClickCard() {
        GlobalAudioManager.Instance.Play(_clickOnCardSound);
    }
}
