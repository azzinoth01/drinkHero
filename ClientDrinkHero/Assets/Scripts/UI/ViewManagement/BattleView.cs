using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleView : View
{
    [SerializeField] private List<CardView> currentPlayerHand;

    [Header("Card Related")] 
    [SerializeField] private GameObject playerCardObjectPrefab;
    [SerializeField] private GameObject playerHandContainer; 
    [SerializeField] private GameObject waitingForConnectionPanel;

    [Header("Player Related")] 
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image playerManaBar; 
    [SerializeField] private TextMeshProUGUI playerHealthLabelText;
    [SerializeField] private TextMeshProUGUI playerManaLabelText;
    [SerializeField] private TextMeshProUGUI playerShieldCountText; 
    
    [Header("Enemy Related")] 
    [SerializeField] private Image enemyHealthBar;
    [SerializeField] private TextMeshProUGUI enemyHealthLabelText;
    [SerializeField] private TextMeshProUGUI enemyShieldCountText;

    [Header("Buttons")] 
    [SerializeField] private Button endTurnButton;
    [SerializeField] private Button optionsMenuButton; 
    [SerializeField] private Button pauseMenuButton;
    
    [Header("Debug Related")]
    [SerializeField] private TextMeshProUGUI debugText;

    // TODO: Observer Pattern?
    private void OnEnable() {

        UIDataContainer.Instance.Player.HealthChange += UpdatePlayerHealthBar;
        UIDataContainer.Instance.Player.ShieldChange += UpdatePlayerShieldCounter;
        UIDataContainer.Instance.Player.RessourceChange += UpdatePlayerEnergyBar;
        UIDataContainer.Instance.Player.UpdateHandCards += UpdateHandCards;
        UIDataContainer.Instance.Player.GameOverEvent += ShowGameOverScreen;

        UIDataContainer.Instance.Enemy.HealthChange += UpdateEnemyHealthBar;
        UIDataContainer.Instance.Enemy.ShieldChange += UpdateEnemyShieldCounter;

        //UIDataContainer.Instance.WaitingPanel.DisplayWaitingPanel += ToggleWaitingPanel;
        
        TurnManager.togglePlayerUiControls += TogglePlayerUIControls;
        TurnManager.updateDebugText += UpdateDebugText;
    }

    private void OnDisable() {
        
        UIDataContainer.Instance.Player.HealthChange -= UpdatePlayerHealthBar;
        UIDataContainer.Instance.Player.ShieldChange -= UpdatePlayerShieldCounter;
        UIDataContainer.Instance.Player.RessourceChange -= UpdatePlayerEnergyBar;
        UIDataContainer.Instance.Player.UpdateHandCards -= UpdateHandCards;
        UIDataContainer.Instance.Player.GameOverEvent -= ShowGameOverScreen;

        UIDataContainer.Instance.Enemy.HealthChange -= UpdateEnemyHealthBar;
        UIDataContainer.Instance.Enemy.ShieldChange -= UpdateEnemyShieldCounter;

        //UIDataContainer.Instance.WaitingPanel.DisplayWaitingPanel -= ToggleWaitingPanel;
        
        TurnManager.togglePlayerUiControls -= TogglePlayerUIControls;
        TurnManager.updateDebugText -= UpdateDebugText;
    }

    void Start() {

        UpdateHandCards();
        InitUIValues();
    }

    private void AddHandCard(ICardDisplay card, int index) {
        var newCard = Instantiate(playerCardObjectPrefab, playerHandContainer.transform.position,
            Quaternion.identity, playerHandContainer.transform);
        var cardView = newCard.GetComponent<CardView>();

        currentPlayerHand.Add(cardView);
        
        cardView.SetDisplayValues(card, index);
    }

    private void UpdateHandCards() {
        IHandCards playerHand = UIDataContainer.Instance.Player.GetHandCards();
        
        if (playerHand == null) {
            return;
        }

        int i;
        for (i = 0; i < playerHand.HandCardCount();) {
            ICardDisplay card = playerHand.GetHandCard(i);
            
            if (currentPlayerHand.Count == i) {
                AddHandCard(card, i);
            }
            else {
                currentPlayerHand[i].gameObject.SetActive(true);
                currentPlayerHand[i].GetComponent<CardView>().SetDisplayValues(card, i);
            }

            int index = i;
            Button button = currentPlayerHand[i].GetComponent<Button>();
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(delegate {
                CardClickEvent(index, playerHand);
            });

            i = i + 1;
        }

        for (; i < currentPlayerHand.Count;) {
            currentPlayerHand[i].gameObject.SetActive(false);

            i = i + 1;
        }
    }

    private void CardClickEvent(int index, IHandCards playerHand) 
    {
        playerHand.PlayHandCard(index);
        UpdateHandCards();
    }

    public bool PlayHandCardOnDrop(int index)
    {
        IHandCards playerHand = UIDataContainer.Instance.Player.GetHandCards();
        
        bool cardWasPlayed = playerHand.PlayHandCard(index);
        if (cardWasPlayed)
        {
            currentPlayerHand[index].gameObject.SetActive(false);
            UpdateHandCards();
        }

        return cardWasPlayed;
    }
    
    private void InitUIValues() {

        UpdatePlayerHealthBar(0);
        UpdatePlayerEnergyBar(0);
        UpdatePlayerShieldCounter(0);

        UpdateEnemyHealthBar(0);
        UpdateEnemyShieldCounter(0);
    }

    private void UpdatePlayerHealthBar(int deltaValue) {
        ICharacter character = UIDataContainer.Instance.Player;
        UpdateBarDisplay(character.CurrentHealth(), character.MaxHealth(), playerHealthLabelText, playerHealthBar);
    }

    private void UpdatePlayerEnergyBar(int deltaValue) {
        IPlayer player = UIDataContainer.Instance.Player;
        UpdateBarDisplay(player.CurrentRessource(), player.MaxRessource(), playerManaLabelText, playerManaBar);
    }

    private void UpdateEnemyHealthBar(int deltaValue) {
        ICharacter character = UIDataContainer.Instance.Enemy;
        UpdateBarDisplay(character.CurrentHealth(), character.MaxHealth(), enemyHealthLabelText, enemyHealthBar);
    }

    private static void UpdateBarDisplay(float currentValue, float maxValue, TextMeshProUGUI label, Image bar) {
        label.SetText(currentValue.ToString());
        bar.fillAmount = currentValue / maxValue;
    }

    private void UpdateEnemyShieldCounter(int deltaValue) {
        ICharacter character = UIDataContainer.Instance.Enemy;
        UpdateShieldCounterDisplay(enemyShieldCountText, character.CurrentShield());
    }

    private void UpdatePlayerShieldCounter(int deltaValue) {
        ICharacter character = UIDataContainer.Instance.Player;
        UpdateShieldCounterDisplay(playerShieldCountText, character.CurrentShield());
    }

    private void UpdateShieldCounterDisplay(TextMeshProUGUI counterText, int value) {
        if (counterText != null) {
            counterText.SetText(value.ToString());
        }
    }

    private void UpdateDebugText(string text) {
        debugText.SetText(text);
    }

    private void TogglePlayerUIControls(bool state) {
        // get all cards currently held and toggle their state 
        foreach (var cardButton in currentPlayerHand) {
            cardButton.GetComponent<Button>().interactable = state;
        }

        endTurnButton.interactable = state;
    }

    private void ShowGameOverScreen() {
        ViewManager.Show<GameOverView>();
    }

    private void ToggleWaitingPanel(bool state)
    {
        // create WaitForConnectionView 
        waitingForConnectionPanel.SetActive(state);
    }

    public override void Initialize()
    { 
        optionsMenuButton.onClick.AddListener(() => ViewManager.Show<OptionsMenuView>());
        pauseMenuButton.onClick.AddListener(() => ViewManager.Show<PauseMenuView>());
    }
}
