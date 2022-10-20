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

        set {
            _minAttack = value;
        }
    }

    public int MinSchild {
        get {
            return _minSchild;
        }

        set {
            _minSchild = value;
        }
    }

    public int MinHealth {
        get {
            return _minHealth;
        }

        set {
            _minHealth = value;
        }
    }

    public int MaxAttack {
        get {
            return _maxAttack;
        }

        set {
            _maxAttack = value;
        }
    }

    public int MaxSchild {
        get {
            return _maxSchild;
        }

        set {
            _maxSchild = value;
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

    public int Cooldown {
        get {
            return _cooldown;
        }

        set {
            _cooldown = value;
        }
    }

    public int CurrentCooldown {
        get {
            return _currentCooldown;
        }

        set {
            _currentCooldown = value;
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
