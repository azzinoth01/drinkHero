using System;

[Serializable]
public class EnemySkill {
    private int _minAttack;
    private int _minSchild;
    private int _minHealth;

    private int _maxAttack;
    private int _maxSchild;
    private int _maxHealth;

    private int _cooldown;
    private int _currentCooldown;

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
