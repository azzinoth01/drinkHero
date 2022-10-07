using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player {
    [SerializeField] private string _name;
    [SerializeField] private int _health;
    [SerializeField] private int _schild;
    [SerializeField] private int _attack;
    [SerializeField] private int _maxRessource;
    [SerializeField] private int _ressource;
    [SerializeField] private int _maxHandCards;
    [SerializeField] private List<Card> _handCards;
    [SerializeField] private GameDeck _gameDeck;
    [SerializeField] private Sprite _sprite;


    public Player(GameDeck gameDeck) {
        _gameDeck = gameDeck;
        _handCards = new List<Card>();

        for (int i = 0; i < _maxHandCards;) {

            _handCards.Add(_gameDeck.DrawCard());

            i = i + 1;
        }

        foreach (HeroSlot heroSlot in _gameDeck.Deck.HeroSlotList) {
            _health = _health + heroSlot.Hero.Health;
        }
    }
    public void ResetRessource() {
        _ressource = _maxRessource;
    }

    public void StartTurn() {
        _handCards.Add(_gameDeck.DrawCard());
        ResetRessource();
    }

    public void PlayHandCard(int index) {
        Card card = _handCards[index];
        _handCards.RemoveAt(index);

        //replace with Global enemy
        Enemy e = new Enemy();


        e.TakeDmg(card.Attack);

        _health = _health + card.Health;
        _schild = _schild + card.Schild;
    }
}
