#if CLIENT
using System;
using System.Collections.Generic;
using UnityEngine;
#endif

[Serializable, Table("GachaCategorieToItem")]
public class GachaCategorieToItemDatabase : DatabaseItem {


#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int? _refGachaCategorie;
    [SerializeField] private int? _refItem;
    [SerializeField] private int _weigthedValue;
    [SerializeField] private UpgradeItemDatabase _item;
    [SerializeField] private GachaCategorieDatabase _gachaCategorie;


    public static Dictionary<string, GachaCategorieToItemDatabase> _cachedData = new Dictionary<string, GachaCategorieToItemDatabase>();

#endif
#if SERVER
    private int _id;
    private int? _refGachaCategorie;
    private int? _refItem;
    private int _weigthedValue;
    private UpgradeItemDatabase _item;
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
    [Column("RefItem")]
    public int? RefItem {
        get {
            return _refItem;
        }

        set {
            _refItem = value;
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





}
