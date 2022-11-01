using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;

public class DatabaseItem {
    private SQLiteConnection _db;

    public SQLiteConnection Db {
        get {
            return _db;
        }

        set {
            _db = value;
        }
    }


    public virtual T GetDatabaseItem<T>(int index) where T : DatabaseItem, new() {
        if (_db == null) {
            return null;
        }
        return _db.Find<T>(index);
    }
    public virtual List<T> GetDatabaseItemList<T>() where T : DatabaseItem, new() {
        if (_db == null) {
            return null;
        }
        Type type = typeof(T);
        TableAttribute attribute = (TableAttribute)Attribute.GetCustomAttribute(type, typeof(TableAttribute));
        string table = attribute.Name;

        string sqlCommand = "SELECT * FROM " + table;

        SQLiteCommand command = _db.CreateCommand(sqlCommand);

        return command.ExecuteQuery<T>();
    }
    public virtual void UpdateDatabaseItem() {
        if (_db == null) {
            return;
        }
        _db.Update(this);
    }
    public virtual void InsertDatabaseItem() {
        if (_db == null) {
            return;
        }
        _db.Insert(this);
    }

    //public virtual void ResolveReferenzKeys() {


    //    foreach (PropertyInfo info in GetType().GetProperties()) {


    //        ForeigenReferenzAttribute foreigenReferenz = info.GetCustomAttribute<ForeigenReferenzAttribute>();

    //        if (foreigenReferenz != null) {
    //            int? key = (int?)GetType().GetProperty(foreigenReferenz.Property).GetValue(this);
    //            if (key == null) {
    //                continue;
    //            }

    //            TableMapping mapping = _db.GetMapping(info.PropertyType);
    //            info.SetValue(this, _db.Find(key, mapping));


    //        }
    //    }

    //}
    //public virtual void ResolveReferenzKeys(int keyValue, string referenzProperty) {


    //    PropertyInfo info = GetType().GetProperty(referenzProperty);

    //    TableMapping mapping = _db.GetMapping(info.PropertyType);
    //    info.SetValue(this, _db.Find(keyValue, mapping));



    //}

}
