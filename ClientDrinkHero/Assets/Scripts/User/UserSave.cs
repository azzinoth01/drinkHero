using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[Serializable]
public class UserSave
{
    [SerializeField] private string _name;
    [SerializeField] private int _gold;
    [SerializeField] private List<SavedHero> _collectedHeroes;
    [SerializeField] private List<SavedItem> _collectedItems;

    private static string _savePath = Application.persistentDataPath + "/UserSave.sav";
    private const string _encodingKey = "DrinkHeroEncodingKey";

    public int Gold {
        get {
            return _gold;
        }

        set {
            _gold = value;
        }
    }

    public List<SavedHero> CollectedHeroes {
        get {
            return _collectedHeroes;
        }
    }

    public List<SavedItem> CollectedItems {
        get {
            return _collectedItems;
        }
    }

    public string Name {
        get {
            return _name;
        }

        set {
            _name = value;
        }
    }

    public UserSave() {
        _collectedHeroes = new List<SavedHero>();
        _collectedItems = new List<SavedItem>();
    }

    public void Save() {

        string json = JsonUtility.ToJson(this);
        json = Encode(json,_encodingKey);
        json = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        File.WriteAllText(_savePath,json);
    }

    public static UserSave LoadSave(UserStartingData startingData) {
        if(startingData == null) {
            return LoadSave();
        }
        UserSave save = new UserSave();
        if(File.Exists(_savePath)) {
            string json = File.ReadAllText(_savePath);
            json = Encoding.UTF8.GetString(Convert.FromBase64String(json));
            json = Encode(json,_encodingKey);
            JsonUtility.FromJsonOverwrite(json,save);
        }
        else {
            save._gold = startingData.Gold;
            save._name = startingData.Name;

            foreach(StartingItem startingItem in startingData.StartingItems) {
                save.AddItem(startingItem.UpgradeItem,startingItem.Amount);
            }
        }
        foreach(HeroData heroData in startingData.StartingHeroes) {
            bool found = false;
            foreach(SavedHero savedHero in save.CollectedHeroes) {
                if(savedHero.Id == heroData.Id) {
                    found = true;
                    break;
                }
            }
            if(found == false) {
                SavedHero hero = new SavedHero(heroData);
                save.CollectedHeroes.Add(hero);
            }
        }

        return save;
    }

    private static UserSave LoadSave() {

        UserSave save = new UserSave();

        if(File.Exists(_savePath)) {
            string json = File.ReadAllText(_savePath);
            JsonUtility.FromJsonOverwrite(json,save);
        }

        return save;

    }

    private static string Encode(string input,string encodingKey) {

        char[] encodedInput = new char[input.Length];

        for(int i = 0; i < input.Length; i++) {
            encodedInput[i] = (char) (input[i] ^ encodingKey[i % encodingKey.Length]);
        }

        return new string(encodedInput);
    }

    public void AddItem(UpgradeItemData item,int amount) {
        foreach(SavedItem savedItem in _collectedItems) {
            if(savedItem.Id == item.Id) {
                savedItem.Amount = savedItem.Amount + amount;
                return;
            }
        }
        SavedItem newSavedItem = new SavedItem(item);
        newSavedItem.Amount = amount;
        _collectedItems.Add(newSavedItem);

    }
    public void UpgradeHeroCardLevel(HeroObject hero,int cardIndex) {
        foreach(SavedHero savedHero in _collectedHeroes) {
            if(hero.Id == savedHero.Id) {
                savedHero.UpgradeCardLevel(cardIndex);
                break;
            }
        }
    }
    public void AddHero(HeroData heroData) {
        SavedHero newHero = new SavedHero(heroData);
        _collectedHeroes.Add(newHero);
    }
}
