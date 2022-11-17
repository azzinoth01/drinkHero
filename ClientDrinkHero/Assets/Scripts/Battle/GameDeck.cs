using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class GameDeck : ICascadable {
    [SerializeField] private List<Card> _scrappedCardList;
    [SerializeField] private List<Card> _remainingCardList;
    [SerializeField] private Deck _deck;
    private List<ICascadable> _cascadables;

    public GameDeck(Deck deck) {
        _scrappedCardList = new List<Card>();
        _remainingCardList = new List<Card>();
        _deck = deck;
        foreach (HeroSlot heroSlot in _deck.HeroSlotList) {
            foreach (Card card in heroSlot.Hero.CardList.Values) {
                _remainingCardList.Add(card);
            }
        }
    }

    public GameDeck() {
        _cascadables = new List<ICascadable>();
        _scrappedCardList = new List<Card>();
        _remainingCardList = new List<Card>();
    }

    public Deck Deck {
        get {
            return _deck;
        }
        set {
            if (_deck != null) {
                _deck.Cascadables.Remove(this);
            }
            _deck = value;
            if (_deck != null) {
                _deck.Cascadables.Add(this);
            }

        }

    }


    public List<ICascadable> Cascadables {
        get {
            return _cascadables;
        }

        set {
            _cascadables = value;
        }
    }

    public void ScrapCard(Card card) {
        _scrappedCardList.Add(card);
    }

    public Card DrawCard() {
        if (_remainingCardList.Count == 0 && _scrappedCardList.Count == 0) {
            return null;
        }
        int i = _remainingCardList.Count;

        if (i == 0) {
            foreach (Card card in _scrappedCardList) {
                _remainingCardList.Add(card);
            }
            _scrappedCardList.Clear();
        }

        if (_remainingCardList.Count == 0) {
            return null;
        }
        // Check for no cards left in stack, if true dont draw
        i = _remainingCardList.Count;

        i = Random.Range(0, i);


        Card returnCard = _remainingCardList[i];
        _remainingCardList.RemoveAt(i);

        return returnCard;
    }

    public void RecreateDeck() {
        _scrappedCardList = new List<Card>();
        _remainingCardList = new List<Card>();
        foreach (HeroSlot heroSlot in _deck.HeroSlotList) {
            foreach (Card card in heroSlot.Hero.CardList.Values) {
                _remainingCardList.Add(card);
            }
        }
    }



    public void Cascade(ICascadable causedBy, PropertyInfo changedProperty = null, object changedValue = null) {
        if (causedBy == null) {
            causedBy = this;
        }


        foreach (ICascadable cascadable in Cascadables) {


            cascadable.Cascade(causedBy, changedProperty, changedValue);
        }

    }
}
