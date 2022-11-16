using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ServerFunctions {


    //private static Regex _keyRegex = new Regex("[\\s\\S]*?(?=\")");
    //private static Regex _valueRegex = new Regex("(?<=\")[\\s\\S]*(?=\")");

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
            data = data + "\"" + item.GetType().GetProperty(pair.Value).GetValue(item).ToString() + "\"";
        }
        return data;
    }

    public static (string, string) ResolveKeyValuePair(string pair) {

        string key = RegexPatterns.PropertyName.Match(pair).Value;
        string value = RegexPatterns.PropertyValue.Match(pair).Value;

        return (key, value);
    }

    [ServerFunction("GetHeros")]
    public static void GetHeros(StreamWriter stream) {
        List<HeroDatabase> heroes = DatabaseManager.GetDatabaseList<HeroDatabase>();

        string data = CreateTransmissionString<HeroDatabase>(heroes);
        stream.Write(data);


    }
    [ServerFunction("GetHerosByKeyPair")]
    public static void GetHerosByKeyPair(StreamWriter stream, string pair) {

        (string, string) keyValue = ResolveKeyValuePair(pair);

        string key = keyValue.Item1;
        long id = long.Parse(keyValue.Item2);

        List<HeroDatabase> heroes = DatabaseManager.GetDatabaseList<HeroDatabase>(key, id);

        string data = CreateTransmissionString<HeroDatabase>(heroes);
        stream.Write(data);
    }
    [ServerFunction("GetCardToHero")]
    public static void GetCardToHero(StreamWriter stream) {
        List<CardToHero> cardToHero = DatabaseManager.GetDatabaseList<CardToHero>();

        string data = CreateTransmissionString<CardToHero>(cardToHero);
        stream.Write(data);
    }
    [ServerFunction("GetCardToHeroByKeyPair")]
    public static void GetCardToHeroByKeyPair(StreamWriter stream, string pair) {
        (string, string) keyValue = ResolveKeyValuePair(pair);

        string key = keyValue.Item1;
        long id = long.Parse(keyValue.Item2);

        List<CardToHero> cardToHero = DatabaseManager.GetDatabaseList<CardToHero>(key, id);
        string data = CreateTransmissionString<CardToHero>(cardToHero);
        stream.Write(data);
    }
    [ServerFunction("GetCards")]
    public static void GetCards(StreamWriter stream) {
        List<CardDatabase> cards = DatabaseManager.GetDatabaseList<CardDatabase>();

        string data = CreateTransmissionString<CardDatabase>(cards);
        stream.Write(data);
    }
    [ServerFunction("GetCardsByKeyPair")]
    public static void GetCardsByKeyPair(StreamWriter stream, string pair) {
        (string, string) keyValue = ResolveKeyValuePair(pair);

        string key = keyValue.Item1;
        long id = long.Parse(keyValue.Item2);

        List<CardDatabase> cards = DatabaseManager.GetDatabaseList<CardDatabase>(key, id);

        string data = CreateTransmissionString<CardDatabase>(cards);
        stream.Write(data);
    }
    [ServerFunction("GetEnemy")]
    public static void GetEnemy(StreamWriter stream) {
        List<EnemyDatabase> enemy = DatabaseManager.GetDatabaseList<EnemyDatabase>();

        string data = CreateTransmissionString<EnemyDatabase>(enemy);
        stream.Write(data);
    }
    [ServerFunction("GetEnemyByKeyPair")]
    public static void GetEnemyByKeyPair(StreamWriter stream, string pair) {
        (string, string) keyValue = ResolveKeyValuePair(pair);

        string key = keyValue.Item1;
        long id = long.Parse(keyValue.Item2);

        List<EnemyDatabase> enemy = DatabaseManager.GetDatabaseList<EnemyDatabase>(key, id);
        string data = CreateTransmissionString<EnemyDatabase>(enemy);
        stream.Write(data);
    }
    [ServerFunction("GetEnemyToEnemySkill")]
    public static void GetEnemyToEnemySkill(StreamWriter stream) {
        List<EnemyToEnemySkill> enemyToEnemySkill = DatabaseManager.GetDatabaseList<EnemyToEnemySkill>();

        string data = CreateTransmissionString<EnemyToEnemySkill>(enemyToEnemySkill);
        stream.Write(data);
    }
    [ServerFunction("GetEnemyToEnemySkillByKeyPair")]
    public static void GetEnemyToEnemySkillByKeyPair(StreamWriter stream, string pair) {
        (string, string) keyValue = ResolveKeyValuePair(pair);

        string key = keyValue.Item1;
        long id = long.Parse(keyValue.Item2);

        List<EnemyToEnemySkill> enemyToEnemySkill = DatabaseManager.GetDatabaseList<EnemyToEnemySkill>(key, id);
        string data = CreateTransmissionString<EnemyToEnemySkill>(enemyToEnemySkill);
        stream.Write(data);
    }
    [ServerFunction("GetEnemySkill")]
    public static void GetEnemySkill(StreamWriter stream) {
        List<EnemySkillDatabase> enemyToEnemySkill = DatabaseManager.GetDatabaseList<EnemySkillDatabase>();

        string data = CreateTransmissionString<EnemySkillDatabase>(enemyToEnemySkill);
        stream.Write(data);
    }
    [ServerFunction("GetEnemySkillByKeyPair")]
    public static void GetEnemySkillByKeyPair(StreamWriter stream, string pair) {
        (string, string) keyValue = ResolveKeyValuePair(pair);

        string key = keyValue.Item1;
        long id = long.Parse(keyValue.Item2);

        List<EnemySkillDatabase> enemyToEnemySkill = DatabaseManager.GetDatabaseList<EnemySkillDatabase>(key, id);
        string data = CreateTransmissionString<EnemySkillDatabase>(enemyToEnemySkill);
        stream.Write(data);
    }
    [ServerFunction("SendMessage")]
    public static void SendMessage(StreamWriter stream, string message) {
        Debug.LogError(message);
    }
}
