#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Serializable, Table("UserToUpradeItem")]
public class UserToUpradeItemDatabase : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int? _refUser;
    [SerializeField] private int? _refItem;
    [SerializeField] private UserDatabase _user;
    [SerializeField] private UpgradeItemDatabase _item;
    [SerializeField] private int _amount;


    public static Dictionary<string, UserToUpradeItemDatabase> _cachedData = new Dictionary<string, UserToUpradeItemDatabase>();

#endif
#if SERVER
    private int _id;
    private int? _refUser;
    private int? _refItem;
    private int _amount;
    private UserDatabase _user;
    private UpgradeItemDatabase _item;

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
    [Column("RefUser")]
    public int? RefUser {
        get {
            return _refUser;
        }

        set {
            _refUser = value;
        }
    }
    [Column("RefItem")]
    public int? RefItem {
        get {
            return _refItem;
        }

        set {
            _refItem = value;
        }
    }

    [Column("Amount")]
    public int Amount {
        get {
            return _amount;
        }

        set {
            _amount = value;
        }
    }

#if CLIENT

    public UpgradeItemDatabase Item {
        get {
            if (_refItem == null) {
                return null;
            }
            if (_item != null) {
                return _item;
            }


            string name = GetPropertyName();

            if (AlreadyRequested(name)) {
                return null;
            }

            RequestItem(name);


            return null;

        }

        set {
            _item = value;
        }
    }

    private void RequestItem(string name) {
        string functionCall = ClientFunctions.GetUpgradeItemDatabaseByKey("ID\"" + _refItem + "\"");


        int index = SendRequest(functionCall, typeof(UpgradeItemDatabase));
        _propertyToRequestedId[index] = name;
    }

    public override void RequestLoadReferenzData() {
        string name = null;

        name = nameof(Item);
        if (AlreadyRequested(name) == false) {
            RequestItem(name);
        }
    }



    public static List<UserToUpradeItemDatabase> CreateObjectDataFromString(string message) {

        List<UserToUpradeItemDatabase> list = new List<UserToUpradeItemDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<UserToUpradeItemDatabase>();

        foreach (string[] obj in objectStrings) {
            UserToUpradeItemDatabase item = new UserToUpradeItemDatabase();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out UserToUpradeItemDatabase existingItem)) {
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


    public UserToUpradeItemDatabase() {
        _id = 0;
    }
}
