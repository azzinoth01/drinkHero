
#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Table("EnemySkill"), Serializable]
public class EnemySkillDatabase : DatabaseItem {


#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private int _minAttack;
    [SerializeField] private int _minShield;
    [SerializeField] private int _minHealth;
    [SerializeField] private int _maxAttack;
    [SerializeField] private int _maxShield;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _cooldown;
    [NonSerialized] private List<EnemyToEnemySkill> _enemyToEnemySkills;

    private static Dictionary<string, EnemySkillDatabase> _cachedData = new Dictionary<string, EnemySkillDatabase>();

#endif
#if SERVER
    private int _id;
    private string _name;
    private int _minAttack;
    private int _minShield;
    private int _minHealth;
    private int _maxAttack;
    private int _maxShield;
    private int _maxHealth;
    private int _cooldown;
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
#if SERVER
    public List<EnemyToEnemySkill> EnemyToEnemySkills {
        get {
            _enemyToEnemySkills = DatabaseManager.GetDatabaseList<EnemyToEnemySkill>("RefEnemySkill", _id);
            return _enemyToEnemySkills;
        }


    }
#endif

#if CLIENT


    public static List<EnemySkillDatabase> CreateObjectDataFromString(string message) {

        List<EnemySkillDatabase> list = new List<EnemySkillDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<EnemySkillDatabase>();

        foreach (string[] obj in objectStrings) {
            EnemySkillDatabase item = new EnemySkillDatabase();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out EnemySkillDatabase existingItem)) {
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

#endif


    public EnemySkillDatabase() {

    }



}


