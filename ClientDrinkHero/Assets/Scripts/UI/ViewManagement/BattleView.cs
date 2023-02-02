using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleView : View {
    [SerializeField] private List<CardView> currentPlayerHand;

    [Header("Card Related")]
    [SerializeField]
    private GameObject playerCardObjectPrefab;

    [SerializeField] private GameObject playerHandContainer;
    [SerializeField] private GameObject waitingForConnectionPanel;

    [Header("Player Related")]
    [SerializeField]
    private Image playerHealthBar;

    [SerializeField] private Image playerManaBar;
    [SerializeField] private TextMeshProUGUI playerHealthLabelText;
    [SerializeField] private TextMeshProUGUI playerManaLabelText;
    [SerializeField] private TextMeshProUGUI playerShieldCountText;

    [Header("Enemy Related")]
    [SerializeField]
    private Image enemyHealthBar;

    [SerializeField] private TextMeshProUGUI enemyHealthLabelText;
    [SerializeField] private TextMeshProUGUI enemyShieldCountText;

    [Header("Buttons")][SerializeField] private Button endTurnButton;
    [SerializeField] private Button optionsMenuButton;
    [SerializeField] private Button pauseMenuButton;

    [Header("Debug Related")]
    [SerializeField]
    private TextMeshProUGUI turnAnnouncerText;

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
        TurnManager.updateDebugText += UpdateTurnAnnouncer;
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
        TurnManager.updateDebugText -= UpdateTurnAnnouncer;
    }

    private void Start() {
        UpdateHandCards();
        InitUIValues();
    }

    private void AddHandCard(ICardDisplay card, int index) {
        var newCard = Instantiate(playerCardObjectPrefab, playerHandContainer.transform.position,
            Quaternion.identity, playerHandContainer.transform);

        Image image = newCard.GetComponent<Image>();
        Material materialPrefab = image.material;

        image.material = Instantiate<Material>(materialPrefab);

        var cardView = newCard.GetComponent<CardView>();

        currentPlayerHand.Add(cardView);

        cardView.SetDisplayValues(card, index);

        DisolveCard disolveCard = cardView.GetComponent<DisolveCard>();
        disolveCard.ResetEffect();
    }

    [ContextMenu("Update Hand cards")]
    private void UpdateHandCards() {
        var playerHand = UIDataContainer.Instance.Player.GetHandCards();

        if (playerHand == null)
            return;

        int i;
        for (i = 0; i < playerHand.HandCardCount();) {
            var card = playerHand.GetHandCard(i);

            if (currentPlayerHand.Count == i) {
                AddHandCard(card, i);
            }
            else {
                currentPlayerHand[i].gameObject.SetActive(true);
                currentPlayerHand[i].GetComponent<CardView>().SetDisplayValues(card, i);

                DisolveCard disolveCard = currentPlayerHand[i].GetComponent<DisolveCard>();
                disolveCard.enabled = false;
                disolveCard.ResetEffect();
            }

            var index = i;
            var button = currentPlayerHand[i].GetComponent<Button>();
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(delegate {
                CardClickEvent(index, playerHand);
            });

            i = i + 1;
        }

        for (; i < currentPlayerHand.Count;) {
            var button = currentPlayerHand[i].GetComponent<Button>();
            button.onClick.RemoveAllListeners();



            DisolveCard disolveCard = currentPlayerHand[i].GetComponent<DisolveCard>();
            disolveCard.enabled = true;
            Debug.Log("Disolve Card");
            //currentPlayerHand[i].gameObject.SetActive(false);

            i = i + 1;
        }
    }

    private void CardClickEvent(int index, IHandCards playerHand) {
        playerHand.PlayHandCard(index);
        UpdateHandCards();
    }

    public bool PlayHandCardOnDrop(int index) {
        var playerHand = UIDataContainer.Instance.Player.GetHandCards();

        var cardWasPlayed = playerHand.PlayHandCard(index);
        if (cardWasPlayed) {
            //currentPlayerHand[index].gameObject.SetActive(false);
            var button = currentPlayerHand[index].GetComponent<Button>();
            button.onClick.RemoveAllListeners();

            DisolveCard disolveCard = currentPlayerHand[index].GetComponent<DisolveCard>();
            disolveCard.enabled = true;
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
        var player = UIDataContainer.Instance.Player;
        UpdateBarDisplay(player.CurrentRessource(), player.MaxRessource(), playerManaLabelText, playerManaBar);
    }

    private void UpdateEnemyHealthBar(int deltaValue) {
        var character = UIDataContainer.Instance.Enemy;
        UpdateBarDisplay(character.CurrentHealth(), character.MaxHealth(), enemyHealthLabelText, enemyHealthBar);
    }

    private static void UpdateBarDisplay(float currentValue, float maxValue, TextMeshProUGUI label, Image bar) {
        label.SetText(currentValue.ToString());
        bar.fillAmount = currentValue / maxValue;
    }

    private void UpdateEnemyShieldCounter(int deltaValue) {
        var character = UIDataContainer.Instance.Enemy;
        UpdateShieldCounterDisplay(enemyShieldCountText, character.CurrentShield());
    }

    private void UpdatePlayerShieldCounter(int deltaValue) {
        ICharacter character = UIDataContainer.Instance.Player;
        UpdateShieldCounterDisplay(playerShieldCountText, character.CurrentShield());
    }

    private void UpdateShieldCounterDisplay(TextMeshProUGUI counterText, int value) {
        if (counterText != null)
            counterText.SetText(value.ToString());
    }

    private void UpdateTurnAnnouncer(string text) {
        turnAnnouncerText.SetText(text);
        
        Sequence sequence = DOTween.Sequence();
        RectTransform rectTransform = turnAnnouncerText.GetComponent<RectTransform>();

        sequence.Append(rectTransform.DOScale(0.85f,0.5f))
            .SetEase(Ease.InBounce)
            .Append(rectTransform.DOScale(1,0.5f))
            .SetEase(Ease.OutSine).OnComplete(() => turnAnnouncerText.SetText(""));
    }

    private void TogglePlayerUIControls(bool state) {
        // get all cards currently held and toggle their state 
        foreach (var cardButton in currentPlayerHand)
            cardButton.GetComponent<Button>().interactable = state;

        endTurnButton.interactable = state;
    }

    private void ShowGameOverScreen() {
        ViewManager.Show<GameOverView>();
    }

    public override void Initialize() {
        optionsMenuButton.onClick.AddListener(() => ViewManager.Show<OptionsMenuView>());
        pauseMenuButton.onClick.AddListener(() => ViewManager.Show<PauseMenuView>());
        
        AudioController.Instance.PlayAudio(AudioType.BattleTheme, true, 0f);
    }
}