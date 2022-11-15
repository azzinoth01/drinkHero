using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;

using System.Reflection;
using UnityEngine;

public static class DatabaseManager {
    private static SqliteConnection _db;
    private static Dictionary<string, TableMapping> _tableMapping;

    public static SqliteConnection Db {
        get {
            return _db;
        }

        set {
            _db = value;
        }
    }

    public static Dictionary<string, TableMapping> TableMapping {
        get {
            return _tableMapping;
        }
    }
    private static T ReadRow<T>(SqliteDataReader reader, Dictionary<string, string> columnMapping) where T : DatabaseItem, new() {

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
    private static void SetReadDataOnProperty<T>(PropertyInfo property, T item, SqliteDataReader reader, int readIndex) {


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

    public static TableMapping GetTableMapping<T>() {
        if (_tableMapping == null) {
            _tableMapping = new Dictionary<string, TableMapping>();
        }

        Type type = typeof(T);



        if (_tableMapping.TryGetValue(nameof(type), out TableMapping mapping)) {

        }
        else {
            mapping = new TableMapping(type);

            _tableMapping.Add(nameof(type), mapping);
        }
        return mapping;
    }

    public static T GetDatabaseItem<T>(long index) where T : DatabaseItem, new() {
        if (_db == null) {
            return null;
        }

        TableMapping mapping = GetTableMapping<T>();

        string sqlCommand = "SELECT * FROM " + mapping.TableName + " WHERE " + mapping.PrimaryKeyColumn + " = " + index;

        SqliteCommand command = _db.CreateCommand();

        command.CommandText = sqlCommand;

        T item = new T();

        SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read()) {


            item = ReadRow<T>(reader, mapping.ColumnsMapping);
        }

        return item;
    }
    public static List<T> GetDatabaseList<T>() where T : DatabaseItem, new() {
        if (_db == null) {
            return null;
        }

        TableMapping mapping = GetTableMapping<T>();

        string sqlCommand = "SELECT * FROM " + mapping.TableName;

        SqliteCommand command = _db.CreateCommand();

        command.CommandText = sqlCommand;

        List<T> list = new List<T>();

        SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read()) {


            list.Add(ReadRow<T>(reader, mapping.ColumnsMapping));
        }

        return list;
    }
    public static List<T> GetDatabaseList<T>(string foreigenKey, long keyValue) where T : DatabaseItem, new() {
        if (_db == null) {
            return null;
        }

        TableMapping mapping = GetTableMapping<T>();

        string sqlCommand = "SELECT * FROM " + mapping.TableName + " WHERE " + foreigenKey + " = " + keyValue;

        SqliteCommand command = _db.CreateCommand();

        command.CommandText = sqlCommand;

        List<T> list = new List<T>();

        SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read()) {


            list.Add(ReadRow<T>(reader, mapping.ColumnsMapping));
        }

        return list;
    }


    public static void UpdateDatabaseItem<T>(T item) where T : DatabaseItem, new() {
        if (_db == null) {
            return;
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

        SqliteCommand command = _db.CreateCommand();

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

        SqliteCommand command = _db.CreateCommand();

        command.CommandText = sqlCommand;

        foreach (KeyValuePair<string, string> pair in mapping.ColumnsMapping) {
            if (pair.Key == mapping.PrimaryKeyColumn) {
                continue;
            }

            command.Parameters.AddWithValue("@" + pair.Key, item.GetType().GetProperty(pair.Value).GetValue(item));

        }


        command.ExecuteNonQuery();

    }

}
