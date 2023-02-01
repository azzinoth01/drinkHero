
#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif
[Table("Effect"), Serializable]
#if CLIENT
public class Effect : DatabaseItem, IEffect {
#endif
#if SERVER
public class Effect : DatabaseItem {
#endif

#if CLIENT
    [SerializeField] protected int _id;
    [SerializeField] protected string _name;
    [SerializeField] protected int _durationType;
    [SerializeField] protected int _durationValue;
    [SerializeField] protected bool _stackable;
    [SerializeField] protected bool _refreshOnStack;
    [SerializeField] protected bool _ignoreStatusLimit;
    [SerializeField] protected int _maxValue;
    [SerializeField] protected int _minValue;
    [SerializeField] protected bool _isOver;
    [SerializeField] private List<CardToEffect> _cardToEffects;
    [SerializeField] private string _classType;

    private static Dictionary<string, Effect> _cachedData = new Dictionary<string, Effect>();

#endif

#if SERVER
    protected int _id;
    protected string _name;
    private int _durationType;
    protected int _durationValue;
    protected bool _stackable;
    protected bool _refreshOnStack;
    protected bool _ignoreStatusLimit;
    protected int _maxValue;
    protected int _minValue;
    protected bool _isOver;
    private string _classType;
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

    [Column("DurationType")]
    public int DurationType {
        get {
            return _durationType;
        }

        set {

            _durationType = value;

        }
    }


    [Column("DurationValue")]
    public int DurationValue {
        get {
            return _durationValue;
        }

        set {
            _durationValue = value;
        }
    }
    [Column("Stackable")]
    public bool Stackable {
        get {
            return _stackable;
        }

        set {
            _stackable = value;
        }
    }
    [Column("RefreshOnStack")]
    public bool RefreshOnStack {
        get {
            return _refreshOnStack;
        }

        set {
            _refreshOnStack = value;
        }
    }
    [Column("IgnoreStatusLimit")]
    public bool IgnoreStatusLimit {
        get {
            return _ignoreStatusLimit;
        }

        set {
            _ignoreStatusLimit = value;
        }
    }
    [Column("MaxValues")]
    public int MaxValue {
        get {
            return _maxValue;
        }

        set {
            _maxValue = value;
        }
    }
    [Column("MinValues")]
    public int MinValue {
        get {
            return _minValue;
        }

        set {
            _minValue = value;
        }
    }


    [Column("Type")]
    public string ClassType {
        get {
            return _classType;
        }

        set {
            _classType = value;
        }
    }


#if CLIENT
    public DurationTypeEnum Duration {
        get {
            return (DurationTypeEnum)_durationType;
        }
    }




    public List<CardToEffect> CardToEffects {
        get {
            if (_cardToEffects.Count != 0) {
                return _cardToEffects;
            }
            else {
                string name = GetPropertyName();

                if (AlreadyRequested(name)) {
                    return _cardToEffects;
                }

                string functionCall = ClientFunctions.GetCardToEffectByKeyPair("RefEffect\"" + _id + "\"");
                int index = SendRequest(functionCall, typeof(CardToEffect));
                _propertyToRequestedId[index] = name;
            }
            return _cardToEffects;

        }
        set {
            _cardToEffects = value;
        }

    }





    public Effect(Effect statusEffect) {

        _id = statusEffect._id;
        _name = statusEffect._name;
        _durationType = statusEffect._durationType;
        _durationType = statusEffect._durationType;
        _durationValue = statusEffect._durationValue;
        _stackable = statusEffect._stackable;
        _refreshOnStack = statusEffect._refreshOnStack;
        _ignoreStatusLimit = statusEffect._ignoreStatusLimit;
        _maxValue = statusEffect._maxValue;
        _minValue = statusEffect._minValue;
        _isOver = statusEffect._isOver;
    }





    protected virtual void ReduceDuration() {
        if (_durationType != (int)DurationTypeEnum.passiv) {
            _durationValue = _durationValue - 1;


        }
    }
    protected virtual void SetIsOver() {
        if (_durationValue <= 0) {
            _isOver = true;

        }

    }





    public virtual bool ActivateEffectBase(ICharacterAction target, ActivationTimeEnum activation, int? value = null) {
        return false;
    }

    public virtual bool ActivateEffect(IPlayerAction target, ActivationTimeEnum activation, int? value = null) {
        return ActivateEffectBase(target, activation, value);
    }



    public virtual bool StatusEffectApplyCheck(IEffect statusEffect) {

        return true;
    }



    public static List<Effect> CreateObjectDataFromString(string message) {

        List<Effect> list = new List<Effect>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<Effect>();

        foreach (string[] obj in objectStrings) {
            Effect item = new Effect();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out Effect existingItem)) {
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



    public Effect() {
        _cardToEffects = new List<CardToEffect>();
    }

#endif

}
