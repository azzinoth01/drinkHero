


#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Table("Card"), Serializable]
#if CLIENT
public class CardDatabase : DatabaseItem, ICardDisplay {

    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _text;
    [SerializeField] private int _cost;
    [SerializeField] private string _spritePath;
    private int? _refUpgradeTo;
    [SerializeField] private string _iconPath;
    [SerializeField] private CardDatabase _upgradeTo;
    [SerializeField] private List<CardToHero> _heroList;
    [SerializeField] private List<CardToEffect> _cardEffectList;
    [SerializeField] private int? _refUpgradeItem;
    [SerializeField] private int _upgradeItemAmount;
    [SerializeField] private string _animationKey;

    private static Dictionary<string, CardDatabase> _cachedData = new Dictionary<string, CardDatabase>();


#endif
#if SERVER
public class CardDatabase : DatabaseItem {
    private int _id;
    private string _name;
    private string _text;
    private int _cost;
    private string _spritePath;
    private string _iconPath;
    private int? _refUpgradeTo;
    private CardDatabase _upgradeTo;
    private List<CardToHero> _heroList;
    private List<CardToEffect> _cardEffectList;

    private int? _refUpgradeItem;
    private int _upgradeItemAmount;
    private string _animationKey;

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
    [Column("IconPath")]
    public string IconPath
    {
        get
        {
            return _iconPath;
        }

        set
        {
            _iconPath = value;
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
    [Column("CardText")]
    public string Text {
        get {
            return _text;
        }

        set {
            _text = value;
        }
    }

    [Column("RefUpgradeItem")]
    public int? RefUpgradeItem {
        get {
            return _refUpgradeItem;
        }

        set {
            _refUpgradeItem = value;
        }
    }
    [Column("UpgradeItemAmount")]
    public int UpgradeItemAmount {
        get {
            return _upgradeItemAmount;
        }

        set {
            _upgradeItemAmount = value;
        }
    }

    [Column("AnimationKey")]
    public string AnimationKey {
        get {
            return _animationKey;
        }

        set {
            _animationKey = value;
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

    public List<CardToHero> HeroList {
        get {

            _heroList = DatabaseManager.GetDatabaseList<CardToHero>("RefCard", _id);

            return _heroList;
        }


    }


#endif


#if CLIENT



    public List<CardToEffect> CardEffectList {
        get {

            if (_cardEffectList.Count != 0) {
                return _cardEffectList;
            }
            else {
                string name = GetPropertyName();

                if (AlreadyRequested(name)) {
                    return _cardEffectList;
                }

                RequestCardEffectList(name);
            }

            return _cardEffectList;
        }

        set {

            _cardEffectList = value;


        }
    }


    public List<CardToHero> HeroList {
        get {

            if (_heroList.Count != 0) {
                return _heroList;
            }
            else {
                string name = GetPropertyName();

                if (AlreadyRequested(name)) {
                    return _heroList;
                }

                RequestHeroList(name);
            }

            return _heroList;
        }

        set {
            _heroList = value;

        }
    }


    private void RequestHeroList(string name) {
        string functionCall = ClientFunctions.GetCardToHeroByKeyPair("RefCard\"" + _id + "\"");
        int index = SendRequest(functionCall, typeof(CardToHero));
        _propertyToRequestedId[index] = name;
    }
    private void RequestCardEffectList(string name) {
        string functionCall = ClientFunctions.GetCardToEffectByKeyPair("RefCard\"" + _id + "\"");
        int index = SendRequest(functionCall, typeof(CardToEffect));
        _propertyToRequestedId[index] = name;
    }

    public static List<CardDatabase> CreateObjectDataFromString(string message) {


        List<CardDatabase> list = new List<CardDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<CardDatabase>();

        foreach (string[] obj in objectStrings) {
            CardDatabase item = new CardDatabase();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out CardDatabase existingItem)) {
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

        name = nameof(HeroList);
        if (AlreadyRequested(name) == false) {
            RequestHeroList(name);
        }
        name = nameof(CardEffectList);
        if (AlreadyRequested(name) == false) {
            RequestCardEffectList(name);
        }

    }


#endif


    public CardDatabase() : base() {
        _refUpgradeTo = null;
        _cardEffectList = new List<CardToEffect>();
        _heroList = new List<CardToHero>();
}



#if CLIENT

    public string CostText() {
        return _cost.ToString();
    }

    public string AttackText() {
        return "";
    }

    public string ShieldText() {
        return "";
    }

    public string HealthText() {
        return "";
    }

    public string GetSpritePath() {
        return _iconPath;
    }
    public string GetIconPath(){
        return _iconPath;
    }

    public string CardText() {
        return _text;
    }

    public string CardName() {
        return _name;
    }

#endif
}
