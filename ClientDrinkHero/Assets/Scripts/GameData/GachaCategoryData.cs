using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


[CsvImportable]
[CreateAssetMenu(fileName = "GachaCategoryData",menuName = "Scriptable Objects/GachaCategoryData")]
public class GachaCategoryData : ScriptableObject
{
    [ReadOnly]
    [SerializeField] private string _id;
    [SerializeField] private string _externalID;

    [SerializeField] private string _name;
    [SerializeField] private List<GachaItem> _gachaitemList;

    public GachaCategoryData() {
        _id = Guid.NewGuid().ToString("N");
        _gachaitemList = new List<GachaItem>();
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

    public List<GachaItem> GachaitemList {
        get {
            return _gachaitemList;
        }
    }

    public string ExternalID {
        get {
            return _externalID;
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