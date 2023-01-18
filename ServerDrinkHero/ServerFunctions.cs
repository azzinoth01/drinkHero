
public static class ServerFunctions {



    public static string CreateTransmissionString<T>(List<T> itemList) {


        TableMapping mapping = DatabaseManager.GetTableMapping<T>();
        string data = "";

        foreach (T item in itemList) {
            if (data != "") {
                data = data + " NEXT ";
            }
            data = data + CreateTransmissionStringOfItem<T>(item, mapping);
        }
        data = data + " END ";
        return data;
    }
    public static string CreateTransmissionStringOfItem<T>(T item, TableMapping? mapping = null) {
        if (mapping == null) {
            mapping = DatabaseManager.GetTableMapping<T>();
        }

        string data = "";

        data = data + "OBJECT VALUE ";

        data = data + "classname";

        data = data + "\"" + mapping.TableName + "\"";
        foreach (KeyValuePair<string, string> pair in mapping.ColumnsMapping) {
            data = data + ";" + pair.Key;
            data = data + "\"" + item.GetType().GetProperty(pair.Value)?.GetValue(item)?.ToString() + "\"";
        }
        return data;
    }

    public static (string, string) ResolveKeyValuePair(string pair) {

        string key = RegexPatterns.PropertyName.Match(pair).Value;
        string value = RegexPatterns.PropertyValue.Match(pair).Value;

        return (key, value);
    }


    private static string SendData<T>(StreamWriter stream, string pair = "") where T : DatabaseItem, new() {

        List<T> items;
        if (pair == "") {
            items = DatabaseManager.GetDatabaseList<T>();
        }
        else {
            (string, string) keyValue = ResolveKeyValuePair(pair);
            string key = keyValue.Item1;
            int id = int.Parse(keyValue.Item2);
            items = DatabaseManager.GetDatabaseList<T>(key, id);
        }

        string data = CreateTransmissionString<T>(items);
        try {
            stream.Write(data);
        }
        catch {
            return "exeption";
        }
        return data;

    }
    private static string SendDataObject<T>(StreamWriter stream, T item) where T : DatabaseItem {



        string data = CreateTransmissionStringOfItem<T>(item);
        data = data + " END ";

        try {
            stream.Write(data);
        }
        catch {
            return "exeption";
        }
        return data;

    }

    private static string SendDataOfRandom<T>(StreamWriter stream) where T : DatabaseItem, new() {

        List<T> items = DatabaseManager.GetDatabaseList<T>();
        Random rand = new Random((int)DateTime.Now.Ticks);



        int index = RandomGenerator.Instance.NextRandom(0, items.Count - 1);

        T item = items[index];

        string data = CreateTransmissionStringOfItem<T>(item);
        data = data + " END ";

        try {
            stream.Write(data);
        }
        catch {
            return "exeption";
        }
        return data;

    }
    private static string SendDataOfRandom<T>(StreamWriter stream, string viewName, string amount = "1") where T : DatabaseItem, new() {

        List<T> items = DatabaseManager.GetDatabaseList<T>(viewName);

        int count = int.Parse(amount);


        List<T> transmissionItem = new List<T>();
        for (int i = 0; i < count;) {

            int index = RandomGenerator.Instance.NextRandom(0, items.Count - 1);
            transmissionItem.Add(items[index]);

            i = i + 1;
        }

        string data = CreateTransmissionString<T>(transmissionItem);


        try {
            stream.Write(data);
        }
        catch {
            return "exeption";
        }
        return data;

    }

    private static string SendDataLimitedOrderdData<T>(StreamWriter stream, string pair, int amount, string OrderKey, string OrderType) where T : DatabaseItem, new() {



        (string, string) keyValue = ResolveKeyValuePair(pair);
        string key = keyValue.Item1;
        int id = int.Parse(keyValue.Item2);



        List<T> items = DatabaseManager.GetDatabaseList<T>(key, id, amount, OrderKey, OrderType);

        string data = CreateTransmissionString<T>(items);
        try {
            stream.Write(data);
        }
        catch {
            return "exeption";
        }
        return data;

    }


