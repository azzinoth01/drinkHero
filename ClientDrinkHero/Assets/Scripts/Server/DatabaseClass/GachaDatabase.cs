#if CLIENT
using System;
using System.Collections.Generic;
using UnityEngine;
#endif

[Serializable, Table("Gacha")]
public class GachaDatabase : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _costType;
    [SerializeField] private int _costSingelPull;
    [SerializeField] private int _costMultiPull;
    [SerializeField] private int _multiPullAmount;
    [SerializeField] private List<GachaToGachaCategorieDatabase> _gachaCetegorieList;
    public static Dictionary<string, GachaDatabase> _cachedData = new Dictionary<string, GachaDatabase>();



#endif
#if SERVER
    private int _id;
    private string _name;
    private string _costType;
    private int _costSingelPull;
    private int _costMultiPull;
    private int _multiPullAmount;
    private List<GachaToGachaCategorieDatabase> _gachaCetegorieList;

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

    [Column("CostType")]
    public string CostType {
        get {
            return _costType;
        }

        set {
            _costType = value;
        }
    }
    [Column("SingelPullCost")]
    public int CostSingelPull {
        get {
            return _costSingelPull;
        }

        set {
            _costSingelPull = value;
        }
    }
    [Column("MultiPullCost")]
    public int CostMultiPull {
        get {
            return _costMultiPull;
        }

        set {
            _costMultiPull = value;
        }
    }
    [Column("MultiPullAmount")]
    public int MultiPullAmount {
        get {
            return _multiPullAmount;
        }

        set {
            _multiPullAmount = value;
        }
    }

#if SERVER

    public List<GachaToGachaCategorieDatabase> GachaCetegorieList {
        get {
            _gachaCetegorieList = DatabaseManager.GetDatabaseList<GachaToGachaCategorieDatabase>("RefGacha", _id);
            return _gachaCetegorieList;
        }

    }

#endif

    public GachaDatabase() {
        _gachaCetegorieList = new List<GachaToGachaCategorieDatabase>();
    }
}
