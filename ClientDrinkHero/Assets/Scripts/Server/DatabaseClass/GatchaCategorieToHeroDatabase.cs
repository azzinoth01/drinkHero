#if CLIENT
using System;
using System.Collections.Generic;
using UnityEngine;
#endif

[Serializable, Table("GachaCategorieToHero")]
public class GatchaCategorieToHeroDatabase : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int? _refGachaCategorie;
    [SerializeField] private int? _refHero;
    [SerializeField] private int _weigthedValue;
    [SerializeField] private HeroDatabase _hero;
    [SerializeField] private GachaCategorieDatabase _gachaCategorie;


    public static Dictionary<string, GatchaCategorieToHeroDatabase> _cachedData = new Dictionary<string, GatchaCategorieToHeroDatabase>();

#endif
#if SERVER
     private int _id;
     private int? _refGachaCategorie;
     private int? _refHero;
     private int _weigthedValue;
     private HeroDatabase _hero;
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
    [Column("RefHero")]
    public int? RefHero {
        get {
            return _refHero;
        }

        set {
            _refHero = value;
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


    private void RequestHero(string name) {
        string functionCall = ClientFunctions.GetHeroDatabaseByKeyPair("ID\"" + _refHero + "\"");
        int index = SendRequest(functionCall, typeof(HeroDatabase));
        _propertyToRequestedId[index] = name;
    }
    private void RequestGachaCategorie(string name) {
        //string functionCall = ClientFunctions.GetUserByKeyPair("ID\"" + _refUser + "\"");
        //int index = SendRequest(functionCall, typeof(UserDatabase));
        //_propertyToRequestedId[index] = name;
    }


#endif




}
