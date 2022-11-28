using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class Card : ICascadable, IWaitingOnServer, ICardDisplay {
    [SerializeField] private string _name;
    [SerializeField] private long _iD;
    [SerializeField] private int _attack;
    [SerializeField] private int _shield;
    [SerializeField] private int _health;
    [SerializeField] private string _text;
    [SerializeField] private int _costs;
    [SerializeField] private Card _upgradeTo;
    [SerializeField] private uint _upgradeCosts;
    [SerializeField] private ElementEnum _element;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private List<BuffHealOverTime> _statusEffects;
    private List<ICascadable> _cascadables;

    private CardDatabase _cardData;
    private bool _isWaitingOnServer;

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

    public string Text {
        get {
            return _text;
        }

    }

    public int Costs {
        get {
            return _costs;
        }


    }

    public ElementEnum Element {
        get {
            return _element;
        }


    }

    public Sprite Sprite {
        get {
            return _sprite;
        }


    }

    public string Name {
        get {
            return _name;
        }


    }

    public List<BuffHealOverTime> StatusEffects {
        get {
            return _statusEffects;
        }


    }

    public CardDatabase CardData {
        get {
            return _cardData;
        }

        set {
            _cardData = value;

            if (_cardData != null) {
                if (ConvertCardData() == false) {
                    GlobalGameInfos.Instance.WaitOnServerObjects.Add(this);
                }

            }
        }
    }
    public bool ConvertCardData() {
        if (_cardData == null) {
            return false;
        }


        _iD = _cardData.Id;
        _name = _cardData.Name;
        _attack = _cardData.Attack;
        _costs = _cardData.Cost;
        _health = _cardData.Health;
        _shield = _cardData.Shield;

        Cascade(this);

        return true;
    }

    public bool GetUpdateFromServer() {


        return ConvertCardData();
    }

    public void Cascade(ICascadable causedBy, PropertyInfo changedProperty = null, object changedValue = null) {
        if (causedBy == null) {
            causedBy = this;
        }
        foreach (ICascadable cascadable in Cascadables) {
            cascadable.Cascade(causedBy, changedProperty, changedValue);
        }
    }

    public string CostText() {
        return _costs.ToString();
    }

    public string AttackText() {
        return _attack.ToString();
    }

    public string ShieldText() {
        return _shield.ToString();
    }

    public string HealthText() {
        return _health.ToString();
    }

    public Sprite SpriteDisplay() {
        return _sprite;
    }

    public long ID {
        get {
            return _iD;
        }

        set {
            _iD = value;
        }
    }

    public bool IsWaitingOnServer {
        get {
            return _isWaitingOnServer;
        }

        set {
            _isWaitingOnServer = value;
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


    public Card() {
        _cascadables = new List<ICascadable>();
        _statusEffects = new List<BuffHealOverTime>();
    }
}
