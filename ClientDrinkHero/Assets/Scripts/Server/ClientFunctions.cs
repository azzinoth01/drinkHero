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

    public static string GetEnemyDatabaseByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemyByKeyPair));

        return GetWriteString(info, pair);

    }

    public static string GetRandomEnemyDatabase() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetRandomEnemy));

        return GetWriteString(info);

    }
    public static string GetRandomNormalEnemyDatabase(int amount = 1) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetRandomNormalEnemy));

        return GetWriteString(info, amount.ToString());

    }
    public static string GetRandomBossEnemyDatabase() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetRandomBossEnemy));

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


    public static string GetCardToEffectByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetCardToEffectByKeyPair));
        return GetWriteString(info, pair);
    }
    public static string GetEffectByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEffectByKeyPair));
        return GetWriteString(info, pair);
    }

    public static string GetCardToEffect() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetCardToEffect));
        return GetWriteString(info);
    }
    public static string GetEffect() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEffect));
        return GetWriteString(info);
    }


    //public static string GetUser() {
    //    MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetUser));

    //    return GetWriteString(info);
    //}
    public static string GetUserByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetUserByKeyPair));
        return GetWriteString(info, pair);
    }
    public static string GetUserToHero() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetUserToHero));

        return GetWriteString(info);
    }
    public static string GetUserToHeroByKeyPair(string pair) {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetUserToHeroByKeyPair));
        return GetWriteString(info, pair);
    }

    public static string CreateNewUser() {
        MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.CreateNewUser));

        return GetWriteString(info);
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
