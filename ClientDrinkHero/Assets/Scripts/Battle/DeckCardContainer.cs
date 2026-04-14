using System;
using UnityEngine;
[Serializable]
public class DeckCardContainer : ICardDisplay
{
    [SerializeField] private CardData _card;
    [SerializeField] private HeroObject _hero;

    public CardData Card {
        get {
            return _card;
        }
        set {
            _card = value;
        }
    }

    public HeroObject Hero {
        get {
            return _hero;
        }
        set {
            _hero = value;
        }
    }
    public DeckCardContainer(CardData card,HeroObject hero) {
        _card = card;
        _hero = hero;
    }
    public string CostText() {
        return _card.Cost.ToString();
    }
    public string AttackText() {
        return "";
    }
    public string ShieldText() {
        return "";
    }
    public string HealthText() {
        return "";
    }
    public string GetSpritePath() {
        return _card.SpritePath;
    }
    public string CardText() {
        return _card.Text;
    }
    public string CardName() {
        return _card.name;
    }
}
