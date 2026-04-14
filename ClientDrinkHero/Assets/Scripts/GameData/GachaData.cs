using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CsvImportable]
[CreateAssetMenu(fileName = "GachaData",menuName = "Scriptable Objects/GachaData")]
public class GachaData : ScriptableObject
{
    [ReadOnly]
    [SerializeField] private string _id;
    [SerializeField] private string _externalID;

    [SerializeField] private string _name;
    [SerializeField] private string _costType;
    [SerializeField] private int _costSingelPull;
    [SerializeField] private int _costMultiPull;
    [SerializeField] private int _multiPullAmount;
    [SerializeField] private List<WeightedGachaCategory> _categories;


    public GachaData() {
        _id = Guid.NewGuid().ToString("N");
        _categories = new List<WeightedGachaCategory>();
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

    public string CostType {
        get {
            return _costType;
        }
    }

    public int CostSingelPull {
        get {
            return _costSingelPull;
        }
    }

    public int CostMultiPull {
        get {
            return _costMultiPull;
        }
    }

    public int MultiPullAmount {
        get {
            return _multiPullAmount;
        }
    }

    public string ExternalID {
        get {
            return _externalID;
        }
    }

    public List<WeightedGachaCategory> Categories {
        get {
            return _categories;
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