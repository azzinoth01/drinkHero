using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class CardData
{
    [ReadOnly]
    [SerializeField] private string _id;

    [SerializeField] private string _name;
    [SerializeField] private string _text;
    [SerializeField] private int _cost;
    [SerializeField] private string _spritePath;
    [SerializeField] private string _iconPath;
    [SerializeField] private CardData _upgradeTo;
    [SerializeField] private List<string> _cardEffectList;
    //[SerializeField] private UpgradeItemDatabase _upgradeItem;
    [SerializeField] private int _upgradeItemAmount;
    [SerializeField] private string _animationKey;


    public CardData() {
        _id = Guid.NewGuid().ToString("N");
        _cardEffectList = new List<string>();
    }

    public string Id {
        get {
            return _id;
        }
    }

    public string Name {
        get {
            return _name;
        }
    }

    public string Text {
        get {
            return _text;
        }
    }

    public int Cost {
        get {
            return _cost;
        }
    }

    public string SpritePath {
        get {
            return _spritePath;
        }
    }

    public string IconPath {
        get {
            return _iconPath;
        }
    }

    public CardData UpgradeTo {
        get {
            return _upgradeTo;
        }
    }

    public int UpgradeItemAmount {
        get {
            return _upgradeItemAmount;
        }
    }

    public string AnimationKey {
        get {
            return _animationKey;
        }
    }

    public List<string> CardEffectList {
        get {
            return _cardEffectList;
        }
    }
}