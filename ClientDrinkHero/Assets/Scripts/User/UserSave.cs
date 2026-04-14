using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class UserSave
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private int _gold;
    [SerializeField] private List<SavedHero> _collectedHeroes;
    [SerializeField] private List<SavedItem> _collectedItems;

    private static string _savePath = Application.persistentDataPath + "/UserSave.json";


    public int Id {
        get {
            return _id;
        }

        set {
            _id = value;
        }
    }

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
        _id = -1;
        _collectedHeroes = new List<SavedHero>();
        _collectedItems = new List<SavedItem>();
    }

    public void Save() {

        string json = JsonUtility.ToJson(this);

        File.WriteAllText(_savePath,json);
    }

    public static UserSave LoadSave(UserStartingData startingData) {
        if(startingData == null) {
            return LoadSave();
        }
        UserSave save = new UserSave();
        if(File.Exists(_savePath)) {
            string json = File.ReadAllText(_savePath);
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
        foreach(SavedHero savedHero in GameDataInstance.Instance.UserSave.CollectedHeroes) {
            if(hero.Id == savedHero.Id) {
                savedHero.UpgradeCardLevel(cardIndex);
                break;
            }
        }
    }
}
