using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "UserStartingData",menuName = "Scriptable Objects/UserStartingData")]
public class UserStartingData : ScriptableObject
{
    [ReadOnly]
    [SerializeField] private string _id;

    [SerializeField] private string _name;
    [SerializeField] private int _gold;
    [SerializeField] private List<HeroData> _startingHeroes;
    [SerializeField] private List<StartingItem> _startingItems;


    public UserStartingData() {
        _id = Guid.NewGuid().ToString("N");
        _startingHeroes = new List<HeroData>();
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

    public List<HeroData> StartingHeroes {
        get {
            return _startingHeroes;
        }
    }

    public int Gold {
        get {
            return _gold;
        }
    }

    public List<StartingItem> StartingItems {
        get {
            return _startingItems;
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