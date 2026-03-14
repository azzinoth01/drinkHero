using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class HeroData
{
    [ReadOnly]
    [SerializeField] private string _id;

    [SerializeField] private int _shield;
    [SerializeField] private int _health;
    [SerializeField] private string _spritePath;
    [SerializeField] private string _name;
    [SerializeField] private List<string> _cardList;


    public HeroData() {
        _id = Guid.NewGuid().ToString("N");
        _cardList = new List<string>();
    }

    public string Id {
        get {
            return _id;
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

    public string SpritePath {
        get {
            return _spritePath;
        }
    }

    public string Name {
        get {
            return _name;
        }
    }

    public List<string> CardList {
        get {
            return _cardList;
        }
    }
}
