using System;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[Table("Card"), Serializable]
public class CardDatabase : DatabaseItem {
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private int _attack;
    [SerializeField] private int _shield;
    [SerializeField] private int _health;
    [SerializeField] private int _cost;
    [SerializeField] private string _spritePath;
    [SerializeField] private int? _refUpgradeTo;
    private CardDatabase _upgradeTo;

    [Column("ID"), PrimaryKey, AutoIncrement]
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
    [Column("Attack")]
    public int Attack {
        get {
            return _attack;
        }

        set {
            _attack = value;
        }
    }
    [Column("Shield")]
    public int Health {
        get {
            return _health;
        }

        set {
            _health = value;
        }
    }
    [Column("Health")]
    public int Shield {
        get {
            return _shield;
        }

        set {
            _shield = value;
        }
    }
    [Column("Cost")]
    public int Cost {
        get {
            return _cost;
        }

        set {
            _cost = value;
        }
    }
    [Column("SpritePath")]
    public string SpritePath {
        get {
            return _spritePath;
        }

        set {
            _spritePath = value;
        }
    }
    [Column("RefUpgradeTo")]
    public int? RefUpgradeTo {
        get {
            return _refUpgradeTo;
        }

        set {
            _refUpgradeTo = value;


        }
    }


    public CardDatabase UpgradeTo {
        get {
            if (_refUpgradeTo == null) {
                return null;
            }
            _upgradeTo = GetDatabaseItem<CardDatabase>((int)_refUpgradeTo);
            return _upgradeTo;

        }

        set {
            if (value == null) {
                _refUpgradeTo = null;
            }
            else {
                _refUpgradeTo = value.Id;
            }

            _upgradeTo = value;
        }
    }

    public CardDatabase() {

    }

    //public void InsertIntoDatabase(SQLiteConnection db) {

    //    db.Insert(this);

    //}


    //public static List<CardDatabase> GetCardFromDatabase(SQLiteConnection db) {


    //    string sqlCommand = "SELECT * FROM Card";

    //    SQLiteCommand command = db.CreateCommand(sqlCommand);


    //    List<CardDatabase> list = command.ExecuteQuery<CardDatabase>();
    //    foreach (CardDatabase c in list) {
    //        c.ResolveReferenzKeys(db);
    //    }


    //    return list;
    //}

    //public static CardDatabase GetCardFromDatabase(SQLiteConnection db, int id) {


    //    return db.Find<CardDatabase>(id);
    //}

    //public void UpdateDatabaseValues(SQLiteConnection db) {


    //    db.Update(this);
    //}


}
