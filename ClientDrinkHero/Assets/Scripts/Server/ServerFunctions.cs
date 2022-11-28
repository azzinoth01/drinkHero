

#if CLIENT
using System.IO;
#endif
public static class ServerFunctions {


#if SERVER
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
    public static string CreateTransmissionStringOfItem<T>(T item, TableMapping mapping = null) {
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

    private static string SendDataOfRandom<T>(StreamWriter stream) where T : DatabaseItem, new() {

        List<T> items = DatabaseManager.GetDatabaseList<T>();
        Random rand = new Random((int)DateTime.Now.Ticks);

        int index = rand.Next(0, items.Count - 1);
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
#endif



    [ServerFunction("GetHeros")]
    public static string GetHeros(StreamWriter stream) {
#if SERVER
        return SendData<HeroDatabase>(stream);
#else
        return null;
#endif

    }

    [ServerFunction("GetHerosByKeyPair")]
    public static string GetHerosByKeyPair(StreamWriter stream, string pair) {
#if SERVER
        return SendData<HeroDatabase>(stream, pair);
#else
        return null;
#endif

    }
    [ServerFunction("GetCardToHero")]
    public static string GetCardToHero(StreamWriter stream) {
#if SERVER
        return SendData<CardToHero>(stream);
#else
        return null;
#endif


    }
    [ServerFunction("GetCardToHeroByKeyPair")]
    public static string GetCardToHeroByKeyPair(StreamWriter stream, string pair) {
#if SERVER
        return SendData<CardToHero>(stream, pair);
#else
        return null;
#endif

    }
    [ServerFunction("GetCards")]
    public static string GetCards(StreamWriter stream) {
#if SERVER
        return SendData<CardDatabase>(stream);
#else
        return null;
#endif

    }
    [ServerFunction("GetCardsByKeyPair")]
    public static string GetCardsByKeyPair(StreamWriter stream, string pair) {
#if SERVER
        return SendData<CardDatabase>(stream, pair);
#else
        return null;
#endif


    }
    [ServerFunction("GetEnemy")]
    public static string GetEnemy(StreamWriter stream) {
#if SERVER
    return SendData<EnemyDatabase>(stream);
#else
        return null;
#endif

    }
    [ServerFunction("GetRandomEnemy")]
    public static string GetRandomEnemy(StreamWriter stream) {
#if SERVER
        return SendDataOfRandom<EnemyDatabase>(stream);
#else
        return null;

#endif

    }
    [ServerFunction("GetEnemyByKeyPair")]
    public static string GetEnemyByKeyPair(StreamWriter stream, string pair) {
#if SERVER
        return SendData<EnemyDatabase>(stream, pair);
#else
        return null;
#endif

    }
    [ServerFunction("GetEnemyToEnemySkill")]
    public static string GetEnemyToEnemySkill(StreamWriter stream) {
#if SERVER
        return SendData<EnemyToEnemySkill>(stream);
#else
        return null;

#endif

    }
    [ServerFunction("GetEnemyToEnemySkillByKeyPair")]
    public static string GetEnemyToEnemySkillByKeyPair(StreamWriter stream, string pair) {
#if SERVER
        return SendData<EnemyToEnemySkill>(stream, pair);
#else
        return null;
#endif

    }
    [ServerFunction("GetEnemySkill")]
    public static string GetEnemySkill(StreamWriter stream) {
#if SERVER
        return SendData<EnemySkillDatabase>(stream);
#else
        return null;
#endif

    }
    [ServerFunction("GetEnemySkillByKeyPair")]
    public static string GetEnemySkillByKeyPair(StreamWriter stream, string pair) {
#if SERVER
        return SendData<EnemySkillDatabase>(stream, pair);
#else
        return null;
#endif

    }
    [ServerFunction("SendMessage")]
    public static string SendMessage(StreamWriter stream, string message) {
#if SERVER
        return message;
#else
        return null;
#endif


    }
}
