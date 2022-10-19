using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player {
    [SerializeField] private string _name;
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _shield;
    [SerializeField] private int _attack;
    [SerializeField] private int _maxRessource;
    [SerializeField] private int _ressource;

    [SerializeField] private List<Card> _handCards;
    public List<Card> HandCards => _handCards;

    [SerializeField] private GameDeck _gameDeck;

    public int PlayerHealth => _health;
    public int PlayerMaxHealth => _maxHealth;
    public int PlayerEnergy => _ressource;
    public int PlayerMaxEnergy => _maxRessource;
    
    public int PlayerShield => _shield;

    public static event Action<float, float> updatePlayerHealthUI;
    public static event Action<float, float> updatePlayerEnergyUI;
    public static event Action<int> updatePlayerShieldUI;
    public static event Action updateHandCardUI;

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

    public void PlayHandCard(int index) {

        Card card = _handCards[index];

        if (card.Costs > _ressource) {
            return;
        }
        _ressource = _ressource - card.Costs;
        
        GlobalGameInfos.Instance.EnemyObject.enemy.TakeDmg(card.Attack);

        // if (_health + card.Health >= _maxHealth)
        // {
        //     _health = _maxHealth;
        // }
        // else if (_health + card.Health <= _maxHealth)
        // {
        //     _health += card.Health;
        // }

        if (_shield + card.Shield < 0)
        {
            _shield = 0;
        }
        else
        {
            _shield += card.Shield;
        }
        
        _gameDeck.ScrapCard(card);
        _handCards.RemoveAt(index);

        UpdateHealthUI();
        UpdateEnergyUI();
        UpdateShieldUI();
    }


    public void TakeDmg(int dmg) {

        if (_shield > 0) {
            if (_shield > dmg) {
                _shield = _shield - dmg;
                dmg = 0;
                
                // Update Shield Counter UI
            }
            else {
                dmg = dmg - _shield;
                _shield = 0;
                
                // Update Shield Counter UI
            }
            UpdateShieldUI();
        }

        if (_health - dmg < 0)
        {
            _health = 0;
        }
        else
        {
            _health -= dmg;
        }

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

    private void UpdateShieldUI()
    {
        updatePlayerShieldUI?.Invoke(_shield);
    }
}
