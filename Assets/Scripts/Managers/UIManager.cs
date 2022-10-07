using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerObject _playerObject;
    private Player _player;

    [SerializeField] private EnemyObject _enemyObject;
    private Enemy _enemy;
    
    [SerializeField] private GameObject _playerCardUIPrefab;
    [SerializeField] private GameObject _playerHandUI;

    [SerializeField] private List<PlayerCardUI> _currentPlayerHand;
    [SerializeField] private TextMeshProUGUI _playerHealthLabelText, _playerEnergyLabelText, _enemyHealthLabelText, _debugText;
    [SerializeField] private Image _playerHealthBar, _playerEnergyBar, _enemyHealthBar;

    [SerializeField] private Button _endTurnButton;

    private void OnEnable()
    {
        TurnManager.togglePlayerUiControls += TogglePlayerUIControls;
        TurnManager.updateDebugText += UpdateDebugText;
    }

    private void OnDisable()
    {
        TurnManager.togglePlayerUiControls -= TogglePlayerUIControls;
        TurnManager.updateDebugText -= UpdateDebugText;
    }

    void Start()
    {
        _player = _playerObject.PlayerReference;
        _enemy = _enemyObject.enemy;
        
        GetHandCards();
        InitUIValues();
    }

    private void GetHandCards()
    {
        var heldCards = _player.HandCards;

        foreach (var card in heldCards)
        {
            AddHandCard(card);
        }
    }
    
    private void AddHandCard(Card card)
    {            
        var newCard = Instantiate(_playerCardUIPrefab, _playerHandUI.transform.position, 
            Quaternion.identity, _playerHandUI.transform);
        var newCardUi = newCard.GetComponent<PlayerCardUI>();

        _currentPlayerHand.Add(newCardUi);  
        
        newCardUi.SetDisplayValues(card);
    }
    
    private void RemoveHandCard(PlayerCardUI card)
    {
        if(!_currentPlayerHand.Contains(card)) return;

        var cardUIGameObject = card.gameObject;
        _currentPlayerHand.Remove(card);
        Destroy(cardUIGameObject);
    }

    private void InitUIValues()
    {
        UpdatePlayerHealthBar(_player.PlayerHealth, _player.PlayerMaxHealth);
        UpdatePlayerEnergyBar(_player.PlayerEnergy, _player.PlayerMaxEnergy);
        UpdateEnemyHealthBar(_enemy.EnemyHealth, _enemy.EnemyMaxHealth);
    }

    private void UpdatePlayerHealthBar(float currentValue, float maxValue)
    {
        UpdateBarDisplay(currentValue, maxValue, _playerHealthLabelText, _playerHealthBar);
    }
    
    private void UpdatePlayerEnergyBar(float currentValue, float maxValue)
    {
        UpdateBarDisplay(currentValue, maxValue, _playerEnergyLabelText, _playerEnergyBar);
    }
    
    private void UpdateEnemyHealthBar(float currentValue, float maxValue)
    {
        UpdateBarDisplay(currentValue, maxValue, _enemyHealthLabelText, _enemyHealthBar);
    }
    
    private static void UpdateBarDisplay(float currentValue, float maxValue, TextMeshProUGUI label, Image bar)
    {
        label.SetText(currentValue.ToString());
        // TODO: 100 should be replace with a max value
        bar.fillAmount = currentValue / maxValue;
    }

    private void TogglePlayerUIControls(bool state)
    {
        // get all cards currently held and toggle their state 
        foreach (var cardButton in _currentPlayerHand)
        {
            cardButton.GetComponent<Button>().interactable = state;
        }
        
        _endTurnButton.interactable = state;
    }

    private void UpdateDebugText(string text)
    {
        _debugText.SetText(text);
    }
}