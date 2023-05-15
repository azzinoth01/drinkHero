using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class GameDeck {
    [SerializeField] private List<DeckCardContainer> _scrappedCardList;
    [SerializeField] private List<DeckCardContainer> _remainingCardList;
    [SerializeField] private Deck _deck;


    public GameDeck(Deck deck) {
        _scrappedCardList = new List<DeckCardContainer>();
        _remainingCardList = new List<DeckCardContainer>();
        _deck = deck;
        foreach (HeroSlot heroSlot in _deck.HeroSlotList) {
            foreach (CardDatabase card in heroSlot.Hero.CardList) {
                DeckCardContainer container = new DeckCardContainer(card, heroSlot.Hero);
                _remainingCardList.Add(container);
            }
        }
    }

    public GameDeck() {

        _scrappedCardList = new List<DeckCardContainer>();
        _remainingCardList = new List<DeckCardContainer>();
    }

    public Deck Deck {
        get {
            return _deck;
        }
        set {

            _deck = value;
            //RecreateDeck();

        }

    }


    public void ScrapCard(DeckCardContainer card) {
        _scrappedCardList.Add(card);
    }

    public DeckCardContainer DrawCard() {
        if (_remainingCardList.Count == 0 && _scrappedCardList.Count == 0) {
            return null;
        }
        int i = _remainingCardList.Count;

        if (i == 0) {
            foreach (DeckCardContainer card in _scrappedCardList) {
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


        DeckCardContainer returnCard = _remainingCardList[i];
        _remainingCardList.RemoveAt(i);

        return returnCard;
    }

    public void RecreateDeck() {
        _scrappedCardList = new List<DeckCardContainer>();
        _remainingCardList = new List<DeckCardContainer>();
        foreach (HeroSlot heroSlot in _deck.HeroSlotList) {
            if (heroSlot.Hero.SpritePath != null) {
                VFXObjectContainer.Instance.PlayAnimation("Slot" + heroSlot.SlotID);
                if (!PlayerTeam.Instance.InstantiatePlayerCharacter(heroSlot.Hero.Id, heroSlot.SlotID))
                    UIDataContainer.Instance.CharacterSlots[heroSlot.SlotID].LoadNewSprite(heroSlot.Hero.SpritePath);
            }
            foreach (CardDatabase card in heroSlot.Hero.CardList) {
                DeckCardContainer container = new DeckCardContainer(card, heroSlot.Hero);
                _remainingCardList.Add(container);
            }
        }
    }
}