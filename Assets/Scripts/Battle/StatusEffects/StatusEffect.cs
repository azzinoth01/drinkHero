using System;
using UnityEngine;

[Serializable]
public class StatusEffect : ICloneable {


    [SerializeField] protected int _id;
    [SerializeField] protected string _name;


    [SerializeField] protected DurationTypeEnum _durationType;
    [SerializeField] protected int _durationValue;
    [SerializeField] protected bool _stackable;
    [SerializeField] protected bool _refreshOnStack;
    [SerializeField] protected bool _ignoreStatusLimit;

    [SerializeField] protected int _value;
    [SerializeField] protected bool _isOver;

    public int Id {
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



    public DurationTypeEnum DurationType {
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

    public int Value {
        get {
            return _value;
        }

        set {
            _value = value;
        }
    }



    public StatusEffect() {

    }



    public StatusEffect(StatusEffect statusEffect) {

        _id = statusEffect._id;
        _name = statusEffect._name;
        _durationType = statusEffect._durationType;
        _durationValue = statusEffect._durationValue;
        _stackable = statusEffect._stackable;
        _refreshOnStack = statusEffect._refreshOnStack;
        _ignoreStatusLimit = statusEffect._ignoreStatusLimit;
        _value = statusEffect._value;
        _isOver = statusEffect._isOver;
    }


    public virtual void ActivateStatusEffect(Character target, ActivationTimeEnum activation, int? value = null) {

    }


    public virtual void ReduceDuration() {
        if (_durationType != DurationTypeEnum.passiv) {
            _durationValue = _durationValue - 1;

            if (_durationValue <= 0) {
                _isOver = true;
            }
        }
    }

    public virtual bool CheckIfEffectIsOver() {
        return _isOver;
    }

    public virtual object Clone() {
        return new StatusEffect(this);

    }
}
