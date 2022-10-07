using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameDeck {
    [SerializeField] private List<Card> _scrapedCardList;
    [SerializeField] private List<Card> _remainingCardList;


    GameDeck(Deck deck) {
        _scrapedCardList = new List<Card>();
        _remainingCardList = new List<Card>();

        foreach (HeroSlot heroSlot in deck.HeroSlotList) {
            foreach (Card card in heroSlot.Hero.CardList) {
                _remainingCardList.Add(card);
            }
        }
    }
}
