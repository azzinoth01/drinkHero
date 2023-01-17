#if SERVER
using MySql.Data.MySqlClient;
using System.Reflection;


#endif

#if CLIENT
using System;
using System.Collections.Generic;
using UnityEngine;
#endif




public static class DatabaseManager {

#if SERVER
    private static MySqlConnection _db;
#endif
    private static Dictionary<string, TableMapping> _tableMapping;
#if SERVER
    public static MySqlConnection Db {
        get {
            return _db;
        }

        set {
            _db = value;
        }
    }
#endif

    public static Dictionary<string, TableMapping> TableMapping {
        get {
            return _tableMapping;
        }
    }

#if SERVER
    private static T ReadRow<T>(MySqlDataReader reader, Dictionary<string, string> columnMapping) where T : DatabaseItem, new() {



        T item = new T();

        for (int i = 0; i < reader.FieldCount;) {

            if (columnMapping.TryGetValue(reader.GetName(i), out string propertyName)) {

                PropertyInfo property = item.GetType().GetProperty(propertyName);

                SetReadDataOnProperty<T>(property, item, reader, i);

            }
            i = i + 1;
        }
        return item;
    }
    private static void SetReadDataOnProperty<T>(PropertyInfo property, T item, MySqlDataReader reader, int readIndex) {


        //  Debug.Log(reader.GetFieldType(readIndex));

        if (reader.IsDBNull(readIndex)) {
            property.SetValue(item, null);
        }



        else if (reader.GetFieldType(readIndex) == typeof(int)) {

            property.SetValue(item, reader.GetInt32(readIndex));


        }
        else if (reader.GetFieldType(readIndex) == typeof(short)) {
            property.SetValue(item, reader.GetInt16(readIndex));
        }
        else if (reader.GetFieldType(readIndex) == typeof(long)) {
            property.SetValue(item, Convert.ChangeType(reader.GetInt64(readIndex), property.PropertyType));
        }
        else if (reader.GetFieldType(readIndex) == typeof(string)) {
            property.SetValue(item, reader.GetString(readIndex));
        }
        else if (reader.GetFieldType(readIndex) == typeof(bool)) {
            property.SetValue(item, reader.GetBoolean(readIndex));
        }
        else if (reader.GetFieldType(readIndex) == typeof(double)) {
            property.SetValue(item, reader.GetDouble(readIndex));
        }
        else if (reader.GetFieldType(readIndex) == typeof(byte)) {
            property.SetValue(item, reader.GetByte(readIndex));
        }
        else if (reader.GetFieldType(readIndex) == typeof(char)) {
            property.SetValue(item, reader.GetChar(readIndex));
        }
        else if (reader.GetFieldType(readIndex) == typeof(DateTime)) {
            property.SetValue(item, reader.GetDateTime(readIndex));
        }
        else if (reader.GetFieldType(readIndex) == typeof(decimal)) {
            property.SetValue(item, reader.GetDecimal(readIndex));
        }
        else if (reader.GetFieldType(readIndex) == typeof(float)) {
            property.SetValue(item, reader.GetFloat(readIndex));
        }
        else {
            property.SetValue(item, null);
        }
    }
#endif
    public static TableMapping GetTableMapping<T>() {

        return GetTableMapping(typeof(T));
    }
    public static TableMapping GetTableMapping(Type type) {
        if (_tableMapping == null) {
            _tableMapping = new Dictionary<string, TableMapping>();
        }

        if (_tableMapping.TryGetValue(type.Name, out TableMapping mapping)) {

        }
        else {
            mapping = new TableMapping(type);

            _tableMapping.Add(type.Name, mapping);
        }
        return mapping;
    }
#if SERVER
    public static T GetDatabaseItem<T>(int? index) where T : DatabaseItem, new() {
        if (_db == null || index == null) {
            return null;
        }
        if (_db.State != System.Data.ConnectionState.Open) {
            _db.Open();
        }

        TableMapping mapping = GetTableMapping<T>();

        string sqlCommand = "SELECT * FROM " + mapping.TableName + " WHERE " + mapping.PrimaryKeyColumn + " = " + index;

        MySqlCommand command = _db.CreateCommand();

        command.CommandText = sqlCommand;

        T item = new T();

        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read()) {


            item = ReadRow<T>(reader, mapping.ColumnsMapping);
        }
        reader.Close();
        return item;
    }
    public static T GetDatabaseItem<T>(List<string> foreigenKeys, List<int> keyValue) where T : DatabaseItem, new() {
        if (_db == null || foreigenKeys.Count == 0 || keyValue.Count == 0) {
            return null;
        }
        if (_db.State != System.Data.ConnectionState.Open) {
            _db.Open();
        }

        TableMapping mapping = GetTableMapping<T>();



        string sqlCommand = "SELECT * FROM " + mapping.TableName + " WHERE " + foreigenKeys[0] + "=" + keyValue[0] + " ";

        for (int i = 1; i < foreigenKeys.Count;) {

            sqlCommand = sqlCommand + "AND " + foreigenKeys[i] + "=" + keyValue[i] + " ";


            i = i + 1;
        }


        MySqlCommand command = _db.CreateCommand();

        command.CommandText = sqlCommand;

        //Console.Write("SQL Command: " + sqlCommand + "\r\n");


        T item = new T();

        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read()) {


            item = ReadRow<T>(reader, mapping.ColumnsMapping);

        }
        reader.Close();
        return item;
    }
    public static List<T> GetDatabaseList<T>() where T : DatabaseItem, new() {
        if (_db == null) {
            return null;
        }
        if (_db.State != System.Data.ConnectionState.Open) {
            _db.Open();
        }

        TableMapping mapping = GetTableMapping<T>();

        string sqlCommand = "SELECT * FROM " + mapping.TableName;

        MySqlCommand command = _db.CreateCommand();

        command.CommandText = sqlCommand;

        //Console.Write("SQL Command: " + sqlCommand + "\r\n");

        List<T> list = new List<T>();

        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read()) {


            list.Add(ReadRow<T>(reader, mapping.ColumnsMapping));
        }
        reader.Close();
        return list;
    }
    public static List<T> GetDatabaseList<T>(string foreigenKey, int keyValue) where T : DatabaseItem, new() {
        if (_db == null) {
            return null;
        }
        if (_db.State != System.Data.ConnectionState.Open) {
            _db.Open();
        }

        TableMapping mapping = GetTableMapping<T>();

        string sqlCommand = "SELECT * FROM " + mapping.TableName + " WHERE " + foreigenKey.Trim() + " = " + keyValue;

        MySqlCommand command = _db.CreateCommand();

        command.CommandText = sqlCommand;

        //Console.Write("SQL Command: " + sqlCommand + "\r\n");


        List<T> list = new List<T>();

        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read()) {


            list.Add(ReadRow<T>(reader, mapping.ColumnsMapping));
        }
        reader.Close();
        return list;
    }


    public static List<T> GetDatabaseList<T>(string viewName) where T : DatabaseItem, new() {
        if (_db == null) {
            return null;
        }
        if (_db.State != System.Data.ConnectionState.Open) {
            _db.Open();
        }

        TableMapping mapping = GetTableMapping<T>();

        string sqlCommand = "SELECT * FROM " + viewName;

        MySqlCommand command = _db.CreateCommand();
        command.CommandText = sqlCommand;

        List<T> list = new List<T>();

        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read()) {


            list.Add(ReadRow<T>(reader, mapping.ColumnsMapping));
        }
        reader.Close();
        return list;
    }


    public static void UpdateDatabaseItem<T>(T item) where T : DatabaseItem, new() {
        if (_db == null) {
            return;
        }
        if (_db.State != System.Data.ConnectionState.Open) {
            _db.Open();
        }


        TableMapping mapping = GetTableMapping<T>();

        string sqlCommand = "UPDATE " + mapping.TableName + " SET ";
        bool firstLoop = false;
        foreach (KeyValuePair<string, string> pair in mapping.ColumnsMapping) {
            if (pair.Key == mapping.PrimaryKeyColumn) {
                continue;
            }

            if (firstLoop == false) {
                firstLoop = true;
                sqlCommand = sqlCommand + " " + pair.Key + " = @" + pair.Key;
            }
            else {
                sqlCommand = sqlCommand + ", " + pair.Key + " = @" + pair.Key;
            }


        }
        sqlCommand = sqlCommand + " WHERE " + mapping.PrimaryKeyColumn + " = " + item.GetType().GetProperty(mapping.PrimaryKeyProperty).GetValue(item);

        MySqlCommand command = _db.CreateCommand();

        command.CommandText = sqlCommand;

        foreach (KeyValuePair<string, string> pair in mapping.ColumnsMapping) {
            if (pair.Key == mapping.PrimaryKeyColumn) {
                continue;
            }



            command.Parameters.AddWithValue("@" + pair.Key, item.GetType().GetProperty(pair.Value).GetValue(item));
        }

        command.ExecuteNonQuery();

    }
    public static void InsertDatabaseItem<T>(T item) where T : DatabaseItem, new() {
        if (_db == null) {
            return;
        }
        if (_db.State != System.Data.ConnectionState.Open) {
            _db.Open();
        }


        TableMapping mapping = GetTableMapping<T>();

        string sqlCommand = "INSERT INTO " + mapping.TableName + " ( ";
        bool firstLoop = false;
        foreach (string key in mapping.ColumnsMapping.Keys) {
            if (key == mapping.PrimaryKeyColumn) {
                continue;
            }

            if (firstLoop == false) {
                firstLoop = true;
                sqlCommand = sqlCommand + " " + key;
            }
            else {
                sqlCommand = sqlCommand + ", " + key;
            }

        }

        sqlCommand = sqlCommand + " ) VALUES (";
        firstLoop = false;

        foreach (string key in mapping.ColumnsMapping.Keys) {
            if (key == mapping.PrimaryKeyColumn) {
                continue;
            }

            if (firstLoop == false) {
                firstLoop = true;
                sqlCommand = sqlCommand + " @" + key;
            }
            else {
                sqlCommand = sqlCommand + ", @" + key;
            }

        }
        sqlCommand = sqlCommand + " ) ";


        MySqlCommand command = _db.CreateCommand();

        command.CommandText = sqlCommand;

        foreach (KeyValuePair<string, string> pair in mapping.ColumnsMapping) {
            if (pair.Key == mapping.PrimaryKeyColumn) {
                continue;
            }

            command.Parameters.AddWithValue("@" + pair.Key, item.GetType().GetProperty(pair.Value).GetValue(item));

        }


        command.ExecuteNonQuery();

    }
    public static int InsertDatabaseItemAndReturnKey<T>(T item) where T : DatabaseItem, new() {
        if (_db == null) {
            return -1;
        }
        if (_db.State != System.Data.ConnectionState.Open) {
            _db.Open();
        }


        TableMapping mapping = GetTableMapping<T>();

        string sqlCommand = "INSERT INTO " + mapping.TableName + " ( ";
        bool firstLoop = false;
        foreach (string key in mapping.ColumnsMapping.Keys) {
            if (key == mapping.PrimaryKeyColumn) {
                continue;
            }

            if (firstLoop == false) {
                firstLoop = true;
                sqlCommand = sqlCommand + " " + key;
            }
            else {
                sqlCommand = sqlCommand + ", " + key;
            }

        }

        sqlCommand = sqlCommand + " ) VALUES (";
        firstLoop = false;

        foreach (string key in mapping.ColumnsMapping.Keys) {
            if (key == mapping.PrimaryKeyColumn) {
                continue;
            }

            if (firstLoop == false) {
                firstLoop = true;
                sqlCommand = sqlCommand + " @" + key;
            }
            else {
                sqlCommand = sqlCommand + ", @" + key;
            }

        }
        sqlCommand = sqlCommand + " ) ";


        MySqlCommand command = _db.CreateCommand();

        MySqlTransaction transaction = _db.BeginTransaction();

        command.Connection = _db;
        command.Transaction = transaction;

        command.CommandText = sqlCommand;

        foreach (KeyValuePair<string, string> pair in mapping.ColumnsMapping) {
            if (pair.Key == mapping.PrimaryKeyColumn) {
                continue;
            }

            command.Parameters.AddWithValue("@" + pair.Key, item.GetType().GetProperty(pair.Value).GetValue(item));

        }

        Console.WriteLine(command.CommandText + " \r\n");

        command.ExecuteNonQuery();
        command.CommandText = "SELECT LAST_INSERT_ID()";

        Console.WriteLine(command.CommandText + " \r\n");

        MySqlDataReader reader = command.ExecuteReader();

        int id = -1;
        while (reader.Read()) {


            id = reader.GetInt32(0);



        }
        reader.Close();

        transaction.Commit();


        return id;
    }
#endif
}
