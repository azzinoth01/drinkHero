using System;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class GameDeck {
    [SerializeField] private List<Card> _scrappedCardList;
    [SerializeField] private List<Card> _remainingCardList;
    [SerializeField] private Deck _deck;

    public GameDeck(Deck deck) {
        _scrappedCardList = new List<Card>();
        _remainingCardList = new List<Card>();
        _deck = deck;
        foreach (HeroSlot heroSlot in _deck.HeroSlotList) {
            foreach (Card card in heroSlot.Hero.CardList) {
                _remainingCardList.Add(card);
            }
        }
    }

    public Deck Deck {
        get {
            return _deck;
        }


    }

    public void ScrapCard(Card card) {
        _scrappedCardList.Add(card);
    }

    public Card DrawCard() {
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
}
