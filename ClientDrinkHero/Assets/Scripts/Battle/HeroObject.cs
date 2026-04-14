using System.Collections.Generic;
using UnityEngine;

public class HeroObject
{
    private string _id;
    private int _shield;
    private int _health;
    private string _spritePath;
    private string _name;
    private List<CardData> _cardList;
    private GameObject _prefab;

    public string Id {
        get {
            return _id;
        }
    }

    public int Shield {
        get {
            return _shield;
        }
    }

    public int Health {
        get {
            return _health;
        }
    }

    public string SpritePath {
        get {
            return _spritePath;
        }
    }

    public string Name {
        get {
            return _name;
        }
    }

    public List<CardData> CardList {
        get {
            return _cardList;
        }
    }

    public GameObject Prefab {
        get {
            return _prefab;
        }
    }

    public HeroObject(HeroData data,List<int> cardUpgradeLevels) {
        _id = data.Id;
        _shield = data.Shield;
        _health = data.Health;
        _spritePath = data.SpritePath;
        _name = data.Name;
        _prefab = data.Prefab;
        _cardList = new List<CardData>(data.CardList);
        for(int i = 0; i < cardUpgradeLevels.Count && i < _cardList.Count; i++) {
            for(int x = 0; x < cardUpgradeLevels[i]; x++) {
                if(_cardList[i].UpgradeTo == null) {
                    break;
                }
                _cardList[i] = _cardList[i].UpgradeTo;
            }
        }
    }

    public void UpgradeCardLevel(int cardIndex) {
        if(_cardList.Count <= cardIndex || _cardList[cardIndex].UpgradeTo == null) {
            return;
        }
        _cardList[cardIndex] = _cardList[cardIndex].UpgradeTo;
    }
}