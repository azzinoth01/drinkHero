#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Serializable, Table("GachaCategorieToGachaItem")]
public class GachaCategorieToGachaItemDatabase : DatabaseItem {


#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int? _refGachaCategorie;
    [SerializeField] private int? _refGachaItem;
    [SerializeField] private string _gachaItemType;
    [SerializeField] private int _weigthedValue;

    public static Dictionary<string, GachaCategorieToGachaItemDatabase> _cachedData = new Dictionary<string, GachaCategorieToGachaItemDatabase>();

#endif
#if SERVER
    private int _id;
    private int? _refGachaCategorie;
    private int? _refGachaItem;
    private string _gachaItemType;
    private int _weigthedValue;
    private GachaCategorieDatabase _gachaCategorie;

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
    [Column("RefGachaCategorie")]
    public int? RefGachaCategorie {
        get {
            return _refGachaCategorie;
        }

        set {
            _refGachaCategorie = value;
        }
    }
    [Column("RefGachaItem")]
    public int? RefGachaItem {
        get {
            return _refGachaItem;
        }

        set {
            _refGachaItem = value;
        }
    }
    [Column("WeigthedValue")]
    public int WeigthedValue {
        get {
            return _weigthedValue;
        }

        set {
            _weigthedValue = value;
        }
    }

    [Column("GachaItemType")]
    public string GachaItemType {
        get {
            return _gachaItemType;
        }

        set {
            _gachaItemType = value;
        }
    }

#if SERVER


    public GachaCategorieDatabase GachaCategorie {
        get {
            _gachaCategorie = DatabaseManager.GetDatabaseItem<GachaCategorieDatabase>(_refGachaCategorie);
            return _gachaCategorie;
        }

    }

#endif


#if CLIENT
    public static List<GachaCategorieToGachaItemDatabase> CreateObjectDataFromString(string message) {

        List<GachaCategorieToGachaItemDatabase> list = new List<GachaCategorieToGachaItemDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<GachaCategorieToGachaItemDatabase>();

        foreach (string[] obj in objectStrings) {
            GachaCategorieToGachaItemDatabase item = new GachaCategorieToGachaItemDatabase();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out GachaCategorieToGachaItemDatabase existingItem)) {
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
