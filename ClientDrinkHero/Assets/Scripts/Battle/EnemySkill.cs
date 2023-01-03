using System;
using UnityEngine;

[Serializable]
public class EnemySkill : IGetUpdateFromServer {
    private long _id;
    [SerializeField] private string _name;
    [SerializeField] private int _minAttack;
    [SerializeField] private int _minShield;
    [SerializeField] private int _minHealth;

    [SerializeField] private int _maxAttack;
    [SerializeField] private int _maxSchield;
    [SerializeField] private int _maxHealth;

    [SerializeField] private int _cooldown;
    [SerializeField] private int _currentCooldown;

    private EnemySkillDatabase _enemySkillData;


    private bool _isWaitingOnServer;

    public int MinAttack {
        get {
            return _minAttack;
        }

        set {
            _minAttack = value;
        }
    }

    public int MinShield {
        get {
            return _minShield;
        }

        set {
            _minShield = value;
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

    public EnemySkillDatabase EnemySkillData {
        get {
            return _enemySkillData;
        }

        set {
            _enemySkillData = value;
            if (_enemySkillData != null) {
                if (ConvertEnemySkillData() == false) {
                    //GlobalGameInfos.Instance.WaitOnServerObjects.Add(this);
                }
            }

        }
    }

    public long Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }

    public bool IsWaitingOnServer {
        get {
            return _isWaitingOnServer;
        }

        set {
            _isWaitingOnServer = value;
        }
    }


    private bool ConvertEnemySkillData() {

        if (_enemySkillData == null) {
            return false;
        }

        _id = _enemySkillData.Id;
        _name = _enemySkillData.Name;
        _minAttack = _enemySkillData.MinAttack;
        _maxAttack = _enemySkillData.MaxAttack;
        _minHealth = _enemySkillData.MinHealth;
        _maxHealth = _enemySkillData.MaxHealth;
        _minShield = _enemySkillData.MinShield;
        _maxSchield = _enemySkillData.MaxShield;



        return true;
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

    public bool GetUpdateFromServer() {
        return ConvertEnemySkillData();
    }




    public EnemySkill() {

    }

}
