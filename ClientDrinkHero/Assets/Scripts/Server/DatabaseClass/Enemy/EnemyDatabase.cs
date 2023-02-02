
#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Table("Enemy"), Serializable]
public class EnemyDatabase : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _shield;
    [SerializeField] private string _spritePath;
    [SerializeField] private bool _isBoss;
    [SerializeField] private int _moneyDrop;
    private List<EnemyToEnemySkill> _enemyToEnemySkills;

    public static Dictionary<string, EnemyDatabase> _cachedData = new Dictionary<string, EnemyDatabase>();



#endif
#if SERVER
    private int _id;
    private int _maxHealth;
    private int _shield;
    private string _spritePath;
    private bool _isBoss;
    private int _moneyDrop;
    private List<EnemyToEnemySkill> _enemyToEnemySkills;

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


    [Column("IsBoss")]
    public bool IsBoss {
        get {
            return _isBoss;
        }

        set {
            _isBoss = value;
        }
    }
    [Column("MoneyDrop")]
    public int MoneyDrop {
        get {
            return _moneyDrop;
        }

        set {
            _moneyDrop = value;
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
    public List<EnemyToEnemySkill> EnemyToEnemySkills {
        get {

            if (_enemyToEnemySkills.Count != 0) {
                return _enemyToEnemySkills;
            }
            else {


                string name = GetPropertyName();

                if (AlreadyRequested(name)) {
                    return _enemyToEnemySkills;
                }

                RequestEnemySkills(name);
            }

            return _enemyToEnemySkills;
        }

        set {
            _enemyToEnemySkills = value;
        }
    }



    private void RequestEnemySkills(string name) {
        string functionCall = ClientFunctions.GetEnemytoEnemySkillByKeyPair("RefEnemy\"" + _id + "\"");
        int index = SendRequest(functionCall, typeof(EnemyToEnemySkill));
        _propertyToRequestedId[index] = name;
    }


    public static List<EnemyDatabase> CreateObjectDataFromString(string message) {

        List<EnemyDatabase> list = new List<EnemyDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<EnemyDatabase>();

        foreach (string[] obj in objectStrings) {
            EnemyDatabase item = new EnemyDatabase();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out EnemyDatabase existingItem)) {
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

    public override void RequestLoadReferenzData() {
        string name = null;

        name = nameof(EnemyToEnemySkills);
        if (AlreadyRequested(name) == false) {
            RequestEnemySkills(name);
        }

    }


#endif

    public EnemyDatabase() : base() {
        _enemyToEnemySkills = new List<EnemyToEnemySkill>();
    }

}
