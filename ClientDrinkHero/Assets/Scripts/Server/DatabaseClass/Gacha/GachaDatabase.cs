#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Serializable, Table("Gacha")]
public class GachaDatabase : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _costType;
    [SerializeField] private int _costSingelPull;
    [SerializeField] private int _costMultiPull;
    [SerializeField] private int _multiPullAmount;
    [SerializeField] private List<GachaToGachaCategorieDatabase> _gachaCetegorieList;
    public static Dictionary<string, GachaDatabase> _cachedData = new Dictionary<string, GachaDatabase>();



#endif
#if SERVER
    private int _id;
    private string _name;
    private string _costType;
    private int _costSingelPull;
    private int _costMultiPull;
    private int _multiPullAmount;
    private List<GachaToGachaCategorieDatabase> _gachaCetegorieList;

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

    [Column("CostType")]
    public string CostType {
        get {
            return _costType;
        }

        set {
            _costType = value;
        }
    }
    [Column("SingelPullCost")]
    public int CostSingelPull {
        get {
            return _costSingelPull;
        }

        set {
            _costSingelPull = value;
        }
    }
    [Column("MultiPullCost")]
    public int CostMultiPull {
        get {
            return _costMultiPull;
        }

        set {
            _costMultiPull = value;
        }
    }
    [Column("MultiPullAmount")]
    public int MultiPullAmount {
        get {
            return _multiPullAmount;
        }

        set {
            _multiPullAmount = value;
        }
    }

#if SERVER

    public List<GachaToGachaCategorieDatabase> GachaCetegorieList {
        get {
            _gachaCetegorieList = DatabaseManager.GetDatabaseList<GachaToGachaCategorieDatabase>("RefGacha", _id);
            return _gachaCetegorieList;
        }

    }

#endif


#if CLIENT
    public static List<GachaDatabase> CreateObjectDataFromString(string message) {

        List<GachaDatabase> list = new List<GachaDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<GachaDatabase>();

        foreach (string[] obj in objectStrings) {
            GachaDatabase item = new GachaDatabase();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out GachaDatabase existingItem)) {
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
    public GachaDatabase() {
        _gachaCetegorieList = new List<GachaToGachaCategorieDatabase>();
    }
}
