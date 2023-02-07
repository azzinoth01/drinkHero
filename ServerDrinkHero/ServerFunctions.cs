
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
    private static string SendDataPairList<T>(StreamWriter stream, string pair) where T : DatabaseItem, new() {

        List<T> items;
        string[] splitPair = RegexPatterns.SplitValue.Split(pair);

        List<string> foreigenKey = new List<string>();
        List<int> keyValue = new List<int>();

        foreach (string split in splitPair) {
            (string, string) keyValuePair = ResolveKeyValuePair(split);
            string key = keyValuePair.Item1;
            int id = int.Parse(keyValuePair.Item2);
            foreigenKey.Add(key);
            keyValue.Add(id);
        }

        items = DatabaseManager.GetDatabaseList<T>(foreigenKey, keyValue);

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


                AddHeroToUser(user, (int)pull.RefGachaItem);

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


    private static void AddHeroToUser(UserDatabase user, int heroId) {

        HeroToUserDatabase heroToUser = new HeroToUserDatabase();
        heroToUser.RefHero = heroId;
        heroToUser.RefUser = user.Id;

        heroToUser = AddDataToDatabase<HeroToUserDatabase>(heroToUser);

        List<CardToHero> heroCardList = DatabaseManager.GetDatabaseList<CardToHero>("RefHero", (int)heroToUser.RefHero);

        foreach (CardToHero heroCard in heroCardList) {
            UserHeroToCardDatabase userHeroCard = new UserHeroToCardDatabase();
            userHeroCard.RefUserHero = heroToUser.Id;
            userHeroCard.RefCard = heroCard.RefCard;

            AddDataToDatabase<UserHeroToCardDatabase>(userHeroCard);
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

    [ServerFunction("GetUpgradeItemDatabaseByKey")]
    public static string GetUpgradeItemDatabaseByKey(ConnectedClient client, string pair) {

        return SendData<UpgradeItemDatabase>(client.StreamWriter, pair);

    }
    [ServerFunction("GetGachaInfo")]
    public static string GetGachaInfo(ConnectedClient client, string pair) {

        return SendData<GachaDatabase>(client.StreamWriter, pair);

    }


    [ServerFunction("GetUserToHero")]
    public static string GetUserToHero(ConnectedClient client) {

        return SendData<HeroToUserDatabase>(client.StreamWriter);


    }
    [ServerFunction("GetUserToHeroByKeyPair")]
    public static string GetUserToHeroByKeyPair(ConnectedClient client, string pair) {

        return SendData<HeroToUserDatabase>(client.StreamWriter, pair);
    }


    [ServerFunction("GetUserToHeroByLoggedInUser")]
    public static string GetUserToHeroByLoggedInUser(ConnectedClient client) {

        return GetUserToHeroByKeyPair(client, "RefUser\"" + client.User.Id + "\"");
    }



    [ServerFunction("GetUser")]
    public static string GetUser(ConnectedClient client) {
        return SendData<UserDatabase>(client.StreamWriter, "ID\"" + client.User.Id + "\"");
    }


    [ServerFunction("GetUserByKeyPair")]
    public static string GetUserByKeyPair(ConnectedClient client, string pair) {

        return SendData<UserDatabase>(client.StreamWriter, pair);


    }

    [ServerFunction("CreateNewUser")]
    public static string CreateNewUser(ConnectedClient client) {

        UserDatabase user = new UserDatabase();
        user = AddDataToDatabase<UserDatabase>(user);

        user.Money = 100;

        DatabaseManager.UpdateDatabaseItem<UserDatabase>(client.User);

        for (int i = 1; i < 5;) {
            AddHeroToUser(user, i);

            i = i + 1;
        }


        client.User = user;
        return SendData<UserDatabase>(client.StreamWriter, "ID\"" + user.Id + "\"");


    }

    [ServerFunction("LoginWithUser")]
    public static string LoginWithUser(ConnectedClient client, string userId) {
        int id = int.Parse(userId);
        UserDatabase user = DatabaseManager.GetDatabaseItem<UserDatabase>(id);

        if (user.Id == 0) {
            //User not found Create new User
            return CreateNewUser(client);
        }

        client.User = user;
        return SendData<UserDatabase>(client.StreamWriter, "ID\"" + user.Id + "\"");
    }

    [ServerFunction("RenameUser")]
    public static string RenameUser(ConnectedClient client, string name) {
        client.User.Name = name;

        DatabaseManager.UpdateDatabaseItem<UserDatabase>(client.User);
        return SendData<UserDatabase>(client.StreamWriter, "ID\"" + client.User.Id + "\"");
    }
    [ServerFunction("AddMoneyToUser")]
    public static string AddMoneyToUser(ConnectedClient client, string amount) {
        client.User.Money = client.User.Money + int.Parse(amount);

        DatabaseManager.UpdateDatabaseItem<UserDatabase>(client.User);
        return SendData<UserDatabase>(client.StreamWriter, "ID\"" + client.User.Id + "\"");
    }

    [ServerFunction("GetUserData")]
    public static string GetUserData(ConnectedClient client) {
        return SendData<UserDatabase>(client.StreamWriter, "ID\"" + client.User.Id + "\"");
    }


    [ServerFunction("PullGachaSingel")]
    public static string PullGachaSingel(ConnectedClient client, string pair) {

        ResponsMessageObject messageObject = new ResponsMessageObject();

        messageObject.Message = "FAILED";


        GachaDatabase gacha = GetItemFromDatabaseByKeyPair<GachaDatabase>(pair);

        client.User = DatabaseManager.GetDatabaseItem<UserDatabase>(client.User.Id);


        if (CheckHasMoney(gacha, client.User) == true) {

            GachaPullInstance pullInstance = new GachaPullInstance(client.User, gacha);

            List<int> pullList = pullInstance.Pull(1);

            ResolvePullId(client.User, pullList);

            if (gacha.CostType == "Gold") {
                client.User.Money = client.User.Money - gacha.CostSingelPull;
            }
            else if (gacha.CostType == "CrystalBottle") {
                client.User.CrystalBottles = client.User.CrystalBottles - gacha.CostSingelPull;
            }
            DatabaseManager.UpdateDatabaseItem<UserDatabase>(client.User);
            messageObject.Message = "SUCCESS";
        }
        return SendDataObject(client.StreamWriter, messageObject);
    }
    [ServerFunction("PullGachaMultiple")]
    public static string PullGachaMultiple(ConnectedClient client, string pair) {

        ResponsMessageObject messageObject = new ResponsMessageObject();

        messageObject.Message = "FAILED";

        GachaDatabase gacha = GetItemFromDatabaseByKeyPair<GachaDatabase>(pair);

        client.User = DatabaseManager.GetDatabaseItem<UserDatabase>(client.User.Id);


        if (CheckHasMoney(gacha, client.User) == true) {

            GachaPullInstance pullInstance = new GachaPullInstance(client.User, gacha);

            List<int> pullList = pullInstance.Pull(gacha.MultiPullAmount);

            ResolvePullId(client.User, pullList);

            if (gacha.CostType == "Gold") {
                client.User.Money = client.User.Money - gacha.CostMultiPull;
            }
            else if (gacha.CostType == "CrystalBottle") {
                client.User.CrystalBottles = client.User.CrystalBottles - gacha.CostMultiPull;
            }
            DatabaseManager.UpdateDatabaseItem<UserDatabase>(client.User);
            messageObject.Message = "SUCCESS";
        }
        return SendDataObject(client.StreamWriter, messageObject);
    }
    [ServerFunction("GetLastPullResult")]
    public static string GetLastPullResult(ConnectedClient client, string amount) {

        return SendDataLimitedOrderdData<PullHistoryDatabase>(client.StreamWriter, "RefUser\"" + client.User.Id + "\"", int.Parse(amount), "ID", "DESC");
    }

    [ServerFunction("GetCardListOfHero")]
    public static string GetCardListOfHero(ConnectedClient client, string pair) {


        (string, string) keyValue = ResolveKeyValuePair(pair);
        string key = keyValue.Item1;
        int id = int.Parse(keyValue.Item2);


        List<string> foreigenKey = new List<string>();
        List<int> foreigenKeyValue = new List<int>();

        foreigenKey.Add("RefUser");
        foreigenKey.Add("RefHero");

        foreigenKeyValue.Add(client.User.Id);
        foreigenKeyValue.Add(id);

        List<CardDatabase> cardList = DatabaseManager.GetDatabaseList<CardDatabase>("CardToHeroView", foreigenKey, foreigenKeyValue);

        string data = CreateTransmissionString<CardDatabase>(cardList);
        try {
            client.StreamWriter.Write(data);
        }
        catch {
            return "exeption";
        }
        return data;

    }
    [ServerFunction("UpgradeCard")]
    public static string UpgradeCard(ConnectedClient client, string pair) {
        ResponsMessageObject messageObject = new ResponsMessageObject();

        messageObject.Message = "FAILED";

        string[] splitPair = RegexPatterns.SplitValue.Split(pair);

        List<string> foreigenKey = new List<string>();
        List<int> keyValue = new List<int>();

        foreach (string split in splitPair) {
            (string, string) keyValuePair = ResolveKeyValuePair(split);
            string key = keyValuePair.Item1;
            int id = int.Parse(keyValuePair.Item2);
            foreigenKey.Add(key);
            keyValue.Add(id);
        }
        foreigenKey.Add("RefUser");
        keyValue.Add(client.User.Id);

        UserHeroToCardDatabase upgradeCard = DatabaseManager.GetDatabaseList<UserHeroToCardDatabase>("", foreigenKey, keyValue)[0];

        //check if card is upgradeable
        if (upgradeCard.Card.RefUpgradeTo == null) {
            return SendDataObject(client.StreamWriter, messageObject);
        }

        //check if upgrade item is needed
        if (upgradeCard.Card.RefUpgradeItem != null && upgradeCard.Card.UpgradeItemAmount != 0) {
            foreigenKey = new List<string>();
            keyValue = new List<int>();

            foreigenKey.Add("RefUser");
            foreigenKey.Add("RefItem");

            keyValue.Add(client.User.Id);
            keyValue.Add((int)upgradeCard.Card.RefUpgradeItem);

            //get possesed upgrade item
            UserToUpradeItemDatabase possesedUpgradeItem = DatabaseManager.GetDatabaseItem<UserToUpradeItemDatabase>(foreigenKey, keyValue);

            // check upgrade cost
            if (possesedUpgradeItem.Amount >= upgradeCard.Card.UpgradeItemAmount) {

                upgradeCard.RefCard = upgradeCard.Card.RefUpgradeTo;

                possesedUpgradeItem.Amount = possesedUpgradeItem.Amount - upgradeCard.Card.UpgradeItemAmount;

                DatabaseManager.UpdateDatabaseItem<UserHeroToCardDatabase>(upgradeCard);
                DatabaseManager.UpdateDatabaseItem<UserToUpradeItemDatabase>(possesedUpgradeItem);

                messageObject.Message = "SUCCESS";
            }

        }
        else {
            //upgrade item not needed
            upgradeCard.RefCard = upgradeCard.Card.RefUpgradeTo;
            DatabaseManager.UpdateDatabaseItem<UserHeroToCardDatabase>(upgradeCard);
            messageObject.Message = "SUCCESS";
        }





        return SendDataObject(client.StreamWriter, messageObject);
    }



    [ServerFunction("SendMessage")]
    public static string SendMessage(ConnectedClient client, string message) {

        return message;
    }
}
