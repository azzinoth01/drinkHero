#if CLIENT
using System;
using System.Collections.Generic;
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


    public UserToUpradeItemDatabase() {
        _id = 0;
    }
}
