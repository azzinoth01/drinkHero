using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    //[SerializeField] private PlayerObject _playerObject;
    //private Player _player;

    //[SerializeField] private EnemyObject _enemyObject;
    //private Enemy _enemy;

    [SerializeField] private GameObject _playerCardUIPrefab;
    [SerializeField] private GameObject _playerHandUI;

    [SerializeField] private List<PlayerCardUI> _currentPlayerHand;
    [SerializeField] private TextMeshProUGUI _playerHealthLabelText, _playerEnergyLabelText, _enemyHealthLabelText, _debugText;
    [SerializeField] private Image _playerHealthBar, _playerEnergyBar, _enemyHealthBar;

    [SerializeField] private Button _endTurnButton;

    private void OnEnable() {
        TurnManager.togglePlayerUiControls += TogglePlayerUIControls;
        TurnManager.updateDebugText += UpdateDebugText;
        Player.updatePlayerHealthUI += UpdatePlayerHealthBar;
        Player.updatePlayerEnergyUI += UpdatePlayerEnergyBar;
        Player.updateHandCardUI += UpdateHandCards;
        Enemy.updateEnemyHealthUI += UpdateEnemyHealthBar;
    }

    private void OnDisable() {
        TurnManager.togglePlayerUiControls -= TogglePlayerUIControls;
        TurnManager.updateDebugText -= UpdateDebugText;
        Player.updatePlayerHealthUI -= UpdatePlayerHealthBar;
        Player.updatePlayerEnergyUI -= UpdatePlayerEnergyBar;
        Player.updateHandCardUI -= UpdateHandCards;
        Enemy.updateEnemyHealthUI -= UpdateEnemyHealthBar;
    }

    void Start() {
        //_player = _playerObject.PlayerReference;
        //_enemy = _enemyObject.enemy;


        //_player = GlobalGameInfos.Instance.PlayerObject.PlayerReference;
        //_enemy = GlobalGameInfos.Instance.EnemyObject.enemy;

        UpdateHandCards();
        InitUIValues();
    }

    private void GetHandCards() {
        //var heldCards = GlobalGameInfos.Instance.PlayerObject.PlayerReference.HandCards;

        //foreach (var card in heldCards) {
        //    AddHandCard(card);
        //}


    }
    private void AddHandCard(Card card) {
        var newCard = Instantiate(_playerCardUIPrefab, _playerHandUI.transform.position,
            Quaternion.identity, _playerHandUI.transform);
        var newCardUi = newCard.GetComponent<PlayerCardUI>();

        _currentPlayerHand.Add(newCardUi);

        newCardUi.SetDisplayValues(card);

    }

    private void UpdateHandCards() {
        int i;
        for (i = 0; i < GlobalGameInfos.Instance.PlayerObject.PlayerReference.HandCards.Count;) {
            Card card = GlobalGameInfos.Instance.PlayerObject.PlayerReference.HandCards[i];
            if (_currentPlayerHand.Count == i) {
                AddHandCard(card);
            }
            else {
                _currentPlayerHand[i].gameObject.SetActive(true);
                _currentPlayerHand[i].GetComponent<PlayerCardUI>().SetDisplayValues(card);
            }

            int index = i;
            Button button = _currentPlayerHand[i].GetComponent<Button>();
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(delegate {
                CardClickEvent(index);
            });

            i = i + 1;
        }

        for (; i < _currentPlayerHand.Count;) {
            _currentPlayerHand[i].gameObject.SetActive(false);

            i = i + 1;
        }
    }

    private void CardClickEvent(int index) {

        GlobalGameInfos.Instance.PlayerObject.PlayerReference.PlayHandCard(index);

        UpdateHandCards();
    }

    private void RemoveHandCard(PlayerCardUI card) {
        if (!_currentPlayerHand.Contains(card))
            return;

        var cardUIGameObject = card.gameObject;
        _currentPlayerHand.Remove(card);
        Destroy(cardUIGameObject);
    }

    private void InitUIValues() {
        UpdatePlayerHealthBar(GlobalGameInfos.Instance.PlayerObject.PlayerReference.PlayerHealth, GlobalGameInfos.Instance.PlayerObject.PlayerReference.PlayerMaxHealth);
        UpdatePlayerEnergyBar(GlobalGameInfos.Instance.PlayerObject.PlayerReference.PlayerEnergy, GlobalGameInfos.Instance.PlayerObject.PlayerReference.PlayerMaxEnergy);
        UpdateEnemyHealthBar(GlobalGameInfos.Instance.EnemyObject.enemy.EnemyHealth, GlobalGameInfos.Instance.EnemyObject.enemy.EnemyMaxHealth);
    }

    private void UpdatePlayerHealthBar(float currentValue, float maxValue) {
        UpdateBarDisplay(currentValue, maxValue, _playerHealthLabelText, _playerHealthBar);
    }

    private void UpdatePlayerEnergyBar(float currentValue, float maxValue) {
        UpdateBarDisplay(currentValue, maxValue, _playerEnergyLabelText, _playerEnergyBar);
    }

    private void UpdateEnemyHealthBar(float currentValue, float maxValue) {
        UpdateBarDisplay(currentValue, maxValue, _enemyHealthLabelText, _enemyHealthBar);
    }

    private static void UpdateBarDisplay(float currentValue, float maxValue, TextMeshProUGUI label, Image bar) {
        label.SetText(currentValue.ToString());
        // TODO: 100 should be replace with a max value
        bar.fillAmount = currentValue / maxValue;
    }

    private void TogglePlayerUIControls(bool state) {
        // get all cards currently held and toggle their state 
        foreach (var cardButton in _currentPlayerHand) {
            cardButton.GetComponent<Button>().interactable = state;
        }

        _endTurnButton.interactable = state;
    }

    private void UpdateDebugText(string text) {
        _debugText.SetText(text);
    }
}