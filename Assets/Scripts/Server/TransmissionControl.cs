using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

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
                    HeroDatabase h = Activator.CreateInstance(type) as HeroDatabase;
                }
            }


            return _callableServerMethods;
        }

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

    public static List<T> GetObjectData<T>(string message) where T : new() {


        List<T> itemList = new List<T>();

        MatchCollection matches = RegexPatterns.GetValueMessage.Matches(message);
        TableMapping mapping = DatabaseManager.GetTableMapping<HeroDatabase>();
        foreach (Match match in matches) {
            T item = new T();
            string value = match.Value;

            string[] splitString = RegexPatterns.SplitValue.Split(value);

            foreach (string parameter in splitString) {
                string parameterName = RegexPatterns.PropertyName.Match(parameter).Value;
                string parameterValue = RegexPatterns.PropertyValue.Match(parameter).Value;

                if (mapping.ColumnsMapping.TryGetValue(parameterName, out string property)) {
                    PropertyInfo info = item.GetType().GetProperty(property);
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
                    }
                }
            }
            itemList.Add(item);
        }
        return itemList;
    }


    public static void CommandMessage(StreamWriter stream, string message) {

        string callFunctionName = RegexPatterns.GetCallFunctionName.Match(message).Value.Trim();
        string parameterString = RegexPatterns.GetCallFunctionParameter.Match(message).Value.Trim();

        if (CallableServerMethods.TryGetValue(callFunctionName, out MethodInfo method)) {
            if (parameterString != null && parameterString != "") {
                object[] parameterArray = new object[2];
                parameterArray[0] = stream;
                parameterArray[1] = parameterString;

                method.Invoke(null, parameterArray);
            }
            else {
                object[] parameterArray = new object[1];
                parameterArray[0] = stream;
                method.Invoke(null, parameterArray);
            }
        }
    }


    //public static bool EvaluateMessage(StreamWriter stream, string message, out string remainingMessage) {
    //    Regex getCompleteMessage = new Regex("OBJECT(?<=(?<!\\\\)OBJECT)[\\s\\S]*?(?=(?<!\\\\)(END))END");
    //    Regex getCommandMessage = new Regex("(?<=(?<!\\\\)COMMAND)[\\s\\S]*?(?=(?<!\\\\)(END))");
    //    Regex getCallFunctionName = new Regex("(?<=(?<!\\\\)COMMAND)[\\s\\S]*?(?=(?<!\\\\)(PARAMETER|END))");
    //    Regex getCallFunctionParameter = new Regex("(?<=(?<!\\\\)PARAMETER)[\\s\\S]*?(?=(?<!\\\\)(END))");

    //    Regex getValueMessage = new Regex("(?<=(?<!\\\\)VALUE)[\\s\\S]*?(?=(?<!\\\\)(END|NEXT))");

    //    Regex firstWord = new Regex("[\\S]*");

    //    Regex splitValue = new Regex("(?<!\\\\);");
    //    Regex propertyName = new Regex("[\\s\\S]*?(?=\")");
    //    Regex propertyValue = new Regex("(?<=\")[\\s\\S]*(?=\")");

    //    Regex objectCommand = new Regex("OBJECT (VALUE|COMMAND)");


    //    Match match = getCompleteMessage.Match(message);


    //    remainingMessage = message.Substring(match.Index + match.Length);

    //    string messageObject = match.Value;
    //    match = objectCommand.Match(messageObject);


    //    if (match.Value == "OBJECT VALUE") {
    //        List<HeroDatabase> heros = new List<HeroDatabase>();

    //        MatchCollection matches = getValueMessage.Matches(messageObject);
    //        TableMapping mapping = DatabaseManager.GetTableMapping<HeroDatabase>();
    //        foreach (Match obj in matches) {
    //            HeroDatabase hero = new HeroDatabase();


    //            string value = obj.Value;

    //            string[] splitString = splitValue.Split(value);

    //            foreach (string parameter in splitString) {
    //                string parameterName = propertyName.Match(parameter).Value;
    //                string parameterValue = propertyValue.Match(parameter).Value;


    //                if (mapping.ColumnsMapping.TryGetValue(parameterName, out string property)) {
    //                    PropertyInfo info = hero.GetType().GetProperty(property);
    //                    if (info != null) {
    //                        Type type = info.PropertyType;

    //                        if (type == typeof(int)) {
    //                            info.SetValue(hero, int.Parse(parameterValue));
    //                        }
    //                        else if (type == typeof(long)) {
    //                            info.SetValue(hero, long.Parse(parameterValue));
    //                        }
    //                        else if (type == typeof(float)) {
    //                            info.SetValue(hero, float.Parse(parameterValue));
    //                        }
    //                        else if (type == typeof(string)) {
    //                            info.SetValue(hero, parameterValue);
    //                        }
    //                        else if (type == typeof(bool)) {
    //                            info.SetValue(hero, bool.Parse(parameterValue));
    //                        }
    //                    }

    //                }
    //            }


    //            heros.Add(hero);
    //        }




    //    }
    //    else if (match.Value == "OBJECT COMMAND") {
    //        string callFunctionName = getCallFunctionName.Match(messageObject).Value.Trim();
    //        string parameterString = getCallFunctionParameter.Match(messageObject).Value.Trim();

    //        if (CallableServerMethods.TryGetValue(callFunctionName, out MethodInfo method)) {
    //            if (parameterString != null && parameterString != "") {
    //                object[] parameterArray = new object[2];
    //                parameterArray[0] = stream;
    //                parameterArray[1] = parameterString;

    //                method.Invoke(null, parameterArray);
    //            }
    //            else {
    //                object[] parameterArray = new object[1];
    //                parameterArray[0] = stream;
    //                method.Invoke(null, parameterArray);
    //            }
    //        }
    //    }

    //    return null;
    //}
}
