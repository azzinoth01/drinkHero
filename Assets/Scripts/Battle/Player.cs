using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : Character {
    [SerializeField] private string _name;

    [SerializeField] private int _attack;
    [SerializeField] private int _maxRessource;
    [SerializeField] private int _ressource;

    [SerializeField] private List<Card> _handCards;
    public List<Card> HandCards => _handCards;

    [SerializeField] private GameDeck _gameDeck;

    public int PlayerEnergy => _ressource;
    public int PlayerMaxEnergy => _maxRessource;

    public int PlayerShield => _shield;

    public static event Action<float, float> updatePlayerHealthUI;
    public static event Action<float, float> updatePlayerEnergyUI;
    public static event Action<int> updatePlayerShieldUI;
    public static event Action updateHandCardUI;

    public static event Action playerDamageReceived, playerDamageBlocked, playerHealed, playerShieldUp;

    public Player(GameDeck gameDeck) {
        _gameDeck = gameDeck;
        _handCards = new List<Card>();


        //define max handcards globaly
        for (int i = 0; i < 5;) {

            _handCards.Add(_gameDeck.DrawCard());

            i = i + 1;
        }

        foreach (HeroSlot heroSlot in _gameDeck.Deck.HeroSlotList) {
            _maxHealth = _maxHealth + heroSlot.Hero.Health;
        }
        _health = _maxHealth;
        //define max base ressources globaly

        _maxRessource = 10;
        _ressource = 10;
    }

    public void ResetRessource() {
        _ressource = _maxRessource;
        UpdateEnergyUI();
    }

    public void StartTurn() {
        //draw until 5 cards

        for (int i = _handCards.Count; i < 5;) {
            _handCards.Add(_gameDeck.DrawCard());
            i = i + 1;
        }
        updateHandCardUI?.Invoke();
        ResetRessource();
    }

    public void PlayHandCard(int index)
    {
        int lastShield = _shield; 
        
        Card card = _handCards[index];

        if (card.Costs > _ressource) {
            return;
        }

        GlobalGameInfos.Instance.SendDataToServer(card);

        _ressource -= card.Costs;

        GlobalGameInfos.Instance.EnemyObject.enemy.TakeDmg(card.Attack);


        if (_shield + card.Shield < 0) {
            _shield = 0;
        }
        else {
            _shield += card.Shield;
            
            if(lastShield < _shield) playerShieldUp?.Invoke();
        }

        if(card.Health > 0) Heal(card.Health);
        
        _gameDeck.ScrapCard(card);
        _handCards.RemoveAt(index);

        UpdateHealthUI();
        UpdateEnergyUI();
        UpdateShieldUI();
    }

    public void Heal(int amount)
    {
        if (_health + amount > _maxHealth) _health = _maxHealth;
        else _health += amount;
        
        playerHealed?.Invoke();
    }

    public void TakeDmg(int dmg)
    {
        int lastHealth = _health;
        
        if (_shield > 0) {
            if (_shield > dmg) {
                _shield = _shield - dmg;
                dmg = 0;
            }
            else {
                dmg = dmg - _shield;
                _shield = 0;
            }
            
            playerDamageBlocked?.Invoke();
            UpdateShieldUI();
        }

        if (_health - dmg < 0) {
            _health = 0;
        }
        else {
            _health -= dmg;
        }

        if(_health < lastHealth) playerDamageReceived?.Invoke();
        
        UpdateHealthUI();
        
        if (_health <= 0) {

            PlayerDeath();
        }
    }

    public void PlayerDeath() {

    }

    private void UpdateHealthUI() {
        updatePlayerHealthUI?.Invoke(_health, _maxHealth);
    }

    private void UpdateEnergyUI() {
        updatePlayerEnergyUI?.Invoke(_ressource, _maxRessource);
    }

    private void UpdateShieldUI() {
        updatePlayerShieldUI?.Invoke(_shield);
    }
}
