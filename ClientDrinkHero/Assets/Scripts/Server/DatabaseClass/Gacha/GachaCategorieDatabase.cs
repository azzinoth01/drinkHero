#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Serializable, Table("GachaCategorie")]
public class GachaCategorieDatabase : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private List<GachaCategorieToGachaItemDatabase> _gachaitemList;




    public static Dictionary<string, GachaCategorieDatabase> _cachedData = new Dictionary<string, GachaCategorieDatabase>();

#endif
#if SERVER
    private int _id;
    private string _name;
    private List<GachaCategorieToGachaItemDatabase> _gachaitemList;


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

#if SERVER
    public List<GachaCategorieToGachaItemDatabase> ItemList {
        get {
            Console.Write("Current ID: " + _id);
            _gachaitemList = DatabaseManager.GetDatabaseList<GachaCategorieToGachaItemDatabase>("RefGachaCategorie", _id);
            return _gachaitemList;
        }

    }


#endif



#if CLIENT
    public static List<GachaCategorieDatabase> CreateObjectDataFromString(string message) {

        List<GachaCategorieDatabase> list = new List<GachaCategorieDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<GachaCategorieDatabase>();

        foreach (string[] obj in objectStrings) {
            GachaCategorieDatabase item = new GachaCategorieDatabase();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out GachaCategorieDatabase existingItem)) {
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

    public GachaCategorieDatabase() {
        _gachaitemList = new List<GachaCategorieToGachaItemDatabase>();

    }
}
