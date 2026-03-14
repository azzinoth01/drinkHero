using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardEffectData
{
    [ReadOnly]
    [SerializeField] private string _id;

    [SerializeField] private string _name;
    [SerializeField] private int _durationType;
    [SerializeField] private int _durationValue;
    [SerializeField] private bool _stackable;
    [SerializeField] private bool _refreshOnStack;
    [SerializeField] private bool _ignoreStatusLimit;
    [SerializeField] private int _maxValue;
    [SerializeField] private int _minValue;
    [SerializeField] private EffectTypeEnum _classType;


    public CardEffectData() {
        _id = Guid.NewGuid().ToString("N");
        _classType = EffectTypeEnum.None;
    }

    public string Name {
        get {
            return _name;
        }
    }

    public int DurationType {
        get {
            return _durationType;
        }
    }

    public int DurationValue {
        get {
            return _durationValue;
        }
    }

    public bool Stackable {
        get {
            return _stackable;
        }
    }

    public bool RefreshOnStack {
        get {
            return _refreshOnStack;
        }
    }

    public bool IgnoreStatusLimit {
        get {
            return _ignoreStatusLimit;
        }
    }

    public int MaxValue {
        get {
            return _maxValue;
        }
    }

    public int MinValue {
        get {
            return _minValue;
        }
    }

    public EffectTypeEnum ClassType {
        get {
            return _classType;
        }
    }
}