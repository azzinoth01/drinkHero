using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;


public class AddEnemyDataToDatabase : MonoBehaviour {

    public string databaseName;
    private string _connectionString;

    public EnemyDatabase dataToAdd;
    public List<EnemyDatabase> databaseList;

    [ContextMenu("Add Data to Database")]
    public void TestDatabase() {

        _connectionString = Application.dataPath;

        int pos = _connectionString.LastIndexOf("/");

        _connectionString = _connectionString.Substring(0, pos);

        _connectionString = _connectionString + "/" + databaseName;

        SQLiteConnection dbCon = new SQLiteConnection(_connectionString);

        dataToAdd.Db = dbCon;

        dataToAdd.InsertDatabaseItem();

        dbCon.Close();

    }

    [ContextMenu("Get Database List")]
    public void GetDatabaseList() {
        _connectionString = Application.dataPath;

        int pos = _connectionString.LastIndexOf("/");

        _connectionString = _connectionString.Substring(0, pos);

        _connectionString = _connectionString + "/" + databaseName;

        SQLiteConnection dbCon = new SQLiteConnection(_connectionString);

        DatabaseItem database = new DatabaseItem();
        database.Db = dbCon;

        databaseList = database.GetDatabaseItemList<EnemyDatabase>();



        dbCon.Close();
    }
    [ContextMenu("Update Database List")]
    public void UpdateDatabaseList() {
        _connectionString = Application.dataPath;

        int pos = _connectionString.LastIndexOf("/");

        _connectionString = _connectionString.Substring(0, pos);

        _connectionString = _connectionString + "/" + databaseName;

        SQLiteConnection dbCon = new SQLiteConnection(_connectionString);

        foreach (EnemyDatabase data in databaseList) {
            data.Db = dbCon;
            data.UpdateDatabaseItem();
        }

        dbCon.Close();
    }
}