    private static T AddDataToDatabase<T>(T item) where T : DatabaseItem, new() {

        int? id = DatabaseManager.InsertDatabaseItemAndReturnKey<T>(item);

        return DatabaseManager.GetDatabaseItem<T>(id);



    }

    private static T GetItemFromDatabaseByKeyPair<T>(string pair) where T : DatabaseItem, new() {


        (string, string) keyValue = ResolveKeyValuePair(pair);
        string key = keyValue.Item1;
        int id = int.Parse(keyValue.Item2);
        List<T> items = DatabaseManager.GetDatabaseList<T>(key, id);



        return items[0];

    }


    public static bool CheckHasMoney(GachaDatabase gacha, UserDatabase user) {

        if (gacha.CostType == "Gold") {
            if (gacha.CostSingelPull <= user.Money) {
                return true;
            }
        }
        else if (gacha.CostType == "CrystalBottle") {
            if (gacha.CostSingelPull <= user.CrystalBottles) {
                return true;
            }
        }

        return false;
    }


    public static void ResolvePullId(UserDatabase user, List<int> pullList) {


        foreach (int pullID in pullList) {




            GachaCategorieToGachaItemDatabase pull = DatabaseManager.GetDatabaseItem<GachaCategorieToGachaItemDatabase>(pullID);


            //Console.Write("Pulled Categorie: " + pull.RefGachaCategorie + " Pulled ID: " + pullID + " Pulled Type: " + pull.RefGachaCategorie + " Pulled Item ID: " + pull.RefGachaItem + "\r\n");
            if (pull.GachaItemType == "Hero") {

                HeroToUserDatabase heroToUser = new HeroToUserDatabase();

                heroToUser.RefHero = pull.RefGachaItem;
                heroToUser.RefUser = user.Id;

                AddDataToDatabase<HeroToUserDatabase>(heroToUser);

                PullHistoryDatabase pullHistory = new PullHistoryDatabase();
                pullHistory.RefUser = user.Id;
                pullHistory.Type = pull.GachaItemType;
                pullHistory.TypeID = pull.RefGachaItem;

                AddDataToDatabase<PullHistoryDatabase>(pullHistory);

            }
            else if (pull.GachaItemType == "Item") {
                UpgradeItemDatabase item = DatabaseManager.GetDatabaseItem<UpgradeItemDatabase>(pull.RefGachaItem);

                List<string> foreigenkeys = new List<string>();
                List<int> keyValues = new List<int>();

                foreigenkeys.Add("RefUser");
                foreigenkeys.Add("RefItem");

                keyValues.Add(user.Id);
                keyValues.Add((int)pull.RefGachaItem);

                UserToUpradeItemDatabase userToUpradeItem = DatabaseManager.GetDatabaseItem<UserToUpradeItemDatabase>(foreigenkeys, keyValues);

                if (userToUpradeItem.Id == 0) {
                    userToUpradeItem = new UserToUpradeItemDatabase();
                    userToUpradeItem.Amount = 1;
                    userToUpradeItem.RefItem = pull.RefGachaItem;
                    userToUpradeItem.RefUser = user.Id;
                    AddDataToDatabase<UserToUpradeItemDatabase>(userToUpradeItem);
                }
                else {
                    userToUpradeItem.Amount = userToUpradeItem.Amount + 1;
                    DatabaseManager.UpdateDatabaseItem<UserToUpradeItemDatabase>(userToUpradeItem);
                }

                PullHistoryDatabase pullHistory = new PullHistoryDatabase();
                pullHistory.RefUser = user.Id;
                pullHistory.Type = pull.GachaItemType;
                pullHistory.TypeID = pull.RefGachaItem;

                AddDataToDatabase<PullHistoryDatabase>(pullHistory);
            }
        }

    }






    [ServerFunction("GetHeros")]
    public static string GetHeros(ConnectedClient client) {
        return SendData<HeroDatabase>(client.StreamWriter);

    }

