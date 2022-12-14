

using System.Reflection;
using System.Text.RegularExpressions;

#if CLIENT
using System.Collections.Generic;
using System;
using System.IO;
#endif

public static class TransmissionControl {
    private static Dictionary<string, MethodInfo> _callableServerMethods;

    private static Dictionary<string, MethodInfo> CallableServerMethods {
        get {
            if (_callableServerMethods == null) {
                _callableServerMethods = new Dictionary<string, MethodInfo>();
                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()) {

                    foreach (MethodInfo method in type.GetMethods()) {
                        ServerFunctionAttribute attribute = method.GetCustomAttribute<ServerFunctionAttribute>();

                        if (attribute != null) {
                            _callableServerMethods.Add(attribute.Name, method);
                        }

                    }

                }
            }


            return _callableServerMethods;
        }

    }

    public static bool CheckHeartBeat(string message, out string remainingMessage) {

        Match match = RegexPatterns.CheckKeepAlive.Match(message);

        if (match.Success) {
            string startString = message.Substring(0, match.Index);
            string endString = message.Substring(match.Index + match.Length);
            remainingMessage = startString + endString;
            return true;
        }
        remainingMessage = message;
        return false;
    }


    public static bool CheckIfDataIsEmpty(string message, out string remainingMessage) {

        Match match = RegexPatterns.CheckDataIsEmpty.Match(message);

        remainingMessage = message.Substring(match.Index + match.Length);

        return match.Success;
    }
    public static string GetMessageObject(string message, out string remainingMessage) {

        Match match = RegexPatterns.GetCompleteMessage.Match(message);

        remainingMessage = message.Substring(match.Index + match.Length);

        return match.Value;
    }

    public static bool? IsCommandMessage(string message) {

        Match match = RegexPatterns.ObjectCommand.Match(message);

        if (match.Value == "OBJECT COMMAND") {
            return true;
        }
        else if (match.Value == "OBJECT VALUE") {
            return false;
        }

        return null;
    }

    //public static void ParseParameterValues<T>(T item, PropertyInfo info, string parameterValue) {

    //    if (info != null) {
    //        Type type = info.PropertyType;

    //        if (type == typeof(int)) {
    //            info.SetValue(item, int.Parse(parameterValue));
    //        }
    //        else if (type == typeof(long)) {
    //            info.SetValue(item, long.Parse(parameterValue));
    //        }
    //        else if (type == typeof(float)) {
    //            info.SetValue(item, float.Parse(parameterValue));
    //        }
    //        else if (type == typeof(string)) {
    //            info.SetValue(item, parameterValue);
    //        }
    //        else if (type == typeof(bool)) {
    //            info.SetValue(item, bool.Parse(parameterValue));
    //        }
    //        else if (type == typeof(int?)) {
    //            if (parameterValue != "" && parameterValue != null) {
    //                info.SetValue(item, int.Parse(parameterValue));
    //            }
    //            else {
    //                info.SetValue(item, null);
    //            }

    //        }
    //        else if (type == typeof(long?)) {
    //            if (parameterValue != "" && parameterValue != null) {
    //                info.SetValue(item, long.Parse(parameterValue));
    //            }
    //            else {
    //                info.SetValue(item, null);
    //            }
    //        }
    //        else if (type == typeof(float?)) {
    //            if (parameterValue != "" && parameterValue != null) {
    //                info.SetValue(item, float.Parse(parameterValue));
    //            }
    //            else {
    //                info.SetValue(item, null);
    //            }
    //        }
    //        else if (type == typeof(bool?)) {
    //            if (parameterValue != "" && parameterValue != null) {
    //                info.SetValue(item, bool.Parse(parameterValue));
    //            }
    //            else {
    //                info.SetValue(item, null);
    //            }
    //        }
    //    }
    //}

    //public static List<T> GetObjectData<T>(string message) where T : new() {


    //    List<T> itemList = new List<T>();

    //    MatchCollection matches = RegexPatterns.GetValueMessage.Matches(message);
    //    TableMapping mapping = DatabaseManager.GetTableMapping<T>();
    //    foreach (Match match in matches) {
    //        T item = new T();
    //        string value = match.Value;

    //        string[] splitString = RegexPatterns.SplitValue.Split(value);

    //        foreach (string parameter in splitString) {
    //            string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
    //            string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

    //            if (parameterName == mapping.PrimaryKeyColumn) {
    //                //mapping.
    //            }

    //            if (mapping.ColumnsMapping.TryGetValue(parameterName, out string property)) {
    //                PropertyInfo info = item.GetType().GetProperty(property);
    //                ParseParameterValues(item, info, parameterValue);
    //            }
    //        }



    //        itemList.Add(item);
    //    }
    //    return itemList;
    //}



    public static string CommandMessage(StreamWriter stream, string message) {

        string callFunctionName = RegexPatterns.GetCallFunctionName.Match(message).Value.Trim();
        string parameterString = RegexPatterns.GetCallFunctionParameter.Match(message).Value.Trim();

        if (CallableServerMethods.TryGetValue(callFunctionName, out MethodInfo method)) {
            if (parameterString != null && parameterString != "") {
                object[] parameterArray = new object[2];
                parameterArray[0] = stream;
                parameterArray[1] = parameterString;

                return (string)method.Invoke(null, parameterArray);
            }
            else {
                object[] parameterArray = new object[1];
                parameterArray[0] = stream;
                return (string)method.Invoke(null, parameterArray);
            }
        }
        return null;
    }


}
