
#if CLIENT
using System;
using UnityEngine;
#endif

[Serializable, Table("EnemyToEnemySkill")]
public class EnemyToEnemySkill : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int? _refEnemy;
    [SerializeField] private int? _refEnemySkill;
    [NonSerialized] private EnemyDatabase _enemy;
    [NonSerialized] private EnemySkillDatabase _enemySkill;

#endif
#if SERVER
    private int _id;
    private int? _refEnemy;
    private int? _refEnemySkill;
    private EnemyDatabase _enemy;
    private EnemySkillDatabase _enemySkill;
    private DataRequestStatusEnum _requestedEnemySkills;
#endif

    [Column("ID"), PrimaryKey]
    public int Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }
    [Column("RefEnemy")]
    public int? RefEnemy {
        get {
            return _refEnemy;
        }

        set {
            _refEnemy = value;
        }
    }
    [Column("RefEnemySkill")]
    public int? RefEnemySkill {
        get {
            return _refEnemySkill;
        }

        set {
            _refEnemySkill = value;
        }
    }


#if SERVER
    public EnemyDatabase Enemy {
        get {
            if (_refEnemy == null) {
                return null;
            }
            _enemy = DatabaseManager.GetDatabaseItem<EnemyDatabase>(_refEnemy);

            return _enemy;
        }

        set {
            if (_enemy == null) {
                _refEnemy = null;
            }
            else {
                _refEnemy = value.Id;
            }
            _enemy = value;
        }
    }

    public EnemySkillDatabase EnemySkill {
        get {
            if (_refEnemySkill == null) {
                return null;
            }
            _enemySkill = DatabaseManager.GetDatabaseItem<EnemySkillDatabase>(_refEnemySkill);
            return _enemySkill;
        }

        set {
            if (_enemySkill == null) {
                _refEnemySkill = null;
            }
            else {
                _refEnemySkill = value.Id;
            }
            _enemySkill = value;
        }
    }
#endif
#if CLIENT

    public EnemySkillDatabase EnemySkill {
        get {
            if (_refEnemySkill == null) {
                return null;
            }
            CheckRequestedData();
            if (_enemySkill != null) {
                return _enemySkill;
            }
            else {

                string name = GetPropertyName();

                if (AlreadyRequested(name)) {
                    return null;
                }


                string functionCall = ClientFunctions.GetCardDatabaseByKeyPair("ID\"" + _refEnemySkill + "\"");
                int index = SendRequest(functionCall, typeof(EnemySkillDatabase));
                _propertyToRequestedId[index] = name;

                return null;
            }
        }

        set {
            _enemySkill = value;
        }
    }


#endif
    public EnemyToEnemySkill() : base() {



    }

}
