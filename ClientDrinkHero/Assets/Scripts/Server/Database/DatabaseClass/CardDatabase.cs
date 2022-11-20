


#if CLIENT
using System;
using System.Collections.Generic;
using UnityEngine;
#endif

[Table("Card"), Serializable]
public class CardDatabase : DatabaseItem {
#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private int _attack;
    [SerializeField] private int _shield;
    [SerializeField] private int _health;
    [SerializeField] private int _cost;
    [SerializeField] private string _spritePath;
    [SerializeField] private int? _refUpgradeTo;
    [NonSerialized] private CardDatabase _upgradeTo;
    [NonSerialized] private List<CardToHero> _heroList;

#endif
#if SERVER
    private int _id;
    private string _name;
    private int _attack;
    private int _shield;
    private int _health;
    private int _cost;
    private string _spritePath;
    private int? _refUpgradeTo;
    private CardDatabase _upgradeTo;
    private List<CardToHero> _heroList;

#endif
    [Column("ID"), PrimaryKey]
    public int Id {
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
    [Column("Health")]
    public int Health {
        get {
            return _health;
        }

        set {
            _health = value;
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
    public int? RefUpgradeTo {
        get {
            return _refUpgradeTo;
        }

        set {
            _refUpgradeTo = value;


        }
    }

#if SERVER
    public CardDatabase UpgradeTo {
        get {
            if (_refUpgradeTo == null) {
                return null;
            }
            _upgradeTo = DatabaseManager.GetDatabaseItem<CardDatabase>(_refUpgradeTo);
            return _upgradeTo;

        }

        set {
            if (value == null) {
                _refUpgradeTo = null;
            }
            else {
                _refUpgradeTo = value.Id;
            }

            _upgradeTo = value;
        }
    }
#endif
#if SERVER
    public List<CardToHero> HeroList {
        get {

            _heroList = DatabaseManager.GetDatabaseList<CardToHero>("RefCard", _id);

            return _heroList;
        }


    }
#endif

    public CardDatabase() {
        _refUpgradeTo = null;
    }



}
