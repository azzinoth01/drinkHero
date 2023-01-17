#if CLIENT
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#endif

[Serializable, Table("ResponsMessageObject")]
public class ResponsMessageObject : DatabaseItem {

#if CLIENT

    [SerializeField] private string _message;

#endif

#if SERVER
    private string _message;
#endif

    [Column("Message")]
    public string Message {
        get {
            return _message;
        }

        set {
            _message = value;
        }
    }


#if CLIENT


    public static List<ResponsMessageObject> CreateObjectDataFromString(string message) {

        List<ResponsMessageObject> list = new List<ResponsMessageObject>();

        List<string[]> objectStrings = DatabaseItemCreationHelper.GetObjectStrings(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<ResponsMessageObject>();

        foreach (string[] obj in objectStrings) {
            ResponsMessageObject item = new ResponsMessageObject();
            foreach (string parameter in obj) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;
                if (mapping.ColumnsMapping.TryGetValue(parameterName, out string property)) {
                    PropertyInfo info = item.GetType().GetProperty(property);
                    DatabaseItemCreationHelper.ParseParameterValues(item, info, parameterValue);
                }
            }
            list.Add(item);
        }

        return list;
    }

#endif


    public ResponsMessageObject() {

    }
}
