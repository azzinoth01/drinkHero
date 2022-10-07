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

}
