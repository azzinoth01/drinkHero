
#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
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

    private static Dictionary<string, EnemyToEnemySkill> _cachedData = new Dictionary<string, EnemyToEnemySkill>();

#endif
#if SERVER
    private int _id;
    private int? _refEnemy;
    private int? _refEnemySkill;
    private EnemyDatabase _enemy;
    private EnemySkillDatabase _enemySkill;

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
            if (_enemySkill != null) {
                return _enemySkill;
            }


            string name = GetPropertyName();

            if (AlreadyRequested(name)) {
                return null;
            }

            RequestEnemySkill(name);


            return null;

        }

        set {
            _enemySkill = value;
        }
    }
    public EnemyDatabase Enemy {
        get {
            if (_refEnemy == null) {
                return null;
            }
            if (_enemy != null) {
                return _enemy;
            }


            string name = GetPropertyName();

            if (AlreadyRequested(name)) {
                return null;
            }

            RequestEnemySkill(name);


            return null;

        }

        set {
            _enemy = value;
        }
    }


    public static List<EnemyToEnemySkill> CreateObjectDataFromString(string message) {

        List<EnemyToEnemySkill> list = new List<EnemyToEnemySkill>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<EnemyToEnemySkill>();

        foreach (string[] obj in objectStrings) {
            EnemyToEnemySkill item = new EnemyToEnemySkill();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out EnemyToEnemySkill existingItem)) {
                        item = existingItem;
                        break;
                    }
                    else {
                        _cachedData.Add(parameterValue, item);
                    }

                }

                if (mapping.ColumnsMapping.TryGetValue(parameterName, out string property)) {
                    PropertyInfo info = item.GetType().GetProperty(property);
                    DatabaseItemCreationHelper.ParseParameterValues(item, info, parameterValue);
                }
            }
            list.Add(item);
        }

        return list;
    }

    private void RequestEnemySkill(string name) {
        string functionCall = ClientFunctions.GetEnemySkillByKeyPair("ID\"" + _refEnemySkill + "\"");
        int index = SendRequest(functionCall, typeof(EnemySkillDatabase));
        _propertyToRequestedId[index] = name;
    }
    private void RequestEnemy(string name) {
        string functionCall = ClientFunctions.GetEnemyDatabaseByKeyPair("ID\"" + _refEnemy + "\"");
        int index = SendRequest(functionCall, typeof(EnemyDatabase));
        _propertyToRequestedId[index] = name;
    }


    public override void RequestLoadReferenzData() {
        string name = null;

        name = nameof(EnemySkill);
        if (AlreadyRequested(name) == false) {
            RequestEnemySkill(name);
        }
        name = nameof(Enemy);
        if (AlreadyRequested(name) == false) {
            RequestEnemy(name);
        }

    }

#endif
    public EnemyToEnemySkill() : base() {



    }

}
