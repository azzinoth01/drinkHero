
#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Serializable, Table("User")]
public class UserDatabase : DatabaseItem {


#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private int _money;
    [SerializeField] private int _crystalBottles;
    [SerializeField] private List<HeroToUserDatabase> _heroToUserDatabasesList;


    public static Dictionary<string, UserDatabase> _cachedData = new Dictionary<string, UserDatabase>();

#endif
#if SERVER
    private int _id;
    private string _name;
    private int _money;
    private int _crystalBottles;
    private List<HeroToUserDatabase> _heroToUserDatabasesList;

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

    [Column("Money")]
    public int Money {
        get {
            return _money;
        }

        set {
            _money = value;
        }
    }
    [Column("CrystalBottles")]
    public int CrystalBottles {
        get {
            return _crystalBottles;
        }

        set {
            _crystalBottles = value;
        }
    }





#if SERVER
    public List<HeroToUserDatabase> HeroToUserDatabasesList {
        get {
            _heroToUserDatabasesList = DatabaseManager.GetDatabaseList<HeroToUserDatabase>("RefHero", Id);
            return _heroToUserDatabasesList;
        }

    }
#endif
#if CLIENT
    public List<HeroToUserDatabase> HeroDatabasesList {
        get {

            if (_heroToUserDatabasesList.Count != 0) {
                return _heroToUserDatabasesList;
            }
            else {


                string name = GetPropertyName();

                if (AlreadyRequested(name)) {
                    return _heroToUserDatabasesList;
                }

                RequestHeroToUser(name);
            }

            return _heroToUserDatabasesList;
        }

        set {
            _heroToUserDatabasesList = value;
        }
    }



    private void RequestHeroToUser(string name) {
        string functionCall = ClientFunctions.GetUserToHeroByKeyPair("RefUser\"" + _id + "\"");


        int index = SendRequest(functionCall, typeof(HeroToUserDatabase));
        _propertyToRequestedId[index] = name;
    }




    public static List<UserDatabase> CreateObjectDataFromString(string message) {

        List<UserDatabase> list = new List<UserDatabase>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<UserDatabase>();

        foreach (string[] obj in objectStrings) {
            UserDatabase item = new UserDatabase();

            //bool userExist = false;

            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (parameterName == mapping.PrimaryKeyColumn) {
                    if (_cachedData.TryGetValue(parameterValue, out UserDatabase existingItem)) {
                        item = existingItem;
                        //userExist = true;
                        //break;
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
            // overwrite existing values
            //if (userExist == true) {
            //    foreach (string parameter in obj) {
            //        string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
            //        string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

            //        if (parameterName == mapping.PrimaryKeyColumn) {
            //        }
            //        else if (mapping.ColumnsMapping.TryGetValue(parameterName, out string property)) {
            //            PropertyInfo info = item.GetType().GetProperty(property);
            //            DatabaseItemCreationHelper.ParseParameterValues(item, info, parameterValue);
            //        }
            //    }
            //}

            list.Add(item);
        }

        return list;
    }

    public override void RequestLoadReferenzData() {
        string name = null;





        name = nameof(HeroDatabasesList);
        if (AlreadyRequested(name) == false) {
            RequestHeroToUser(name);
        }

    }







#endif

    public UserDatabase() : base() {
        _heroToUserDatabasesList = new List<HeroToUserDatabase>();
        _id = 0;

    }






}
