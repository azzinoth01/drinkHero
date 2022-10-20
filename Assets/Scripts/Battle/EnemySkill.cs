using System;
using UnityEngine;

[Serializable]
public class EnemySkill {
    [SerializeField] private string _name;
    [SerializeField] private int _minAttack;
    [SerializeField] private int _minSchield;
    [SerializeField] private int _minHealth;

    [SerializeField] private int _maxAttack;
    [SerializeField] private int _maxSchield;
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

    public int MinSchield {
        get {
            return _minSchield;
        }

        set {
            _minSchield = value;
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

    public int MaxSchield {
        get {
            return _maxSchield;
        }

        set {
            _maxSchield = value;
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
