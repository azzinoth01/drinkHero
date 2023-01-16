
#if SERVER
using System.Reflection;
#endif


using System.Text.RegularExpressions;

public static class TransmissionControl {

#if SERVER
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
# endif

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


}
