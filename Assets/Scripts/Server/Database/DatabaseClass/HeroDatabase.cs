
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable, Table("Hero")]
public class HeroDatabase : DatabaseItem {
    [SerializeField] private long _id;
    [SerializeField] private int _shield;
    [SerializeField] private int _health;
    [SerializeField] private string _spritePath;
    [SerializeField] private string _name;
    [NonSerialized] private List<CardToHero> _cardList;




    [Column("ID"), PrimaryKey]
    public long Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }
    [Column("Shield")]
    public int Shield {
        get {
            return _shield;
        }

        set {
            _shield = value;
        }
    }
    [Column("Health")]
    public int Health {
        get {
            return _health;
        }

        set {
            _health = value;
        }
    }
    [Column("SpritePath")]
    public string SpritePath {
        get {
            return _spritePath;
        }

        set {
            _spritePath = value;
        }
    }
    [Column("Name")]
    public string Name {
        get {
            return _name;
        }

        set {
            _name = value;
        }
    }

    public List<CardToHero> CardList {
        get {
            _cardList = DatabaseManager.GetDatabaseList<CardToHero>("RefHero", _id);
            return _cardList;
        }

    }

    public HeroDatabase() {

    }



}
