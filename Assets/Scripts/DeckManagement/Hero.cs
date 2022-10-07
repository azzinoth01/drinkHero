using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hero {
    [SerializeField] private string _name;
    [SerializeField] private uint _iD;
    [SerializeField] private int _attack;
    [SerializeField] private int _schild;
    [SerializeField] private int _health;
    [SerializeField] private List<Card> _cardList;
    [SerializeField] private ElementEnum _element;
    [SerializeField] private Sprite _sprite;

    public List<Card> CardList {
        get {
            return _cardList;
        }

    }


    public Hero() {
        _cardList = new List<Card>();
    }
}
