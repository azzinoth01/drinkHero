using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Character {
    [SerializeField] protected int _health;
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected int _shield;

    [SerializeField] protected float _healModifier;
    [SerializeField] protected float _dmgModifier;
    [SerializeField] protected float _dmgRecieveModifier;
    [SerializeField] protected float _shieldModifier;




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

    public void HealCharacter(int heal) {
        _health = _health + heal;

        if (_health > _maxHealth) {
            _health = _maxHealth;
        }
    }
}
