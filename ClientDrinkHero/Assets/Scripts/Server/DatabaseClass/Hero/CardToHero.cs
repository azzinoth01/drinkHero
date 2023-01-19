
#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Table("CardToHero"), Serializable]
public class CardToHero : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    private int? _refCard;
    private int? _refHero;
    [SerializeField] private CardDatabase _card;
    [SerializeField] private HeroDatabase _hero;


    private static Dictionary<string, CardToHero> _cachedData = new Dictionary<string, CardToHero>();

#endif


#if SERVER
    private int _id;
    private int? _refCard;
    private int? _refHero;
    private CardDatabase _card;
    private HeroDatabase _hero;

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
    [Column("RefCard")]
    public int? RefCard {
        get {
            return _refCard;
        }

        set {
            _refCard = value;
        }
    }
    [Column("RefHero")]
    public int? RefHero {
        get {
            return _refHero;
        }

        set {
            _refHero = value;
        }
    }

#if SERVER
    public CardDatabase Card {
        get {
            if (_refCard == null) {
                return null;
            }
            _card = DatabaseManager.GetDatabaseItem<CardDatabase>(_refCard);

            return _card;
        }

        set {
            if (_card == null) {
                _refCard = null;
            }
            else {
                _refCard = value.Id;
            }
            _card = value;
        }
    }

    public HeroDatabase Hero {
        get {
            if (_refHero == null) {
                return null;
            }
            _hero = DatabaseManager.GetDatabaseItem<HeroDatabase>(_refHero);
            return _hero;
        }

        set {
            if (_hero == null) {
                _refHero = null;
            }
            else {
                _refHero = value.Id;
            }
            _hero = value;
        }
    }

#endif

#if CLIENT

    public CardDatabase Card {
        get {

            if (_refCard == null) {
                return null;
            }
            else if (_card != null) {

                return _card;
            }


            //request data from server
            string name = GetPropertyName();

            if (AlreadyRequested(name)) {
                return null;
            }

            RequestCard(name);


            return null;


        }

        set {

            _card = value;

        }

    }

    public HeroDatabase Hero {
        get {
            if (_refHero == null) {
                return null;
            }
            else if (_hero != null) {

                return _hero;
            }


            //request data from server
            string name = GetPropertyName();

            if (AlreadyRequested(name)) {
                return null;
            }


            RequestHero(name);

            return null;


        }

        set {

            _hero = value;

        }


    }
    private void RequestHero(string name) {

        string functionCall = ClientFunctions.GetHeroDatabaseByKeyPair("ID\"" + _refHero + "\"");
        int index = SendRequest(functionCall, typeof(HeroDatabase));
        _propertyToRequestedId[index] = name;
    }
    private void RequestCard(string name) {
        string functionCall = ClientFunctions.GetCardDatabaseByKeyPair("ID\"" + _refCard + "\"");
        int index = SendRequest(functionCall, typeof(CardDatabase));
        _propertyToRequestedId[index] = name;
    }


    public static List<CardToHero> CreateObjectDataFromString(string message) {

        List<CardToHero> list = new List<CardToHero>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<CardToHero>();

        foreach (string[] obj in objectStrings) {
            CardToHero item = new CardToHero();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out CardToHero existingItem)) {
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

        name = nameof(Hero);
        if (AlreadyRequested(name) == false) {
            RequestHero(name);
        }
        name = nameof(Card);
        if (AlreadyRequested(name) == false) {
            RequestCard(name);
        }

    }


#endif
    public CardToHero() : base() {
        _refCard = null;
        _refHero = null;
        _card = null;
        _hero = null;
    }


}
