#if CLIENT
using System;
using System.Collections.Generic;
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



    public static Dictionary<string, GachaToGachaCategorieDatabase> _cachedData = new Dictionary<string, GachaToGachaCategorieDatabase>();

#endif
#if SERVER
    private int _id;
    private int? _refGachaCategorie;
    private int? _refGachaItem;
    private string _gachaItemType;
    private int _weigthedValue;

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




}
