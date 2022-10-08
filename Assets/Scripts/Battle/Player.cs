using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player {
    [SerializeField] private string _name;
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _schild;
    [SerializeField] private int _attack;
    [SerializeField] private int _maxRessource;
    [SerializeField] private int _ressource;

    [SerializeField] private List<Card> _handCards;
    public List<Card> HandCards => _handCards;
    
    [SerializeField] private GameDeck _gameDeck;
    [SerializeField] private Sprite _sprite;
    
    public Enemy enemy;

    public int PlayerHealth => _health;
    public int PlayerMaxHealth => _maxHealth;
    public int PlayerEnergy => _ressource;
    public int PlayerMaxEnergy => _maxRessource;

    public static event Action<float, float> updatePlayerHealthUI; 
    public static event Action<float, float> updatePlayerEnergyUI;
    
    public Player(GameDeck gameDeck) {
        _gameDeck = gameDeck;
        _handCards = new List<Card>();


        //define max handcards globaly
        for (int i = 0; i < 5;) {

            _handCards.Add(_gameDeck.DrawCard());

            i = i + 1;
        }

        foreach (HeroSlot heroSlot in _gameDeck.Deck.HeroSlotList) {
            _health = _health + heroSlot.Hero.Health;
        }

        //define max base ressources globaly

        _maxRessource = 10;
        _ressource = 10;
    }
    public void ResetRessource() {
        _ressource = _maxRessource;
        UpdateEnergyUI();
    }

    public void StartTurn() {
        // Check for # of hand cards > 5, if true dont draw
        if (_handCards.Count < 5)
        {
            _handCards.Add(_gameDeck.DrawCard());
        }
        ResetRessource();
    }

    public void PlayHandCard(int index) {
        
        Card card = _handCards[index];

        if (card.Costs > _ressource) {
            return;
        }
        _ressource = _ressource - card.Costs;

        UpdateEnergyUI();
        
        _handCards.RemoveAt(index);
        
        // Update Card Deck UI
        // replace with Global enemy
        
        enemy.TakeDmg(card.Attack);

        _health = _health + card.Health;
        _schild = _schild + card.Schild;
    }


    public void TakeDmg(int dmg) {

        if (_schild > 0) {
            if (_schild > dmg) {
                _schild = _schild - dmg;
                dmg = 0;
            }
            else {
                dmg = dmg - _schild;
                _schild = 0;
            }
        }

        _health = _health - dmg;

        UpdateHealthUI();

        if (_health <= 0) {

            PlayerDeath();
        }
    }

    public void PlayerDeath() {

    }

    private void UpdateHealthUI()
    {
        updatePlayerHealthUI?.Invoke(_health, _maxHealth);
    }

    private void UpdateEnergyUI()
    {
        updatePlayerEnergyUI?.Invoke(_ressource, _maxRessource);
    }
    
    
}
