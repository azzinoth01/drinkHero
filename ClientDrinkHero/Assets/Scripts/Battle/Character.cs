using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public abstract class Character : ICascadable {
    [SerializeField] protected long _id;
    [SerializeField] protected int _health;
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected int _shield;

    [SerializeField] protected float _healModifier;
    [SerializeField] protected float _dmgModifier;
    [SerializeField] protected float _dmgRecieveModifier;
    [SerializeField] protected float _shieldModifier;
    private List<ICascadable> _cascadables;



    protected List<Buff> _buffList;
    protected List<Debuff> _debuffList;

    public int Health {
        get {
            return _health;
        }

        set {
            _health = value;
        }
    }

    public int MaxHealth {
        get {
            return _maxHealth;
        }

        set {
            _maxHealth = value;
        }
    }

    public int Shield {
        get {
            return _shield;
        }

        set {
            _shield = value;
        }
    }

    public List<Buff> BuffList {
        get {
            return _buffList;
        }


    }

    public List<Debuff> DebuffList {
        get {
            return _debuffList;
        }


    }


    public List<ICascadable> Cascadables {
        get {
            return _cascadables;
        }

        set {
            _cascadables = value;
        }
    }

    public void HealCharacter(int heal) {
        _health = _health + heal;

        if (_health > _maxHealth) {
            _health = _maxHealth;
        }
    }

    public virtual void Clear() {

        _maxHealth = 0;
        _health = 0;
        _shield = 0;

        _buffList = new List<Buff>();
        _debuffList = new List<Debuff>();

    }

    public Character() {
        _cascadables = new List<ICascadable>();
        _buffList = new List<Buff>();
        _debuffList = new List<Debuff>();
    }

    public virtual void Cascade(ICascadable causedBy, PropertyInfo changedProperty = null, object changedValue = null) {
        if (causedBy == null) {
            causedBy = this;
        }
        foreach (ICascadable cascadable in Cascadables) {
            cascadable.Cascade(causedBy, changedProperty, changedValue);
        }
    }
}
