#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Serializable, Table("PullHistory")]
public class PullHistoryDatabase : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int? _refUser;
    [SerializeField] private string _type;
    [SerializeField] private int? _typeID;



    public static Dictionary<string, PullHistoryDatabase> _cachedData = new Dictionary<string, PullHistoryDatabase>();

#endif
#if SERVER
    private int _id;
    private int? _refUser;
    private string _type;
    private int? _typeID;

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
    [Column("Type")]
    public string Type {
        get {
            return _type;
        }

        set {
            _type = value;
        }
    }
    [Column("TypeID")]
    public int? TypeID {
        get {
            return _typeID;
        }

        set {
            _typeID = value;
            if (_type == "Hero")
            {
                HeroHolder.Instance.GetHeroById((int)_typeID).Unlocked = true;
            }
        }
    }
    [Column("RefUser")]
    public int? RefUser {
        get {
            return _refUser;
        }

        set {
            _refUser = value;
        }
    }


#if CLIENT


    public static List<PullHistoryDatabase> CreateObjectDataFromString(string message) {

        List<PullHistoryDatabase> list = new List<PullHistoryDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<PullHistoryDatabase>();

        foreach (string[] obj in objectStrings) {
            PullHistoryDatabase item = new PullHistoryDatabase();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out PullHistoryDatabase existingItem)) {
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


}
