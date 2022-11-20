
#if CLIENT
using System;
using System.Collections.Generic;
using UnityEngine;
#endif

[Table("Enemy"), Serializable]
public class EnemyDatabase : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _shield;
    [SerializeField] private string _spritePath;
    [NonSerialized] private List<EnemyToEnemySkill> _enemyToEnemySkills;
    private DataRequestStatusEnum _requestedEnemySkills;
#endif
#if SERVER
    private int _id;
    private int _maxHealth;
    private int _shield;
    private string _spritePath;
    private List<EnemyToEnemySkill> _enemyToEnemySkills;
    private DataRequestStatusEnum _requestedEnemySkills;
#endif

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
    public int Id {
        get {
            return _id;
        }
        set {
            _id = value;
        }

    }
#if SERVER
    public List<EnemyToEnemySkill> EnemyToEnemySkills {
        get {
            _enemyToEnemySkills = DatabaseManager.GetDatabaseList<EnemyToEnemySkill>("RefEnemy", Id);
            return _enemyToEnemySkills;
        }

    }
#endif
#if CLIENT
    public List<EnemyToEnemySkill> GetEnemySkillList(out bool waitOnData) {
        if (_enemyToEnemySkills == null && _requestedEnemySkills == DataRequestStatusEnum.NotRequested) {
            _requestedEnemySkills = DataRequestStatusEnum.Requested;
            ClientFunctions.GetEnemytoEnemySkillByKeyPair("RefEnemy\"" + _id.ToString() + "\"");
            WriteBackData writeBackData = new WriteBackData(this, GetType().GetMethod(nameof(SetEnemySkillList)), typeof(EnemyToEnemySkill));
            GlobalGameInfos.writeServerDataTo.Enqueue(writeBackData);

        }
        else if (_requestedEnemySkills == DataRequestStatusEnum.Recieved) {
            waitOnData = false;
            return _enemyToEnemySkills;
        }
        waitOnData = true;
        return null;
    }

    public void SetEnemySkillList(List<EnemyToEnemySkill> list) {
        _enemyToEnemySkills = list;
        _requestedEnemySkills = DataRequestStatusEnum.Recieved;
    }

#endif

    public EnemyDatabase() {
        _requestedEnemySkills = DataRequestStatusEnum.NotRequested;
    }

}