    [ServerFunction("GetHerosByKeyPair")]
    public static string GetHerosByKeyPair(ConnectedClient client, string pair) {

        return SendData<HeroDatabase>(client.StreamWriter, pair);

    }
    [ServerFunction("GetCardToHero")]
    public static string GetCardToHero(ConnectedClient client) {

        return SendData<CardToHero>(client.StreamWriter);
    }
    [ServerFunction("GetCardToHeroByKeyPair")]
    public static string GetCardToHeroByKeyPair(ConnectedClient client, string pair) {

        return SendData<CardToHero>(client.StreamWriter, pair);


    }
    [ServerFunction("GetCards")]
    public static string GetCards(ConnectedClient client) {

        return SendData<CardDatabase>(client.StreamWriter);

    }
    [ServerFunction("GetCardsByKeyPair")]
    public static string GetCardsByKeyPair(ConnectedClient client, string pair) {

        return SendData<CardDatabase>(client.StreamWriter, pair);


    }
    [ServerFunction("GetEnemy")]
    public static string GetEnemy(ConnectedClient client) {

        return SendData<EnemyDatabase>(client.StreamWriter);


    }
    [ServerFunction("GetRandomEnemy")]
    public static string GetRandomEnemy(ConnectedClient client) {

        return SendDataOfRandom<EnemyDatabase>(client.StreamWriter);

    }
    [ServerFunction("GetRandomNormalEnemy")]
    public static string GetRandomNormalEnemy(ConnectedClient client, string amount) {


        string viewName = "NormalEnemyView";

        return SendDataOfRandom<EnemyDatabase>(client.StreamWriter, viewName, amount);

    }
    [ServerFunction("GetRandomBossEnemy")]
    public static string GetRandomBossEnemy(ConnectedClient client) {

        string viewName = "BossEnemyView";

        return SendDataOfRandom<EnemyDatabase>(client.StreamWriter, viewName);


    }
    [ServerFunction("GetEnemyByKeyPair")]
    public static string GetEnemyByKeyPair(ConnectedClient client, string pair) {

        return SendData<EnemyDatabase>(client.StreamWriter, pair);


    }
    [ServerFunction("GetEnemyToEnemySkill")]
    public static string GetEnemyToEnemySkill(ConnectedClient client) {

        return SendData<EnemyToEnemySkill>(client.StreamWriter);


    }
    [ServerFunction("GetEnemyToEnemySkillByKeyPair")]
    public static string GetEnemyToEnemySkillByKeyPair(ConnectedClient client, string pair) {

        return SendData<EnemyToEnemySkill>(client.StreamWriter, pair);


    }
    [ServerFunction("GetEnemySkill")]
    public static string GetEnemySkill(ConnectedClient client) {

        return SendData<EnemySkillDatabase>(client.StreamWriter);

    }
    [ServerFunction("GetEnemySkillByKeyPair")]
    public static string GetEnemySkillByKeyPair(ConnectedClient client, string pair) {

        return SendData<EnemySkillDatabase>(client.StreamWriter, pair);


    }
    [ServerFunction("GetCardToEffect")]
    public static string GetCardToEffect(ConnectedClient client) {

        return SendData<CardToEffect>(client.StreamWriter);


    }
    [ServerFunction("GetCardToEffectByKeyPair")]
    public static string GetCardToEffectByKeyPair(ConnectedClient client, string pair) {

        return SendData<CardToEffect>(client.StreamWriter, pair);


    }
    [ServerFunction("GetEffect")]
    public static string GetEffect(ConnectedClient client) {

        return SendData<Effect>(client.StreamWriter);


    }
    [ServerFunction("GetEffectByKeyPair")]
    public static string GetEffectByKeyPair(ConnectedClient client, string pair) {

        return SendData<Effect>(client.StreamWriter, pair);


    }


    [ServerFunction("GetUserToHero")]
    public static string GetUserToHero(ConnectedClient client) {

        return SendData<HeroToUserDatabase>(client.StreamWriter);


    }
    [ServerFunction("GetUserToHeroByKeyPair")]
    public static string GetUserToHeroByKeyPair(ConnectedClient client, string pair) {

        return SendData<HeroToUserDatabase>(client.StreamWriter, pair);


    }
    [ServerFunction("GetUser")]
    public static string GetUser(ConnectedClient client) {

        return SendData<UserDatabase>(client.StreamWriter);

    }


