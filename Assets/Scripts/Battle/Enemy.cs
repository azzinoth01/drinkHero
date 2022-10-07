using System;
using UnityEngine;

[Serializable]
public class Enemy {
    [SerializeField] private int _health;
    [SerializeField] private int _schild;
    [SerializeField] private int _attack;
    [SerializeField] private ElementEnum _element;
    [SerializeField] private Sprite _sprite;
}
