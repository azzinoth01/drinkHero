using System;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;


[Table("Enemy"), Serializable]
public class EnemyDatabase : DatabaseItem {

    [SerializeField] private int _id;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _shield;
    [SerializeField] private string _spritePath;

    [Column("MaxHealth")]
    public int MaxHealth {
        get {
            return _maxHealth;
        }

        set {
            _maxHealth = value;
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

    [Column("SpritePath")]
    public string SpritePath {
        get {
            return _spritePath;
        }

        set {
            _spritePath = value;
        }
    }

    [Column("ID"), PrimaryKey, AutoIncrement]
    public int Id {
        get {
            return _id;
        }
        set {
            _id = value;
        }

    }

    public EnemyDatabase() {

    }


    //public void InsertIntoDatabase(SQLiteConnection db) {


    //    //string sqlCommand = "INSERT INTO Enemy(MaxHealth,Shield,SpritePath) VALUES(@MaxHealth,@Shield,@SpritePath)";

    //    //SQLiteCommand command = db.CreateCommand(sqlCommand);

    //    //command.Bind("@MaxHealth", _maxHealth);
    //    //command.Bind("@Shield", _shield);
    //    //command.Bind("@SpritePath", _spritePath);


    //    //command.ExecuteNonQuery();

    //    db.Insert(this);

    //}


    //public static List<EnemyDatabase> GetEnemyFromDatabase(SQLiteConnection db) {


    //    string sqlCommand = "SELECT * FROM Enemy";

    //    SQLiteCommand command = db.CreateCommand(sqlCommand);

    //    // table mapping funktioniert automatisch anhand der getter und settter Namen

    //    List<EnemyDatabase> list = command.ExecuteQuery<EnemyDatabase>();





    //    return list;
    //}

    //public static EnemyDatabase GetEnemyFromDatabase(SQLiteConnection db, int id) {
    //    //string sqlCommand = "SELECT * FROM Enemy WHERE ID = @id";

    //    //SQLiteCommand command = db.CreateCommand(sqlCommand);

    //    //command.Bind("@id", id);

    //    //EnemyDatabase enemy = command.ExecuteQuery<EnemyDatabase>()[0];

    //    //db.Find<EnemyDatabase>(id);

    //    return db.Find<EnemyDatabase>(id);
    //}

    //public void UpdateDatabaseValues(SQLiteConnection db) {
    //    //string sqlCommand = "UPDATE Enemy SET MaxHealth,Shield,SpritePath WHERE ID = @id";

    //    //SQLiteCommand command = db.CreateCommand(sqlCommand);

    //    //command.Bind("@id", _id);

    //    //command.ExecuteNonQuery();

    //    db.Update(this);
    //}
}
