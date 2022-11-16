using System;
using System.Collections.Generic;
using UnityEngine;


[Table("EnemySkill"), Serializable]
public class EnemySkillDatabase : DatabaseItem {



    [SerializeField] private long _id;
    [SerializeField] private string _name;
    [SerializeField] private int _minAttack;
    [SerializeField] private int _minShield;
    [SerializeField] private int _minHealth;
    [SerializeField] private int _maxAttack;
    [SerializeField] private int _maxShield;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _cooldown;

    [NonSerialized] private List<EnemyToEnemySkill> _enemyToEnemySkills;

    [Column("ID"), PrimaryKey]
    public long Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }
    [Column("Name")]
    public string Name {
        get {
            return _name;
        }

        set {
            _name = value;
        }
    }
    [Column("MinAttack")]
    public int MinAttack {
        get {
            return _minAttack;
        }

        set {
            _minAttack = value;
        }
    }
    [Column("MinShield")]
    public int MinShield {
        get {
            return _minShield;
        }

        set {
            _minShield = value;
        }
    }
    [Column("MinHealth")]
    public int MinHealth {
        get {
            return _minHealth;
        }

        set {
            _minHealth = value;
        }
    }
    [Column("MaxAttack")]
    public int MaxAttack {
        get {
            return _maxAttack;
        }

        set {
            _maxAttack = value;
        }
    }
    [Column("MaxShield")]
    public int MaxShield {
        get {
            return _maxShield;
        }

        set {
            _maxShield = value;
        }
    }
    [Column("MaxHealth")]
    public int MaxHealth {
        get {
            return _maxHealth;
        }

        set {
            _maxHealth = value;
        }
    }
    [Column("Cooldown")]
    public int Cooldown {
        get {
            return _cooldown;
        }

        set {
            _cooldown = value;
        }
    }

    public List<EnemyToEnemySkill> EnemyToEnemySkills {
        get {
            _enemyToEnemySkills = DatabaseManager.GetDatabaseList<EnemyToEnemySkill>("RefEnemySkill", _id);
            return _enemyToEnemySkills;
        }


    }

    public EnemySkillDatabase() {

    }



}


