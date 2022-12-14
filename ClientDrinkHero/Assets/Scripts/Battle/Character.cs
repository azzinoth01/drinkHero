using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Character : ICharacterAction, ICharacter {
    [SerializeField] protected long _id;
    [SerializeField] protected int _health;
    [SerializeField] protected int _maxHealth;
    [SerializeField] protected int _shield;


    [SerializeField] protected ModifierStruct _healModifier;
    [SerializeField] protected ModifierStruct _dmgModifier;
    [SerializeField] protected ModifierStruct _defenceModifier;
    [SerializeField] protected ModifierStruct _shieldModifier;


    protected int _skipTurn;



    protected int _buffMultihit;
    protected int _dmgCausedThisAction;


    protected List<IBuff> _buffList;
    protected List<IDebuff> _debuffList;

    public event Action<int> HealthChange;
    public event Action<int> ShieldChange;
    public event Action TurnEnded;

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

    public List<IBuff> BuffList {
        get {
            return _buffList;
        }


    }

    public List<IDebuff> DebuffList {
        get {
            return _debuffList;
        }


    }


    protected abstract void Death();




    public void HealCharacter(int heal) {
        _health = _health + heal;

        if (_health > _maxHealth) {
            _health = _maxHealth;
        }
    }

    public virtual void Clear() {

        _buffMultihit = 1;
        _maxHealth = 0;
        _health = 0;
        _shield = 0;
        _dmgCausedThisAction = 0;
        _buffList = new List<IBuff>();
        _debuffList = new List<IDebuff>();

        _dmgModifier = new ModifierStruct(0, 0);
        _healModifier = new ModifierStruct(0, 0);
        _defenceModifier = new ModifierStruct(0, 0);
        _shieldModifier = new ModifierStruct(0, 0);

    }

    public Character() {

        _buffList = new List<IBuff>();
        _debuffList = new List<IDebuff>();

        _buffMultihit = 1;
        _dmgCausedThisAction = 0;

        _dmgModifier = new ModifierStruct(0, 0);
        _healModifier = new ModifierStruct(0, 0);
        _defenceModifier = new ModifierStruct(0, 0);
        _shieldModifier = new ModifierStruct(0, 0);

    }



    public void Heal(int value) {

        value = _healModifier.CalcValue(value);

        _health = _health + value;
        if (_health > _maxHealth) {
            _health = _maxHealth;
        }
        HealthChange?.Invoke(value);
    }

    public void TakeDmg(int value) {

        value = _defenceModifier.CalcValue(value);

        int shieldDmg = 0;
        if (_shield > 0) {
            if (_shield > value) {
                _shield = _shield - value;
                shieldDmg = -value;
                value = 0;
            }
            else {
                value = value - _shield;
                shieldDmg = -_shield;
                _shield = 0;

            }
            ShieldChange?.Invoke(shieldDmg);
        }
        int healthDmg = 0;
        if (_health - value < 0) {
            healthDmg = -_health;
            _health = 0;
        }
        else {
            healthDmg = -value;
            _health -= value;
        }

        HealthChange?.Invoke(healthDmg);


        if (_health <= 0) {

            Death();
        }
    }

    void ICharacterAction.Shield(int value) {

        value = _shieldModifier.CalcValue(value);

        _shield = _shield + value;
        ShieldChange?.Invoke(value);
    }

    int ICharacter.MaxHealth() {
        return _maxHealth;
    }

    public int CurrentHealth() {
        return _health;
    }

    public int CurrentShield() {
        return _shield;
    }
    protected void InvokeTurnEnd() {
        TurnEnded!.Invoke();
    }

    public abstract void EndTurn();
    public abstract void StartTurn();



    protected void UpdateUI(int deltaHealth = 0, int deltaShield = 0) {
        HealthChange?.Invoke(deltaHealth);
        ShieldChange?.Invoke(deltaShield);
    }

    public void AddAttackModifier(int value) {
        _dmgModifier.AddModifier(value);

    }

    public void AddDefenceModifier(int value) {
        _defenceModifier.AddModifier(value);
    }

    public abstract void SwapShieldWithEnemy();

    public void RemoveShield() {
        _shield = 0;
    }

    public void SkipTurn(int value) {
        _skipTurn = value;
    }


    public void SetBuffMultihit(int value) {
        _buffMultihit = value;
    }

    public abstract void AttackEnemy(int value);
    protected void CheckDebuffsAndBuffs(ActivationTimeEnum activation, int? value = null) {
        for (int i = BuffList.Count; i > 0;) {
            i = i - 1;
            if (BuffList[i].ActivateEffect(this, activation, value) == false) {
                BuffList.RemoveAt(i);
            }

        }
        for (int i = DebuffList.Count; i > 0;) {
            i = i - 1;
            if (DebuffList[i].ActivateEffect(this, activation, value) == false) {
                DebuffList.RemoveAt(i);
            }

        }
    }

    protected void InvokeEndTurn() {
        TurnEnded?.Invoke();
    }
}
