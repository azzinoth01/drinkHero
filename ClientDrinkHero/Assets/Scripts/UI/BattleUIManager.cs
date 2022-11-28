using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour {

    [SerializeField] private GameObject _playerCardUIPrefab;
    [SerializeField] private GameObject _playerHandUI;
    [SerializeField] private GameObject _playerOptionsPanel, _playerDeathPanel;

    [SerializeField] private List<PlayerCardUI> _currentPlayerHand;
    [SerializeField]
    private TextMeshProUGUI _playerHealthLabelText, _playerEnergyLabelText, _enemyHealthLabelText,
        _debugText, _playerShieldCount, _enemyShieldCount;
    [SerializeField] private Image _playerHealthBar, _playerEnergyBar, _enemyHealthBar;

    [SerializeField] private Button _endTurnButton;


    //TODO: maybe refactor..
    private void OnEnable() {

        UIDataContainer.Instance.Player.HealthChange += UpdatePlayerHealthBar;
        UIDataContainer.Instance.Player.ShieldChange += UpdatePlayerShieldCounter;
        UIDataContainer.Instance.Player.RessourceChange += UpdatePlayerEnergyBar;
        UIDataContainer.Instance.Player.UpdateHandCards += UpdateHandCards;
        UIDataContainer.Instance.Player.GameOverEvent += ShowGameOverScreen;

        UIDataContainer.Instance.Enemy.HealthChange += UpdateEnemyHealthBar;
        UIDataContainer.Instance.Enemy.ShieldChange += UpdateEnemyShieldCounter;


        TurnManager.togglePlayerUiControls += TogglePlayerUIControls;
        TurnManager.updateDebugText += UpdateDebugText;
        //Player.updatePlayerHealthUI += UpdatePlayerHealthBar;
        //Player.updatePlayerEnergyUI += UpdatePlayerEnergyBar;
        //Player.updateHandCardUI += UpdateHandCards;
        //Player.updatePlayerShieldUI += UpdatePlayerShieldCounter;
        //Player.onPlayerDeath += ShowGameOverScreen;
        //Enemy.updateEnemyHealthUI += UpdateEnemyHealthBar;
        //Enemy.updateEnemyShieldUI += UpdateEnemyShieldCounter;


    }

    private void OnDisable() {


        UIDataContainer.Instance.Player.HealthChange -= UpdatePlayerHealthBar;
        UIDataContainer.Instance.Player.ShieldChange -= UpdatePlayerShieldCounter;
        UIDataContainer.Instance.Player.RessourceChange -= UpdatePlayerEnergyBar;
        UIDataContainer.Instance.Player.UpdateHandCards -= UpdateHandCards;
        UIDataContainer.Instance.Player.GameOverEvent -= ShowGameOverScreen;

        UIDataContainer.Instance.Enemy.HealthChange -= UpdateEnemyHealthBar;
        UIDataContainer.Instance.Enemy.ShieldChange -= UpdateEnemyShieldCounter;

        TurnManager.togglePlayerUiControls -= TogglePlayerUIControls;
        TurnManager.updateDebugText -= UpdateDebugText;
        //Player.updatePlayerHealthUI -= UpdatePlayerHealthBar;
        //Player.updatePlayerEnergyUI -= UpdatePlayerEnergyBar;
        //Player.updateHandCardUI -= UpdateHandCards;
        //Player.updatePlayerShieldUI -= UpdatePlayerShieldCounter;
        //Player.onPlayerDeath -= ShowGameOverScreen;
        //Enemy.updateEnemyHealthUI -= UpdateEnemyHealthBar;
        //Enemy.updateEnemyShieldUI -= UpdateEnemyShieldCounter;


    }

    void Start() {

        UpdateHandCards();
        InitUIValues();
    }

    private void AddHandCard(ICardDisplay card) {
        var newCard = Instantiate(_playerCardUIPrefab, _playerHandUI.transform.position,
            Quaternion.identity, _playerHandUI.transform);
        var newCardUi = newCard.GetComponent<PlayerCardUI>();

        _currentPlayerHand.Add(newCardUi);

        newCardUi.SetDisplayValues(card);
    }

    private void UpdateHandCards() {
        IHandCards playerHand = UIDataContainer.Instance.Player.GetHandCards();
        if (playerHand == null) {
            return;
        }

        int i;
        for (i = 0; i < playerHand.HandCardCount();) {
            ICardDisplay card = playerHand.GetHandCard(i);
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
                CardClickEvent(index, playerHand);
            });

            i = i + 1;
        }

        for (; i < _currentPlayerHand.Count;) {
            _currentPlayerHand[i].gameObject.SetActive(false);

            i = i + 1;
        }
    }

    private void CardClickEvent(int index, IHandCards playerHand) {
        playerHand.PlayHandCard(index);


        UpdateHandCards();
    }

    private void InitUIValues() {

        UpdatePlayerHealthBar(0);
        UpdatePlayerEnergyBar(0);
        UpdatePlayerShieldCounter(0);

        UpdateEnemyHealthBar(0);
        UpdateEnemyShieldCounter(0);

        // value initialisation not needed can be done when the objects initzialies itself

        //UpdatePlayerHealthBar(GlobalGameInfos.Instance.PlayerObject.Player.Health, GlobalGameInfos.Instance.PlayerObject.Player.MaxHealth);
        //UpdatePlayerEnergyBar(GlobalGameInfos.Instance.PlayerObject.Player.PlayerEnergy, GlobalGameInfos.Instance.PlayerObject.Player.PlayerMaxEnergy);
        //UpdatePlayerShieldCounter(GlobalGameInfos.Instance.PlayerObject.Player.Shield);

        //UpdateEnemyHealthBar(GlobalGameInfos.Instance.EnemyObject.enemy.Health, GlobalGameInfos.Instance.EnemyObject.enemy.MaxHealth);
        //UpdateEnemyShieldCounter(GlobalGameInfos.Instance.EnemyObject.enemy.Shield);
    }

    private void UpdatePlayerHealthBar(int deltaValue) {
        ICharacter character = UIDataContainer.Instance.Player;
        UpdateBarDisplay(character.CurrentHealth(), character.MaxHealth(), _playerHealthLabelText, _playerHealthBar);
    }

    private void UpdatePlayerEnergyBar(int deltaValue) {
        IPlayer player = UIDataContainer.Instance.Player;
        UpdateBarDisplay(player.CurrentRessource(), player.MaxRessource(), _playerEnergyLabelText, _playerEnergyBar);
    }

    private void UpdateEnemyHealthBar(int deltaValue) {
        ICharacter character = UIDataContainer.Instance.Enemy;
        UpdateBarDisplay(character.CurrentHealth(), character.MaxHealth(), _enemyHealthLabelText, _enemyHealthBar);
    }

    private static void UpdateBarDisplay(float currentValue, float maxValue, TextMeshProUGUI label, Image bar) {
        label.SetText(currentValue.ToString());
        bar.fillAmount = currentValue / maxValue;
    }

    private void UpdateEnemyShieldCounter(int deltaValue) {
        ICharacter character = UIDataContainer.Instance.Enemy;
        UpdateShieldCounterDisplay(_enemyShieldCount, character.CurrentShield());
    }

    private void UpdatePlayerShieldCounter(int deltaValue) {
        ICharacter character = UIDataContainer.Instance.Player;
        UpdateShieldCounterDisplay(_playerShieldCount, character.CurrentShield());
    }

    private void UpdateShieldCounterDisplay(TextMeshProUGUI counterText, int value) {
        if (counterText != null) {
            counterText.SetText(value.ToString());
        }
    }

    private void TogglePlayerUIControls(bool state) {
        // get all cards currently held and toggle their state 
        foreach (var cardButton in _currentPlayerHand) {
            cardButton.GetComponent<Button>().interactable = state;
        }

        _endTurnButton.interactable = state;
    }

    private void ShowGameOverScreen() {
        _playerDeathPanel.SetActive(true);
    }

    private void UpdateDebugText(string text) {
        _debugText.SetText(text);
    }

    public void ShowOptionsPanel() {
        _playerOptionsPanel.SetActive(true);
    }

    public void HideOptionsPanel() {
        _playerOptionsPanel.SetActive(false);
    }

    public void ReturnToMainMenu() {
        SceneLoader.Load(GameSceneEnum.MainMenu);
    }
}