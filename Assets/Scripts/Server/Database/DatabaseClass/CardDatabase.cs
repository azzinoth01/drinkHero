

using System;
using System.Collections.Generic;
using UnityEngine;

[Table("Card"), Serializable]
public class CardDatabase : DatabaseItem {
    [SerializeField] private long _id;
    [SerializeField] private string _name;
    [SerializeField] private int _attack;
    [SerializeField] private int _shield;
    [SerializeField] private int _health;
    [SerializeField] private int _cost;
    [SerializeField] private string _spritePath;
    [SerializeField] private string _refUpgradeTo;
    [NonSerialized] private CardDatabase _upgradeTo;
    [NonSerialized] private List<CardToHero> _heroList;

    [Column("ID"), PrimaryKey]
    public long Id {
        get {
            return _id;
        }

        set {
            _id = value;
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
    [Column("Attack")]
    public int Attack {
        get {
            return _attack;
        }

        set {
            _attack = value;
        }
    }
    [Column("Shield")]
    public int Health {
        get {
            return _health;
        }

        set {
            _health = value;
        }
    }
    [Column("Health")]
    public int Shield {
        get {
            return _shield;
        }

        set {
            _shield = value;
        }
    }
    [Column("Cost")]
    public int Cost {
        get {
            return _cost;
        }

        set {
            _cost = value;
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
    [Column("RefUpgradeTo")]
    public string RefUpgradeTo {
        get {
            return _refUpgradeTo;
        }

        set {
            _refUpgradeTo = value;


        }
    }


    public CardDatabase UpgradeTo {
        get {
            if (_refUpgradeTo == null) {
                return null;
            }
            _upgradeTo = DatabaseManager.GetDatabaseItem<CardDatabase>(long.Parse(_refUpgradeTo));
            return _upgradeTo;

        }

        set {
            if (value == null) {
                _refUpgradeTo = null;
            }
            else {
                _refUpgradeTo = value.Id.ToString();
            }

            _upgradeTo = value;
        }
    }

    public List<CardToHero> HeroList {
        get {
            _heroList = DatabaseManager.GetDatabaseList<CardToHero>("RefCard", _id);
            return _heroList;
        }


    }

    public CardDatabase() {

    }



}