    [ServerFunction("GetUserByKeyPair")]
    public static string GetUserByKeyPair(ConnectedClient client, string pair) {

        return SendData<UserDatabase>(client.StreamWriter, pair);


    }

    [ServerFunction("CreateNewUser")]
    public static string CreateNewUser(ConnectedClient client) {

        UserDatabase user = new UserDatabase();
        user = AddDataToDatabase<UserDatabase>(user);

        for (int i = 1; i < 5;) {
            HeroToUserDatabase heroToUser = new HeroToUserDatabase();

            heroToUser.RefHero = i;
            heroToUser.RefUser = user.Id;

            AddDataToDatabase<HeroToUserDatabase>(heroToUser);

            i = i + 1;
        }


        client.User = user;
        return SendData<UserDatabase>(client.StreamWriter, "ID\"" + user.Id + "\"");


    }

    [ServerFunction("LoginWithUser")]
    public static string LoginWithUser(ConnectedClient client, string userId) {
        int id = int.Parse(userId);
        UserDatabase user = DatabaseManager.GetDatabaseItem<UserDatabase>(id);

        client.User = user;
        return SendData<UserDatabase>(client.StreamWriter, "ID\"" + user.Id + "\"");
    }

    [ServerFunction("RenameUser")]
    public static string RenameUser(ConnectedClient client, string name) {
        client.User.Name = name;

        DatabaseManager.UpdateDatabaseItem<UserDatabase>(client.User);
        return SendData<UserDatabase>(client.StreamWriter, "ID\"" + client.User.Id + "\"");
    }


    [ServerFunction("PullGachaSingel")]
    public static string PullGachaSingel(ConnectedClient client, string pair) {

        ResponsMessageObject messageObject = new ResponsMessageObject();

        messageObject.Message = "FAILED";


        GachaDatabase gacha = GetItemFromDatabaseByKeyPair<GachaDatabase>(pair);

        UserDatabase user = client.User;

        if (CheckHasMoney(gacha, user) == true) {

            GachaPullInstance pullInstance = new GachaPullInstance(user, gacha);

            List<int> pullList = pullInstance.Pull(1);

            ResolvePullId(user, pullList);

            if (gacha.CostType == "Gold") {
                user.Money = user.Money - gacha.CostSingelPull;
            }
            else if (gacha.CostType == "CrystalBottle") {
                user.CrystalBottles = user.CrystalBottles - gacha.CostSingelPull;
            }
            DatabaseManager.UpdateDatabaseItem<UserDatabase>(user);
            messageObject.Message = "SUCCESS";
        }
        return SendDataObject(client.StreamWriter, messageObject);
    }
    [ServerFunction("PullGachaMultiple")]
    public static string PullGachaMultiple(ConnectedClient client, string pair) {

        ResponsMessageObject messageObject = new ResponsMessageObject();

        messageObject.Message = "FAILED";

        GachaDatabase gacha = GetItemFromDatabaseByKeyPair<GachaDatabase>(pair);

        UserDatabase user = client.User;

        if (CheckHasMoney(gacha, user) == true) {

            GachaPullInstance pullInstance = new GachaPullInstance(user, gacha);

            List<int> pullList = pullInstance.Pull(gacha.MultiPullAmount);

            ResolvePullId(user, pullList);

            if (gacha.CostType == "Gold") {
                user.Money = user.Money - gacha.CostMultiPull;
            }
            else if (gacha.CostType == "CrystalBottle") {
                user.CrystalBottles = user.CrystalBottles - gacha.CostMultiPull;
            }
            DatabaseManager.UpdateDatabaseItem<UserDatabase>(user);
            messageObject.Message = "SUCCESS";
        }
        return SendDataObject(client.StreamWriter, messageObject);
    }
    [ServerFunction("GetLastPullResult")]
    public static string GetLastPullResult(ConnectedClient client, string amount) {

        return SendDataLimitedOrderdData<PullHistoryDatabase>(client.StreamWriter, "RefUser\"" + client.User.Id + "\"", int.Parse(amount), "ID", "DESC");
    }


    [ServerFunction("SendMessage")]
    public static string SendMessage(ConnectedClient client, string message) {

        return message;
    }
}
