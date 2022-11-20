using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[Serializable]
public class Hero : ICascadable, IWaitingOnServer {
    [SerializeField] private string _name;
    [SerializeField] private long _iD;
    [SerializeField] private int _attack;
    [SerializeField] private int _shield;
    [SerializeField] private int _health;
    [SerializeField] private Dictionary<int, Card> _cardList;
    [SerializeField] private ElementEnum _element;
    [SerializeField] private Sprite _sprite;

    private List<ICascadable> _cascadables;
    private bool _isWaitingOnServer;

    private HeroDatabase _heroData;

    public Dictionary<int, Card> CardList {
        get {
            return _cardList;


        }

    }

    public int Attack {
        get {
            return _attack;
        }


    }

    public int Shield {
        get {
            return _shield;
        }


    }

    public int Health {
        get {
            return _health;
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

    public HeroDatabase HeroData {
        get {
            return _heroData;
        }

        set {
            _heroData = value;
            if (_heroData != null) {
                if (ConvertHeroDatabase() == false) {
                    GlobalGameInfos.Instance.WaitOnServerObjects.Add(this);
                }

            }
        }
    }

    public bool ConvertHeroDatabase() {
        if (_heroData == null) {
            return false;
        }
        _iD = _heroData.Id;
        _name = _heroData.Name;
        _health = _heroData.Health;
        _shield = _heroData.Shield;
        List<CardToHero> cardToHeroes = _heroData.GetCardList(out _isWaitingOnServer);

        if (_isWaitingOnServer) {
            return false;
        }

        foreach (CardToHero cardToHero in cardToHeroes) {
            if (cardToHero.RefCard == null) {
                continue;
            }
            bool waitOn = false;
            long id = cardToHero.RefCard.Value;
            int index = cardToHero.Id;
            if (_cardList.TryGetValue(index, out Card card)) {

                card.CardData = cardToHero.GetCard(out waitOn);

            }
            else {
                card = new Card();
                card.ID = id;
                card.CardData = cardToHero.GetCard(out waitOn);
                _cardList.AddWithCascading(index, card, this);
            }

            _isWaitingOnServer = _isWaitingOnServer | waitOn;
        }
        if (_isWaitingOnServer) {
            return false;
        }

        Cascade(this);

        return true;
    }

    public bool IsWaitingOnServer {
        get {
            return _isWaitingOnServer;
        }

        set {
            _isWaitingOnServer = value;
        }
    }

    public Hero() {
        _cardList = new Dictionary<int, Card>();
        _cascadables = new List<ICascadable>();
    }

    public void Cascade(ICascadable causedBy, PropertyInfo changedProperty = null, object changedValue = null) {
        if (causedBy == null) {
            causedBy = this;
        }
        foreach (ICascadable cascadable in Cascadables) {
            cascadable.Cascade(causedBy, changedProperty, changedValue);
        }

    }

    public bool GetUpdateFromServer() {


        return ConvertHeroDatabase();
    }
}
