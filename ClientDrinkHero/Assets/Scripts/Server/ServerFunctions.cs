

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

#endif

    private static string SendData<T>(StreamWriter stream, string pair = "") where T : DatabaseItem, new() {
#if SERVER
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
#endif
#if CLIENT
        return null;
#endif
    }
    private static string SendDataOfRandom<T>(StreamWriter stream) where T : DatabaseItem, new() {
#if SERVER
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



#endif
#if CLIENT
        return null;
#endif
    }




    [ServerFunction("GetHeros")]
    public static string GetHeros(StreamWriter stream) {
        return SendData<HeroDatabase>(stream);

    }

    [ServerFunction("GetHerosByKeyPair")]
    public static string GetHerosByKeyPair(StreamWriter stream, string pair) {
        return SendData<HeroDatabase>(stream, pair);

    }
    [ServerFunction("GetCardToHero")]
    public static string GetCardToHero(StreamWriter stream) {

        return SendData<CardToHero>(stream);


    }
    [ServerFunction("GetCardToHeroByKeyPair")]
    public static string GetCardToHeroByKeyPair(StreamWriter stream, string pair) {

        return SendData<CardToHero>(stream, pair);

    }
    [ServerFunction("GetCards")]
    public static string GetCards(StreamWriter stream) {

        return SendData<CardDatabase>(stream);

    }
    [ServerFunction("GetCardsByKeyPair")]
    public static string GetCardsByKeyPair(StreamWriter stream, string pair) {

        return SendData<CardDatabase>(stream, pair);


    }
    [ServerFunction("GetEnemy")]
    public static string GetEnemy(StreamWriter stream) {
        return SendData<EnemyDatabase>(stream);

    }
    [ServerFunction("GetRandomEnemy")]
    public static string GetRandomEnemy(StreamWriter stream) {

        return SendDataOfRandom<EnemyDatabase>(stream);


    }
    [ServerFunction("GetEnemyByKeyPair")]
    public static string GetEnemyByKeyPair(StreamWriter stream, string pair) {

        return SendData<EnemyDatabase>(stream, pair);

    }
    [ServerFunction("GetEnemyToEnemySkill")]
    public static string GetEnemyToEnemySkill(StreamWriter stream) {

        return SendData<EnemyToEnemySkill>(stream);


    }
    [ServerFunction("GetEnemyToEnemySkillByKeyPair")]
    public static string GetEnemyToEnemySkillByKeyPair(StreamWriter stream, string pair) {

        return SendData<EnemyToEnemySkill>(stream, pair);

    }
    [ServerFunction("GetEnemySkill")]
    public static string GetEnemySkill(StreamWriter stream) {

        return SendData<EnemySkillDatabase>(stream);

    }
    [ServerFunction("GetEnemySkillByKeyPair")]
    public static string GetEnemySkillByKeyPair(StreamWriter stream, string pair) {

        return SendData<EnemySkillDatabase>(stream, pair);

    }
    [ServerFunction("SendMessage")]
    public static string SendMessage(StreamWriter stream, string message) {

        return message;


    }
}
