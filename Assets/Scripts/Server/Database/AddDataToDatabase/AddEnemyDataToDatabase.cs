

using Mono.Data.Sqlite;
using System.Collections.Generic;
using UnityEngine;



public class AddEnemyDataToDatabase : MonoBehaviour {

    public string databaseName;
    private string _connectionString;

    public EnemyDatabase dataToAdd;
    public List<EnemyDatabase> databaseList;

    [ContextMenu("Add Data to Database")]
    public void InsertDatabaseItem() {




        _connectionString = Application.dataPath;

        int pos = _connectionString.LastIndexOf("/");

        _connectionString = _connectionString.Substring(0, pos);

        _connectionString = _connectionString + "/" + databaseName;

        _connectionString = "URI=file:" + _connectionString;

        SqliteConnection con = new SqliteConnection(_connectionString);




        con.Open();

        DatabaseManager.Db = con;

        DatabaseManager.InsertDatabaseItem(dataToAdd);


        con.Close();




    }

    [ContextMenu("Get Database List")]
    public void GetDatabaseList() {
        _connectionString = Application.dataPath;

        int pos = _connectionString.LastIndexOf("/");

        _connectionString = _connectionString.Substring(0, pos);

        _connectionString = _connectionString + "/" + databaseName;

        _connectionString = "URI=file:" + _connectionString;

        SqliteConnection con = new SqliteConnection(_connectionString);




        con.Open();

        DatabaseManager.Db = con;


        databaseList = DatabaseManager.GetDatabaseList<EnemyDatabase>();



        con.Close();
    }
    [ContextMenu("Update Database List")]
    public void UpdateDatabaseList() {
        _connectionString = Application.dataPath;

        int pos = _connectionString.LastIndexOf("/");

        _connectionString = _connectionString.Substring(0, pos);

        _connectionString = _connectionString + "/" + databaseName;

        _connectionString = "URI=file:" + _connectionString;

        SqliteConnection con = new SqliteConnection(_connectionString);




        con.Open();

        DatabaseManager.Db = con;


        foreach (EnemyDatabase data in databaseList) {
            Debug.Log(data);
            DatabaseManager.UpdateDatabaseItem(data);
        }

        con.Close();
    }
}
