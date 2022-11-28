
#if CLIENT
using System;
using System.Collections.Generic;
using UnityEngine;
#endif

[Serializable, Table("Hero")]
public class HeroDatabase : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int _shield;
    [SerializeField] private int _health;
    [SerializeField] private string _spritePath;
    [SerializeField] private string _name;
    [NonSerialized] private List<CardToHero> _cardList;


#endif
#if SERVER
    private int _id;
    private int _shield;
    private int _health;
    private string _spritePath;
    private string _name;
    private List<CardToHero> _cardList;
    private DataRequestStatusEnum _cardListRequested;

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


#if SERVER

    public List<CardToHero> CardList {
        get {
            _cardList = DatabaseManager.GetDatabaseList<CardToHero>("RefHero", _id);



            return _cardList;
        }

    }

#endif
#if CLIENT

    public List<CardToHero> CardList {
        get {
            CheckRequestedData();
            if (_cardList.Count != 0) {
                return _cardList;
            }
            else {



                string name = GetPropertyName();

                if (AlreadyRequested(name)) {
                    return _cardList;
                }

                string functionCall = ClientFunctions.GetCardToHeroByKeyPair("RefHero\"" + _id + "\"");
                int index = SendRequest(functionCall, typeof(CardToHero));
                _propertyToRequestedId[index] = name;
            }

            return _cardList;
        }

        set {
            _cardList = value;
        }
    }

#endif
    public HeroDatabase() : base() {
        _cardList = new List<CardToHero>();
    }



}
