using System;
using UnityEngine;
[Serializable]
public class DeckCardContainer : ICardDisplay {
    [SerializeField] private CardDatabase _card;
    [SerializeField] private HeroDatabase _hero;

    public CardDatabase Card {
        get {
            return _card;
        }

        set {
            _card = value;
        }
    }

    public HeroDatabase Hero {
        get {
            return _hero;
        }

        set {
            _hero = value;
        }
    }

    public DeckCardContainer(CardDatabase card, HeroDatabase hero) {
        _card = card;
        _hero = hero;
    }



    public string CostText() {
        return _card.CostText();
    }

    public string AttackText() {
        return _card.AttackText();
    }

    public string ShieldText() {
        return _card.ShieldText();
    }

    public string HealthText() {
        return _card.HealthText();
    }

    public string GetSpritePath() {
        return _hero.SpritePath;
    }

    public string CardText() {
        return _card.CardText();
    }

    public string CardName() {
        return _card.CardName();
    }
}
