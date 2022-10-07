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

    [SerializeField] private List<Card> _handCards;
    [SerializeField] private GameDeck _gameDeck;
    [SerializeField] private Sprite _sprite;


    public Enemy enemy;

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
    }

    public void StartTurn() {
        _handCards.Add(_gameDeck.DrawCard());
        ResetRessource();
    }

    public void PlayHandCard(int index) {



        Card card = _handCards[index];

        if (card.Costs > _ressource) {
            return;
        }
        _ressource = _ressource - card.Costs;
        _handCards.RemoveAt(index);

        //replace with Global enemy



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

        if (_health <= 0) {

            PlayerDeath();
        }
    }

    public void PlayerDeath() {

    }
}
