using System;
using UnityEngine;

[Serializable]
public abstract class Character {
    [SerializeField] protected int _health;
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected int _shield;

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
}
