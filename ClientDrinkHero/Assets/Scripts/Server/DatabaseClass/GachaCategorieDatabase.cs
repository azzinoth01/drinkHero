#if CLIENT
using System;
using System.Collections.Generic;
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


    public GachaCategorieDatabase() {
        _gachaitemList = new List<GachaCategorieToGachaItemDatabase>();

    }
}
