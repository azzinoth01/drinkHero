using System;
using UnityEngine;

[Serializable]
public class Player {
    [SerializeField] private string _name;
    [SerializeField] private int _health;
    [SerializeField] private int _schild;
    [SerializeField] private int _attack;
    [SerializeField] private int _ressource;
    [SerializeField] private Card _handCards;
    [SerializeField] private GameDeck _gameDeck;
    [SerializeField] private Sprite _sprite;
}
