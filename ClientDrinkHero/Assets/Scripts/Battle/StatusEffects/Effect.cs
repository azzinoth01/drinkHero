using UnityEngine;

public class Effect : IEffect
{

    [SerializeField] protected string _id;
    [SerializeField] protected string _name;
    [SerializeField] protected int _durationType;
    [SerializeField] protected int _durationValue;
    [SerializeField] protected bool _stackable;
    [SerializeField] protected bool _refreshOnStack;
    [SerializeField] protected bool _ignoreStatusLimit;
    [SerializeField] protected int _maxValue;
    [SerializeField] protected int _minValue;
    [SerializeField] protected bool _isOver;
    [SerializeField] private EffectTypeEnum _classType;

    public string Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }
    public string Name {
        get {
            return _name;
        }

        set {
            _name = value;
        }
    }
    public int DurationType {
        get {
            return _durationType;
        }

        set {

            _durationType = value;

        }
    }
    public int DurationValue {
        get {
            return _durationValue;
        }

        set {
            _durationValue = value;
        }
    }
    public bool Stackable {
        get {
            return _stackable;
        }

        set {
            _stackable = value;
        }
    }
    public bool RefreshOnStack {
        get {
            return _refreshOnStack;
        }

        set {
            _refreshOnStack = value;
        }
    }
    public bool IgnoreStatusLimit {
        get {
            return _ignoreStatusLimit;
        }

        set {
            _ignoreStatusLimit = value;
        }
    }
    public int MaxValue {
        get {
            return _maxValue;
        }

        set {
            _maxValue = value;
        }
    }
    public int MinValue {
        get {
            return _minValue;
        }

        set {
            _minValue = value;
        }
    }
    public EffectTypeEnum ClassType {
        get {
            return _classType;
        }

        set {
            _classType = value;
        }
    }
    public DurationTypeEnum Duration {
        get {
            return (DurationTypeEnum) _durationType;
        }
    }
    public Effect(CardEffectData cardEffectData) {

        _id = cardEffectData.Id;
        _name = cardEffectData.Name;
        _durationType = cardEffectData.DurationType;
        _durationValue = cardEffectData.DurationValue;
        _stackable = cardEffectData.Stackable;
        _refreshOnStack = cardEffectData.RefreshOnStack;
        _ignoreStatusLimit = cardEffectData.IgnoreStatusLimit;
        _maxValue = cardEffectData.MaxValue;
        _minValue = cardEffectData.MinValue;
        _isOver = false;
        _classType = cardEffectData.ClassType;
    }

    protected virtual void ReduceDuration() {
        if(_durationType != (int) DurationTypeEnum.passiv) {
            _durationValue = _durationValue - 1;
        }
    }
    protected virtual void SetIsOver() {
        if(_durationValue <= 0) {
            _isOver = true;
        }
    }
    public virtual bool ActivateEffectBase(ICharacterAction target,ActivationTimeEnum activation,int? value = null) {
        return false;
    }
    public virtual bool ActivateEffect(IPlayerAction target,ActivationTimeEnum activation,int? value = null) {
        return ActivateEffectBase(target,activation,value);
    }
    public virtual bool StatusEffectApplyCheck(IEffect statusEffect) {
        return true;
    }
}
