using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


[CsvImportable]
[CreateAssetMenu(fileName = "HeroData",menuName = "Scriptable Objects/HeroData")]
public class HeroData : ScriptableObject
{
    [ReadOnly]
    [SerializeField] private string _id;
    [SerializeField] private string _externalID;

    [SerializeField] private int _shield;
    [SerializeField] private int _health;
    [SerializeField] private string _spritePath;
    [SerializeField] private string _name;
    [SerializeField] private List<CardData> _cardList;
    [SerializeField] private GameObject _prefab;


    public HeroData() {
        _id = Guid.NewGuid().ToString("N");
        _cardList = new List<CardData>();
    }

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

    public string ExternalID {
        get {
            return _externalID;
        }
    }

    public GameObject Prefab {
        get {
            return _prefab;
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
