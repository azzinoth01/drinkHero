using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class AddCardDataToDatabase : MonoBehaviour {
    public string databaseName;
    private string _connectionString;

    public CardDatabase dataToAdd;
    public List<CardDatabase> databaseList;

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

        databaseList = database.GetDatabaseItemList<CardDatabase>();



        dbCon.Close();
    }
    [ContextMenu("Update Database List")]
    public void UpdateDatabaseList() {
        _connectionString = Application.dataPath;

        int pos = _connectionString.LastIndexOf("/");

        _connectionString = _connectionString.Substring(0, pos);

        _connectionString = _connectionString + "/" + databaseName;

        SQLiteConnection dbCon = new SQLiteConnection(_connectionString);

        foreach (CardDatabase data in databaseList) {
            data.Db = dbCon;
            data.UpdateDatabaseItem();
        }

        dbCon.Close();
    }
}
