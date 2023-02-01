
#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
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
    //private List<CardToHero> _cardList;

    public static Dictionary<string, HeroDatabase> _cachedData = new Dictionary<string, HeroDatabase>();


    [SerializeField] private List<CardDatabase> _cardDatabases;

#endif
#if SERVER
    private int _id;
    private int _shield;
    private int _health;
    private string _spritePath;
    private string _name;
    private List<CardToHero> _cardList;
    private List<CardDatabase> _cardDatabases;


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

    public List<CardDatabase> CardList {
        get {

            if (_cardDatabases.Count != 0) {
                return _cardDatabases;
            }
            else {

                string name = GetPropertyName();

                if (AlreadyRequested(name)) {
                    return _cardDatabases;
                }

                RequestCardList(name);

            }

            return _cardDatabases;
        }

        set {
            _cardDatabases = value;
        }
    }

    private void RequestCardList(string name) {

        string functionCall = ClientFunctions.GetCardListOfHero("RefHero\"" + _id + "\"");
        int index = SendRequest(functionCall, typeof(CardDatabase));
        _propertyToRequestedId[index] = name;
    }


    public static List<HeroDatabase> CreateObjectDataFromString(string message) {

        List<HeroDatabase> list = new List<HeroDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<HeroDatabase>();

        foreach (string[] obj in objectStrings) {
            HeroDatabase item = new HeroDatabase();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out HeroDatabase existingItem)) {
                        item = existingItem;
                        break;
                    }
                    else {
                        _cachedData.Add(parameterValue, item);
                    }

                }

                if (mapping.ColumnsMapping.TryGetValue(parameterName, out string property)) {
                    PropertyInfo info = item.GetType().GetProperty(property);
                    DatabaseItemCreationHelper.ParseParameterValues(item, info, parameterValue);
                }
            }
            list.Add(item);
        }

        return list;
    }

    public override void RequestLoadReferenzData() {
        string name = null;

        name = nameof(CardList);
        if (AlreadyRequested(name) == false) {
            RequestCardList(name);
        }

    }
#endif
    public HeroDatabase() : base() {
#if SERVER
        _cardList = new List<CardToHero>();
#endif

        _cardDatabases = new List<CardDatabase>();
    }




}
