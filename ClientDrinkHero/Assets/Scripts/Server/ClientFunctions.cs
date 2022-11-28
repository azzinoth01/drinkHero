using System.Reflection;

public static class ClientFunctions {



    public static string SendMessageToDatabase(string message) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.SendMessage));

        return GetWriteString(info, message);
    }
    public static string GetHeroDatabase() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetHeros));

        return GetWriteString(info);

    }
    public static string GetHeroDatabaseByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetHerosByKeyPair));

        return GetWriteString(info, pair);

    }

    public static string GetEnemyDatabase() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemy));

        return GetWriteString(info);

    }

    public static string GetRandomEnemyDatabase() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetRandomEnemy));

        return GetWriteString(info);

    }

    public static string GetCardToHeroByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetCardToHeroByKeyPair));

        return GetWriteString(info, pair);

    }
    public static string GetCardDatabaseByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetCardsByKeyPair));
        return GetWriteString(info, pair);

    }
    public static string GetEnemytoEnemySkillByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemyToEnemySkillByKeyPair));

        return GetWriteString(info, pair);

    }
    public static string GetEnemySkillByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemySkillByKeyPair));

        return GetWriteString(info, pair);


    }

    public static string SendHeartbeat() {
        return "KEEPALIVE ";

    }

    public static string CreateFunctionCallString(string functionName, string parameter = null) {
        string functionCall = "OBJECT COMMAND " + functionName;
        if (parameter != null) {
            functionCall = functionCall + " PARAMETER " + parameter;
        }
        functionCall = functionCall + " END ";

        return functionCall;

    }
    public static string GetWriteString(MethodInfo info, string parameter = null) {
        string callName = info.GetCustomAttribute<ServerFunctionAttribute>().Name;

        string function = CreateFunctionCallString(callName, parameter);
        return function;
    }





}
