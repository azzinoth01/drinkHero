using System;
using UnityEngine;

[Serializable]
public class Card {
    [SerializeField] private string _name;
    [SerializeField] private uint _iD;
    [SerializeField] private int _attack;
    [SerializeField] private int _schild;
    [SerializeField] private int _health;
    [SerializeField] private string _text;
    [SerializeField] private int _costs;
    [SerializeField] private Card _upgradeTo;
    [SerializeField] private uint _upgradeCosts;
    [SerializeField] private ElementEnum _element;
    [SerializeField] private Sprite _sprite;

    public int Attack {
        get {
            return _attack;
        }


    }

    public int Schild {
        get {
            return _schild;
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
}
