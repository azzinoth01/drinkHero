using System;
using UnityEngine;


[Serializable, Table("EnemyToEnemySkill")]
public class EnemyToEnemySkill : DatabaseItem {
    [SerializeField] private long _id;
    [SerializeField] private string _refEnemy;
    [SerializeField] private string _refEnemySkill;
    [NonSerialized] private EnemyDatabase _enemy;
    [NonSerialized] private EnemySkillDatabase _enemySkill;

    [Column("ID"), PrimaryKey]
    public long Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }
    [Column("RefCard")]
    public string RefCard {
        get {
            return _refEnemy;
        }

        set {
            _refEnemy = value;
        }
    }
    [Column("RefHero")]
    public string RefHero {
        get {
            return _refEnemySkill;
        }

        set {
            _refEnemySkill = value;
        }
    }

    public EnemyDatabase Enemy {
        get {
            if (_refEnemy == null) {
                return null;
            }
            _enemy = DatabaseManager.GetDatabaseItem<EnemyDatabase>(long.Parse(_refEnemy));

            return _enemy;
        }

        set {
            if (_enemy == null) {
                _refEnemy = null;
            }
            else {
                _refEnemy = value.Id.ToString();
            }
            _enemy = value;
        }
    }

    public EnemySkillDatabase EnemySkill {
        get {
            if (_refEnemySkill == null) {
                return null;
            }
            _enemySkill = DatabaseManager.GetDatabaseItem<EnemySkillDatabase>(long.Parse(_refEnemySkill));
            return _enemySkill;
        }

        set {
            if (_enemySkill == null) {
                _refEnemySkill = null;
            }
            else {
                _refEnemySkill = value.Id.ToString();
            }
            _enemySkill = value;
        }
    }

    public EnemyToEnemySkill() {

    }

}
