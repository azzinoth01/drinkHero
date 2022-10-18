using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] private GameObject _playerCardUIPrefab;
    [SerializeField] private GameObject _playerHandUI;

    [SerializeField] private List<PlayerCardUI> _currentPlayerHand;
    [SerializeField] private TextMeshProUGUI _playerHealthLabelText, _playerEnergyLabelText, _enemyHealthLabelText, 
        _debugText, _playerShieldCount, _enemyShieldCount;
    [SerializeField] private Image _playerHealthBar, _playerEnergyBar, _enemyHealthBar;

    [SerializeField] private Button _endTurnButton;

    private void OnEnable() {
        TurnManager.togglePlayerUiControls += TogglePlayerUIControls;
        TurnManager.updateDebugText += UpdateDebugText;
        Player.updatePlayerHealthUI += UpdatePlayerHealthBar;
        Player.updatePlayerEnergyUI += UpdatePlayerEnergyBar;
        Player.updateHandCardUI += UpdateHandCards;
        Player.updatePlayerShieldUI += UpdatePlayerShieldCounter;
        Enemy.updateEnemyHealthUI += UpdateEnemyHealthBar;
        Enemy.updateEnemyShieldUI += UpdateEnemyShieldCounter;
    }

    private void OnDisable() {
        TurnManager.togglePlayerUiControls -= TogglePlayerUIControls;
        TurnManager.updateDebugText -= UpdateDebugText;
        Player.updatePlayerHealthUI -= UpdatePlayerHealthBar;
        Player.updatePlayerEnergyUI -= UpdatePlayerEnergyBar;
        Player.updateHandCardUI -= UpdateHandCards;
        Player.updatePlayerShieldUI -= UpdatePlayerShieldCounter;
        Enemy.updateEnemyHealthUI -= UpdateEnemyHealthBar;
        Enemy.updateEnemyShieldUI -= UpdateEnemyShieldCounter;
    }

    void Start() {
        
        UpdateHandCards();
        InitUIValues();
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
        for (i = 0; i < GlobalGameInfos.Instance.PlayerObject.Player.HandCards.Count;) {
            Card card = GlobalGameInfos.Instance.PlayerObject.Player.HandCards[i];
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

        GlobalGameInfos.Instance.PlayerObject.Player.PlayHandCard(index);

        UpdateHandCards();
    }

    private void InitUIValues() {
        UpdatePlayerHealthBar(GlobalGameInfos.Instance.PlayerObject.Player.PlayerHealth, GlobalGameInfos.Instance.PlayerObject.Player.PlayerMaxHealth);
        UpdatePlayerEnergyBar(GlobalGameInfos.Instance.PlayerObject.Player.PlayerEnergy, GlobalGameInfos.Instance.PlayerObject.Player.PlayerMaxEnergy);
        UpdatePlayerShieldCounter(GlobalGameInfos.Instance.PlayerObject.Player.PlayerShield);
        
        UpdateEnemyHealthBar(GlobalGameInfos.Instance.EnemyObject.enemy.EnemyHealth, GlobalGameInfos.Instance.EnemyObject.enemy.EnemyMaxHealth);
        UpdateEnemyShieldCounter(GlobalGameInfos.Instance.EnemyObject.enemy.EnemyShield);
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

    private static void UpdateBarDisplay(float currentValue, float maxValue, TextMeshProUGUI label, Image bar) 
    {
        Debug.Log($"Bar Display Updated. Current Value: {currentValue} MaxValue {maxValue}");
        label.SetText(currentValue.ToString());
        bar.fillAmount = currentValue / maxValue;
    }

    private void UpdateEnemyShieldCounter(int value)
    {
        UpdateShieldCounterDisplay(_enemyShieldCount, value);    
    }
    
    private void UpdatePlayerShieldCounter(int value)
    {
        UpdateShieldCounterDisplay(_playerShieldCount, value);    
    }
    
    private void UpdateShieldCounterDisplay(TextMeshProUGUI counterText, int value)
    {
        counterText.SetText(value.ToString());   
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