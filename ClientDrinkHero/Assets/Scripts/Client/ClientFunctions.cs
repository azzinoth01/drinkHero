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

        WriteToServer(info, message);
    }
    public static void GetHeroDatabase() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetHeros));

        WriteToServer(info);

    }
    public static void GetHeroDatabaseByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetHerosByKeyPair));

        WriteToServer(info, pair);

    }

    public static void GetEnemyDatabase() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemy));

        WriteToServer(info);

    }

    public static void GetRandomEnemyDatabase() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetRandomEnemy));

        WriteToServer(info);

    }

    public static void GetCardToHeroByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetCardToHeroByKeyPair));

        WriteToServer(info, pair);

    }
    public static void GetCardDatabaseByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetCardsByKeyPair));
        WriteToServer(info, pair);

    }
    public static void GetEnemytoEnemySkillByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemyToEnemySkillByKeyPair));

        WriteToServer(info, pair);

    }
    public static void GetEnemySkillByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemySkillByKeyPair));

        WriteToServer(info, pair);


    }
    public static void WriteToServer(MethodInfo info, string parameter = null) {
        string callName = info.GetCustomAttribute<ServerFunctionAttribute>().Name;

        string function = CreateFunctionCallString(callName, parameter);
        GlobalGameInfos.serverRequestQueue.Enqueue(function);
        //try {
        //    GlobalGameInfos.Instance.Writer.Write(function);
        //}
        //catch {
        //    GlobalGameInfos.Instance.StopServerConnection();
        //}
    }

    public static void SendHeartbeat() {
        GlobalGameInfos.serverRequestQueue.Enqueue("KEEPALIVE ");
        //try {
        //    GlobalGameInfos.Instance.Writer.Write("KEEPALIVE ");
        //}
        //catch {
        //    GlobalGameInfos.Instance.StopServerConnection();
        //}
    }
}
