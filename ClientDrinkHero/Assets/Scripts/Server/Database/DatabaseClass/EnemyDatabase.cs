
using System;
using System.Collections.Generic;
using UnityEngine;


[Table("Enemy"), Serializable]
public class EnemyDatabase : DatabaseItem {

    [SerializeField] private long _id;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _shield;
    [SerializeField] private string _spritePath;

    [NonSerialized] private List<EnemyToEnemySkill> _enemyToEnemySkills;

    [Column("MaxHealth")]
    public int MaxHealth {
        get {
            return _maxHealth;
        }

        set {
            _maxHealth = value;
        }
    }
    [Column("Shield")]
    public int Shield {
        get {
            return _shield;
        }

        set {
            _shield = value;
        }
    }

    [Column("SpritePath")]
    public string SpritePath {
        get {
            return _spritePath;
        }

        set {
            _spritePath = value;
        }
    }

    [Column("ID"), PrimaryKey]
    public long Id {
        get {
            return _id;
        }
        set {
            _id = value;
        }

    }

    public List<EnemyToEnemySkill> EnemyToEnemySkills {
        get {
            _enemyToEnemySkills = DatabaseManager.GetDatabaseList<EnemyToEnemySkill>("RefEnemy", Id);
            return _enemyToEnemySkills;
        }

    }

    public EnemyDatabase() {

    }

}
