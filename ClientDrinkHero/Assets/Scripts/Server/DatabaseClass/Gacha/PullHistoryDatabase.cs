#if CLIENT
using System;
using System.Collections.Generic;
using UnityEngine;
#endif

[Serializable, Table("PullHistory")]
public class PullHistoryDatabase : DatabaseItem {

#if CLIENT
    [SerializeField] private int _id;
    [SerializeField] private int? _refUser;
    [SerializeField] private string _type;
    [SerializeField] private int? _typeID;



    public static Dictionary<string, PullHistoryDatabase> _cachedData = new Dictionary<string, PullHistoryDatabase>();

#endif
#if SERVER
    private int _id;
    private int? _refUser;
    private string _type;
    private int? _typeID;

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
    [Column("Type")]
    public string Type {
        get {
            return _type;
        }

        set {
            _type = value;
        }
    }
    [Column("TypeID")]
    public int? TypeID {
        get {
            return _typeID;
        }

        set {
            _typeID = value;
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
}
