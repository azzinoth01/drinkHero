using System;
using UnityEngine;

[Serializable]
public class EnemySkill {
    [SerializeField] private string _name;
    [SerializeField] private int _minAttack;
    [SerializeField] private int _minSchild;
    [SerializeField] private int _minHealth;

    [SerializeField] private int _maxAttack;
    [SerializeField] private int _maxSchild;
    [SerializeField] private int _maxHealth;

    [SerializeField] private int _cooldown;
    [SerializeField] private int _currentCooldown;

    public int MinAttack {
        get {
            return _minAttack;
        }


    }

    public int MinSchild {
        get {
            return _minSchild;
        }


    }

    public int MinHealth {
        get {
            return _minHealth;
        }

    }

    public int MaxAttack {
        get {
            return _maxAttack;
        }

    }

    public int MaxSchild {
        get {
            return _maxSchild;
        }


    }

    public int MaxHealth {
        get {
            return _maxHealth;
        }


    }

    public int CurrentCooldown {
        get {
            return _currentCooldown;
        }


    }

    public int Cooldown {
        get {
            return _cooldown;
        }


    }

    public void CooldownTick() {
        if (CurrentCooldown == 0) {
            return;
        }
        _currentCooldown = _currentCooldown - 1;

    }

    public void StartCooldown() {
        _currentCooldown = Cooldown;
    }
}
