#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Serializable, Table("HeroToUser")]
public class HeroToUserDatabase : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int? _refUser;
    [SerializeField] private int? _refHero;
    [SerializeField] private HeroDatabase _hero;
    private UserDatabase _user;

    private static Dictionary<string, HeroToUserDatabase> _cachedData = new Dictionary<string, HeroToUserDatabase>();

#endif
#if SERVER
    private int _id;
    private int? _refUser;
    private int? _refHero;
    private HeroDatabase _hero;
    private UserDatabase _user;

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
    [Column("RefHero")]
    public int? RefHero {
        get {
            return _refHero;
        }

        set {
            _refHero = value;
        }
    }


#if SERVER
    public HeroDatabase Hero {
        get {
            if (_refHero == null) {
                return null;
            }
            _hero = DatabaseManager.GetDatabaseItem<HeroDatabase>(_refHero);

            return _hero;
        }

        set {
            if (_hero == null) {
                _refHero = null;
            }
            else {
                _refHero = value.Id;
            }
            _hero = value;
        }
    }

    public UserDatabase EnemySkill {
        get {
            if (_refUser == null) {
                return null;
            }
            _user = DatabaseManager.GetDatabaseItem<UserDatabase>(_refUser);
            return _user;
        }

        set {
            if (_user == null) {
                _refUser = null;
            }
            else {
                _refUser = value.Id;
            }
            _user = value;
        }
    }
#endif
#if CLIENT

    public UserDatabase User {
        get {
            if (_refUser == null) {
                return null;
            }
            if (_user != null) {
                return _user;
            }


            string name = GetPropertyName();

            if (AlreadyRequested(name)) {
                return null;
            }

            RequestUser(name);


            return null;

        }

        set {
            _user = value;
        }
    }
    public HeroDatabase Hero {
        get {
            if (_refHero == null) {
                return null;
            }
            if (_hero != null) {
                return _hero;
            }


            string name = GetPropertyName();

            if (AlreadyRequested(name)) {
                return null;
            }

            RequestHero(name);


            return null;

        }

        set {
            _hero = value;
        }
    }


    public static List<HeroToUserDatabase> CreateObjectDataFromString(string message) {

        List<HeroToUserDatabase> list = new List<HeroToUserDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<HeroToUserDatabase>();

        foreach (string[] obj in objectStrings) {
            HeroToUserDatabase item = new HeroToUserDatabase();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out HeroToUserDatabase existingItem)) {
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

    private void RequestHero(string name) {
        string functionCall = ClientFunctions.GetHeroDatabaseByKeyPair("ID\"" + _refHero + "\"");
        int index = SendRequest(functionCall, typeof(HeroDatabase));
        _propertyToRequestedId[index] = name;
    }
    private void RequestUser(string name) {
        string functionCall = ClientFunctions.GetUserByKeyPair("ID\"" + _refUser + "\"");
        int index = SendRequest(functionCall, typeof(UserDatabase));
        _propertyToRequestedId[index] = name;
    }


    public override void RequestLoadReferenzData() {
        string name = null;

        name = nameof(Hero);
        if (AlreadyRequested(name) == false) {
            RequestHero(name);
        }
        name = nameof(User);
        if (AlreadyRequested(name) == false) {
            RequestUser(name);
        }

    }

#endif
    public HeroToUserDatabase() : base() {

        _id = 0;

    }

}
