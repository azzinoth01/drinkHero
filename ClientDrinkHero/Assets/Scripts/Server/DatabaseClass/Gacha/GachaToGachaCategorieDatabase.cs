#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Serializable, Table("GachaToGachaCategorie")]
public class GachaToGachaCategorieDatabase : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int? _refGachaCategorie;
    [SerializeField] private int? _refGacha;
    [SerializeField] private int _weigthedValue;
    [SerializeField] private GachaDatabase _gacha;
    [SerializeField] private GachaCategorieDatabase _gachaCategorie;


    public static Dictionary<string, GachaToGachaCategorieDatabase> _cachedData = new Dictionary<string, GachaToGachaCategorieDatabase>();

#endif
#if SERVER
    private int _id;
    private int? _refGachaCategorie;
    private int? _refGacha;
    private int _weigthedValue;
    private GachaDatabase _gacha;
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
    [Column("RefGacha")]
    public int? RefGacha {
        get {
            return _refGacha;
        }

        set {
            _refGacha = value;
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
#if SERVER
    public GachaDatabase Gacha {
        get {
            _gacha = DatabaseManager.GetDatabaseItem<GachaDatabase>(_refGacha);
            return _gacha;
        }

    }

    public GachaCategorieDatabase GachaCategorie {
        get {
            _gachaCategorie = DatabaseManager.GetDatabaseItem<GachaCategorieDatabase>(_refGachaCategorie);
            return _gachaCategorie;
        }

    }

#endif

#if CLIENT

    public GachaCategorieDatabase GachaCategorie {
        get {
            if (_refGachaCategorie == null) {
                return null;
            }
            if (_gachaCategorie != null) {
                return _gachaCategorie;
            }


            string name = GetPropertyName();

            if (AlreadyRequested(name)) {
                return null;
            }

            RequestGachaCategorie(name);


            return null;

        }

        set {
            _gachaCategorie = value;
        }
    }
    public GachaDatabase Gacha {
        get {
            if (_refGacha == null) {
                return null;
            }
            if (_gacha != null) {
                return _gacha;
            }


            string name = GetPropertyName();

            if (AlreadyRequested(name)) {
                return null;
            }

            RequestGacha(name);


            return null;

        }

        set {
            _gacha = value;
        }
    }


    private void RequestGacha(string name) {
        //string functionCall = ClientFunctions.GetHeroDatabaseByKeyPair("ID\"" + _refHero + "\"");
        //int index = SendRequest(functionCall, typeof(HeroDatabase));
        //_propertyToRequestedId[index] = name;
    }
    private void RequestGachaCategorie(string name) {
        //string functionCall = ClientFunctions.GetUserByKeyPair("ID\"" + _refUser + "\"");
        //int index = SendRequest(functionCall, typeof(UserDatabase));
        //_propertyToRequestedId[index] = name;
    }


#endif


#if CLIENT
    public static List<GachaToGachaCategorieDatabase> CreateObjectDataFromString(string message) {

        List<GachaToGachaCategorieDatabase> list = new List<GachaToGachaCategorieDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<GachaToGachaCategorieDatabase>();

        foreach (string[] obj in objectStrings) {
            GachaToGachaCategorieDatabase item = new GachaToGachaCategorieDatabase();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out GachaToGachaCategorieDatabase existingItem)) {
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
