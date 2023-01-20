#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Table("CardToEffect"), Serializable]
public class CardToEffect : DatabaseItem {
#if CLIENT
    [SerializeField] private int _id;
    private int? _refCard;
    private int? _refEffect;
    private CardDatabase _card;
    [SerializeField] private Effect _effect;

    private static Dictionary<string, CardToEffect> _cachedData = new Dictionary<string, CardToEffect>();

#endif
#if SERVER
    private int _id;
    private int? _refCard;
    private int? _refEffect;
    private CardDatabase _card;
    private Effect _effect;

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
    [Column("RefEffect")]
    public int? RefEffect {
        get {
            return _refEffect;
        }

        set {
            _refEffect = value;
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


    public Effect Effect {
        get {
            if (_refEffect == null) {
                return null;
            }
            _effect = DatabaseManager.GetDatabaseItem<Effect>(_refEffect);

            return _effect;
        }

        set {
            if (_effect == null) {
                _refEffect = null;
            }
            else {
                _refEffect = value.Id;
            }
            _effect = value;
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

    public Effect Effect {
        get {

            if (_refEffect == null) {
                return null;
            }
            else if (_effect != null) {
                return _effect;
            }

            //request data from server
            string name = GetPropertyName();

            if (AlreadyRequested(name)) {
                return null;
            }
            RequestEffect(name);


            return null;
        }

        set {

            _effect = value;

        }

    }

    private void RequestEffect(string name) {
        if (_refEffect == null) {
            Debug.Log("ref effect is null");
        }
        string functionCall = ClientFunctions.GetEffectByKeyPair("ID\"" + _refEffect + "\"");
        int index = SendRequest(functionCall, typeof(Effect));
        _propertyToRequestedId[index] = name;
    }

    private void RequestCard(string name) {
        string functionCall = ClientFunctions.GetCardDatabaseByKeyPair("ID\"" + _refCard + "\"");
        int index = SendRequest(functionCall, typeof(CardDatabase));
        _propertyToRequestedId[index] = name;
    }

    public static List<CardToEffect> CreateObjectDataFromString(string message) {

        List<CardToEffect> list = new List<CardToEffect>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<CardToEffect>();

        foreach (string[] obj in objectStrings) {
            CardToEffect item = new CardToEffect();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out CardToEffect existingItem)) {
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

        name = nameof(Card);
        if (AlreadyRequested(name) == false) {
            RequestCard(name);
        }
        name = nameof(Effect);
        if (AlreadyRequested(name) == false) {
            RequestEffect(name);
        }

    }


#endif

    public CardToEffect() : base() {

    }


}
