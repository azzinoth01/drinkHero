using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


[CsvImportable]
[CreateAssetMenu(fileName = "CardData",menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    [ReadOnly]
    [SerializeField] private string _id;
    [SerializeField] private string _externalID;

    [SerializeField] private string _name;
    [SerializeField] private string _text;
    [SerializeField] private int _cost;
    [SerializeField] private string _iconPath;
    [SerializeField] private CardData _upgradeTo;
    [SerializeField] private List<CardEffectData> _cardEffectList;
    [SerializeField] private UpgradeItemData _upgradeItem;
    [SerializeField] private int _upgradeItemAmount;
    [SerializeField] private string _animationKey;




    public CardData() {
        _id = Guid.NewGuid().ToString("N");
        _cardEffectList = new List<CardEffectData>();
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

    public List<CardEffectData> CardEffectList {
        get {
            return _cardEffectList;
        }
    }

    public string ExternalID {
        get {
            return _externalID;
        }
    }
    public UpgradeItemData UpgradeItem {
        get {
            return _upgradeItem;
        }
    }
#if UNITY_EDITOR
    [Button("Save Asset")]
    private void SaveAsset() {
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }
#endif
}