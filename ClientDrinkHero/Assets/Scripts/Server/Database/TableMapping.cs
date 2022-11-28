

using System.Reflection;
#if CLIENT
using System.Collections.Generic;

using System;

#endif
public class TableMapping {

    private Dictionary<string, string> _columnMapping;
    private string _primaryKeyProperty;
    private string _primaryKeyColumn;
    private string _tableName;
    private Type _type;

    public TableMapping(Type type) {

        _type = type;
        _columnMapping = new Dictionary<string, string>();
        CreateMapping(type);

    }

    public Dictionary<string, string> ColumnsMapping {
        get {
            return _columnMapping;
        }
    }

    public string PrimaryKeyProperty {
        get {
            return _primaryKeyProperty;
        }
    }

    public Type Type {
        get {
            return _type;
        }
    }

    public string TableName {
        get {
            return _tableName;
        }


    }

    public string PrimaryKeyColumn {
        get {
            return _primaryKeyColumn;
        }

    }

    public void CreateMapping(Type type) {

        _tableName = type.GetCustomAttribute<TableAttribute>().Table;
        foreach (PropertyInfo info in type.GetProperties()) {
            PrimaryKeyAttribute key = info.GetCustomAttribute<PrimaryKeyAttribute>();
            ColumnAttribute column = info.GetCustomAttribute<ColumnAttribute>();
            if (column != null) {
                _columnMapping.Add(column.Name, info.Name);

            }
            if (key != null) {
                _primaryKeyProperty = info.Name;
                _primaryKeyColumn = column.Name;

            }
        }
    }

}
