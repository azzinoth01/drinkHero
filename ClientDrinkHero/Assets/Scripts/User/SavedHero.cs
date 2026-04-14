using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedHero
{
    [SerializeField] private string _id;
    [SerializeField] private List<int> _cardUpgradeLevels;

    public string Id {
        get {
            return _id;
        }
    }

    public List<int> CardUpgradeLevels {
        get {
            return _cardUpgradeLevels;
        }
    }

    private SavedHero() {

    }

    public SavedHero(HeroData data) {
        _id = data.Id;
        _cardUpgradeLevels = new List<int>();
        foreach(CardData card in data.CardList) {
            _cardUpgradeLevels.Add(0);
        }
    }

    public void UpgradeCardLevel(int index) {
        while(index >= _cardUpgradeLevels.Count) {
            _cardUpgradeLevels.Add(0);
        }
        _cardUpgradeLevels[index] = _cardUpgradeLevels[index] + 1;
    }
}