using System;
using Unity.VisualScripting.Dependencies.Sqlite;

[Serializable, Table("Hero")]
public class HeroDatabase : DatabaseItem {
    private int _id;
    private int _shield;
    private int _health;
    private string _spritePath;
    private string _name;





    [Column("ID"), PrimaryKey, AutoIncrement]
    public int Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }
    [Column("Shield")]
    public int Shield {
        get {
            return _shield;
        }

        set {
            _shield = value;
        }
    }
    [Column("Health")]
    public int Health {
        get {
            return _health;
        }

        set {
            _health = value;
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
    [Column("Name")]
    public string Name {
        get {
            return _name;
        }

        set {
            _name = value;
        }
    }

    public HeroDatabase() {

    }



}
