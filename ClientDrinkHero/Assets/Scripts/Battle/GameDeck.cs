using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class GameDeck {
    [SerializeField] private List<CardDatabase> _scrappedCardList;
    [SerializeField] private List<CardDatabase> _remainingCardList;
    [SerializeField] private Deck _deck;


    public GameDeck(Deck deck) {
        _scrappedCardList = new List<CardDatabase>();
        _remainingCardList = new List<CardDatabase>();
        _deck = deck;
        foreach (HeroSlot heroSlot in _deck.HeroSlotList) {
            foreach (CardDatabase card in heroSlot.Hero.CardList) {
                _remainingCardList.Add(card);
            }
        }
    }

    public GameDeck() {

        _scrappedCardList = new List<CardDatabase>();
        _remainingCardList = new List<CardDatabase>();
    }

    public Deck Deck {
        get {
            return _deck;
        }
        set {

            _deck = value;
            RecreateDeck();

        }

    }


    public void ScrapCard(CardDatabase card) {
        _scrappedCardList.Add(card);
    }

    public CardDatabase DrawCard() {
        if (_remainingCardList.Count == 0 && _scrappedCardList.Count == 0) {
            return null;
        }
        int i = _remainingCardList.Count;

        if (i == 0) {
            foreach (CardDatabase card in _scrappedCardList) {
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


        CardDatabase returnCard = _remainingCardList[i];
        _remainingCardList.RemoveAt(i);

        return returnCard;
    }

    public void RecreateDeck() {
        _scrappedCardList = new List<CardDatabase>();
        _remainingCardList = new List<CardDatabase>();
        foreach (HeroSlot heroSlot in _deck.HeroSlotList) {
            foreach (CardDatabase card in heroSlot.Hero.CardList) {
                _remainingCardList.Add(card);
            }
        }
    }



}
