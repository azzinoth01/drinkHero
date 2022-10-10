using System;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class GameDeck {
    [SerializeField] private List<Card> _scrapedCardList;
    [SerializeField] private List<Card> _remainingCardList;
    [SerializeField] private Deck _deck;

    public GameDeck(Deck deck) {
        _scrapedCardList = new List<Card>();
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
        _scrapedCardList.Add(card);
    }

    public Card DrawCard() {
        int i = _remainingCardList.Count;

        if (i == 0) {
            foreach (Card card in _scrapedCardList) {
                _remainingCardList.Add(card);
            }
            _scrapedCardList.Clear();
        }

        if (_remainingCardList.Count == 0) {
            return null;
        }
        // Check for no cards left in stack, if true dont draw

        i = Random.Range(0, i);

        //Debug.Log(i);

        Card returnCard = _remainingCardList[i];
        _remainingCardList.RemoveAt(i);

        return returnCard;
    }
}
