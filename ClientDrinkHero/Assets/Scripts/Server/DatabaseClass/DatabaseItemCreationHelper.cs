using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

public static class DatabaseItemCreationHelper {




    public static List<string[]> GetObjectStrings(string message) {
        MatchCollection matches = RegexPatterns.GetValueMessage.Matches(message);
        List<string[]> list = new List<string[]>();
        foreach (Match match in matches) {
            string[] splitString = RegexPatterns.SplitValue.Split(match.Value);
            list.Add(splitString);
        }
        return list;
    }

    public static void ParseParameterValues<T>(T item, PropertyInfo info, string parameterValue) {

        if (info != null) {
            Type type = info.PropertyType;

            if (type == typeof(int)) {
                info.SetValue(item, int.Parse(parameterValue));
            }
            else if (type == typeof(long)) {
                info.SetValue(item, long.Parse(parameterValue));
            }
            else if (type == typeof(float)) {
                info.SetValue(item, float.Parse(parameterValue));
            }
            else if (type == typeof(string)) {
                info.SetValue(item, parameterValue);
            }
            else if (type == typeof(bool)) {
                info.SetValue(item, bool.Parse(parameterValue));
            }
            else if (type == typeof(int?)) {
                if (parameterValue != "" && parameterValue != null) {
                    info.SetValue(item, int.Parse(parameterValue));
                }
                else {
                    info.SetValue(item, null);
                }

            }
            else if (type == typeof(long?)) {
                if (parameterValue != "" && parameterValue != null) {
                    info.SetValue(item, long.Parse(parameterValue));
                }
                else {
                    info.SetValue(item, null);
                }
            }
            else if (type == typeof(float?)) {
                if (parameterValue != "" && parameterValue != null) {
                    info.SetValue(item, float.Parse(parameterValue));
                }
                else {
                    info.SetValue(item, null);
                }
            }
            else if (type == typeof(bool?)) {
                if (parameterValue != "" && parameterValue != null) {
                    info.SetValue(item, bool.Parse(parameterValue));
                }
                else {
                    info.SetValue(item, null);
                }
            }
        }
    }

}
