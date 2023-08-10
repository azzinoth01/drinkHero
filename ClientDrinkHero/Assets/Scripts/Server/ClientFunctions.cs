public static class ClientFunctions {



    //returns User
    public static string UnlockHeroByID(string Id) {
        string callName = "UnlockHeroByID";
        return GetWriteString(callName, id);
    }


    // returns UserToUpradeItemDatabase 
    public static string GetUserToUpgradeItemList() {
        string callName = "GetUserToUpgradeItemByLoggedinUser";
        return GetWriteString(callName);
    }



    //returns ResponsMessageObject FAILURE / SUCCESS
    public static string GetGachaInfo() {
        string callName = "GetGachaInfo";
        return GetWriteString(callName, "ID\"" + 1 + "\"");
    }

    //returns UpgradeItemDatabase
    public static string GetUpgradeItemDatabaseByKey(string pair) {
        string callName = "GetUpgradeItemDatabaseByKey";
        return GetWriteString(callName, pair);
    }


    //returns User
    public static string AddMoneyToUser(int amount) {
        string callName = "AddMoneyToUser";
        return GetWriteString(callName, amount.ToString());
    }
    //returns User
    public static string GetUserData() {
        string callName = "GetUserData";
        return GetWriteString(callName);
    }

    //returns User
    public static string RenameUser(string pair) {
        string callName = "RenameUser";
        return GetWriteString(callName, pair);
    }

    //returns ResponsMessageObject FAILURE / SUCCESS
    public static string GachaMultiPull() {
        string callName = "PullGachaMultiple";
        return GetWriteString(callName, "ID\"" + 1 + "\"");
    }
    //returns ResponsMessageObject FAILURE / SUCCESS
    public static string PullGachaSingel() {
        string callName = "PullGachaSingel";
        return GetWriteString(callName, "ID\"" + 1 + "\"");
    }
    //returns List PullHistory
    public static string GetLastPullResult(int amount = 1) {
        string callName = "GetLastPullResult";
        return GetWriteString(callName, amount.ToString());
    }

    // returns List Card
    public static string GetCardListOfHero(string pair) {
        string callName = "GetCardListOfHero";
        return GetWriteString(callName, pair);
    }

    //returns ResponsMessageObject FAILURE / SUCCESS
    //pair = RefHero"1";RefCard"1" 
    // "RefHero\"1\";RefCard\"1\"" 
    public static string UpgradeCard(string pair) {
        string callName = "UpgradeCard";
        return GetWriteString(callName, pair);
    }


    public static string SendMessageToDatabase(string message) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.SendMessage));

        string callName = "SendMessage";
        return GetWriteString(callName, message);
    }
    public static string GetHeroDatabase() {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetHeros));

        string callName = "GetHeros";
        return GetWriteString(callName);
    }
    public static string GetHeroDatabaseByKeyPair(string pair) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetHerosByKeyPair));

        string callName = "GetHerosByKeyPair";
        return GetWriteString(callName, pair);
    }

    public static string GetEnemyDatabase() {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemy));

        string callName = "GetEnemy";
        return GetWriteString(callName);

    }

    public static string GetEnemyDatabaseByKeyPair(string pair) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemyByKeyPair));

        string callName = "GetEnemyByKeyPair";
        return GetWriteString(callName, pair);

    }

    public static string GetRandomEnemyDatabase() {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetRandomEnemy));

        string callName = "GetRandomEnemy";
        return GetWriteString(callName);

    }
    public static string GetRandomNormalEnemyDatabase(int amount = 1) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetRandomNormalEnemy));


        string callName = "GetRandomNormalEnemy";
        return GetWriteString(callName, amount.ToString());
    }
    public static string GetRandomBossEnemyDatabase() {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetRandomBossEnemy));

        string callName = "GetRandomBossEnemy";
        return GetWriteString(callName);

    }

    public static string GetCardToHeroByKeyPair(string pair) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetCardToHeroByKeyPair));

        string callName = "GetCardToHeroByKeyPair";
        return GetWriteString(callName, pair);

    }
    public static string GetCardDatabaseByKeyPair(string pair) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetCardsByKeyPair));
        string callName = "GetCardsByKeyPair";
        return GetWriteString(callName, pair);

    }
    public static string GetEnemytoEnemySkillByKeyPair(string pair) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemyToEnemySkillByKeyPair));
        string callName = "GetEnemyToEnemySkillByKeyPair";
        return GetWriteString(callName, pair);
    }
    public static string GetEnemySkillByKeyPair(string pair) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEnemySkillByKeyPair));
        string callName = "GetEnemySkillByKeyPair";
        return GetWriteString(callName, pair);
    }


    public static string GetCardToEffectByKeyPair(string pair) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetCardToEffectByKeyPair));
        string callName = "GetCardToEffectByKeyPair";
        return GetWriteString(callName, pair);
    }
    public static string GetEffectByKeyPair(string pair) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEffectByKeyPair));
        string callName = "GetEffectByKeyPair";
        return GetWriteString(callName, pair);
    }

    public static string GetCardToEffect() {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetCardToEffect));
        string callName = "GetCardToEffect";
        return GetWriteString(callName);
    }
    public static string GetEffect() {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetEffect));
        string callName = "GetEffect";
        return GetWriteString(callName);
    }

    public static string GetUserByKeyPair(string pair) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetUserByKeyPair));
        string callName = "GetUserByKeyPair";
        return GetWriteString(callName, pair);
    }
    public static string GetUserToHero() {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetUserToHero));

        string callName = "GetUserToHero";
        return GetWriteString(callName);
    }

    public static string GetUserToHeroByLoggedInUser() {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetUserToHero));

        string callName = "GetUserToHeroByLoggedInUser";
        return GetWriteString(callName);
    }

    public static string GetUserToHeroByKeyPair(string pair) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.GetUserToHeroByKeyPair));
        string callName = "GetUserToHeroByKeyPair";
        return GetWriteString(callName, pair);
    }


    public static string CreateNewUser() {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.CreateNewUser));

        string callName = "CreateNewUser";
        return GetWriteString(callName);
    }
    public static string LoginWithUser(string userId) {
        //MethodInfo info = typeof(ServerFunctions).GetMethod(nameof(ServerFunctions.CreateNewUser));

        string callName = "LoginWithUser";
        return GetWriteString(callName, userId);
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
    public static string GetWriteString(string callName, string parameter = null) {

        string function = CreateFunctionCallString(callName, parameter);
        return function;
    }





}
