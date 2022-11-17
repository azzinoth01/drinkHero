using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable, Table("EnemyToEnemySkill")]
public class EnemyToEnemySkill : DatabaseItem {
    [SerializeField] private long _id;
    [SerializeField] private string _refEnemy;
    [SerializeField] private string _refEnemySkill;
    [NonSerialized] private EnemyDatabase _enemy;
    [NonSerialized] private EnemySkillDatabase _enemySkill;

    private DataRequestStatusEnum _requestedEnemySkills;

    [Column("ID"), PrimaryKey]
    public long Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }
    [Column("RefEnemy")]
    public string RefEnemy {
        get {
            return _refEnemy;
        }

        set {
            _refEnemy = value;
        }
    }
    [Column("RefEnemySkill")]
    public string RefEnemySkill {
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
#if SERVER
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
#endif
#if CLIENT
    public EnemySkillDatabase GetEnemySkill(out bool waitOnData) {
        if (_enemySkill == null && _requestedEnemySkills == DataRequestStatusEnum.NotRequested) {
            _requestedEnemySkills = DataRequestStatusEnum.Requested;
            ClientFunctions.GetEnemySkillByKeyPair("ID\"" + _refEnemySkill + "\"");
            WriteBackData writeBackData = new WriteBackData(this, GetType().GetMethod(nameof(SetEnemySkill)), typeof(EnemySkillDatabase));
            GlobalGameInfos.writeServerDataTo.Enqueue(writeBackData);

        }
        else if (_requestedEnemySkills == DataRequestStatusEnum.Recieved) {
            waitOnData = false;
            return _enemySkill;
        }
        waitOnData = true;
        return null;
    }

    public void SetEnemySkill(List<EnemySkillDatabase> list) {
        _enemySkill = list[0];
        _requestedEnemySkills = DataRequestStatusEnum.Recieved;
    }

#endif
    public EnemyToEnemySkill() {
        _requestedEnemySkills = DataRequestStatusEnum.NotRequested;
    }

}
