using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class User {


    [SerializeField] private string _name;
    [SerializeField] private uint _iD;
    [SerializeField] private uint _money;
    [SerializeField] private uint _crystalBottles;
    [SerializeField] private List<Hero> _heroList;
    [SerializeField] private List<Deck> _deckList;
    [SerializeField] private Sprite _sprite;
}
