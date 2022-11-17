using System.Collections.Generic;
using System.Reflection;

public static class ClientFunctions {


    public static string CreateFunctionCallString(string functionName, string parameter = null) {
        string functionCall = "OBJECT COMMAND " + functionName;
        if (parameter != null) {
            functionCall = functionCall + " PARAMETER " + parameter;
        }
        functionCall = functionCall + " END ";

        return functionCall;

    }
    public static void SendMessageToDatabase(string message) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.SendMessage));

        string callName = info.GetCustomAttribute<ServerFunctionAttribute>().Name;

        string function = CreateFunctionCallString(callName, message);

        GlobalGameInfos.Instance.Writer.Write(function);
    }
    public static void GetHeroDatabase() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetHeros));

        string callName = info.GetCustomAttribute<ServerFunctionAttribute>().Name;

        string function = CreateFunctionCallString(callName);

        GlobalGameInfos.Instance.Writer.Write(function);

    }
    public static void GetHeroDatabaseByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetHerosByKeyPair));

        string callName = info.GetCustomAttribute<ServerFunctionAttribute>().Name;

        string function = CreateFunctionCallString(callName, pair);

        GlobalGameInfos.Instance.Writer.Write(function);

    }

    public static void GetEnemyDatabase() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemy));

        string callName = info.GetCustomAttribute<ServerFunctionAttribute>().Name;

        string function = CreateFunctionCallString(callName);

        GlobalGameInfos.Instance.Writer.Write(function);

    }
    public static void GetCardToHeroByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetCardToHeroByKeyPair));

        string callName = info.GetCustomAttribute<ServerFunctionAttribute>().Name;

        string function = CreateFunctionCallString(callName, pair);

        GlobalGameInfos.Instance.Writer.Write(function);

    }
    public static void GetCardDatabaseByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetCardsByKeyPair));

        string callName = info.GetCustomAttribute<ServerFunctionAttribute>().Name;

        string function = CreateFunctionCallString(callName, pair);

        GlobalGameInfos.Instance.Writer.Write(function);

    }
    public static void GetEnemytoEnemySkillByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemyToEnemySkillByKeyPair));

        string callName = info.GetCustomAttribute<ServerFunctionAttribute>().Name;

        string function = CreateFunctionCallString(callName, pair);

        GlobalGameInfos.Instance.Writer.Write(function);

    }
    public static void GetEnemySkillByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemySkillByKeyPair));

        string callName = info.GetCustomAttribute<ServerFunctionAttribute>().Name;

        string function = CreateFunctionCallString(callName, pair);

        GlobalGameInfos.Instance.Writer.Write(function);

    }
}
