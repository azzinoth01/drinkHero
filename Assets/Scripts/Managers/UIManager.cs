using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerObject _playerObject;
    private Player _player;

    [SerializeField] private EnemyObject _enemyObject;
    private Enemy _enemy;
    
    [SerializeField] private GameObject _playerCardUIPrefab;
    [SerializeField] private GameObject _playerHandUI;

    [SerializeField] private List<PlayerCardUI> _currentPlayerHand;
    [SerializeField] private TextMeshProUGUI _playerHealthLabelText, _playerEnergyLabelText, _enemyHealthLabelText;
    
    void Start()
    {
        _player = _playerObject.PlayerReference;
        _enemy = _enemyObject.enemy;
        
        GetHandCards();
        UpdateUIValues();
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

    private void UpdateUIValues()
    {
        _playerHealthLabelText.SetText(_player.PlayerHealth.ToString());
        _playerEnergyLabelText.SetText(_player.PlayerEnergy.ToString());
        _enemyHealthLabelText.SetText(_enemy.EnemyHealth.ToString());
    }
}