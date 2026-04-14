using Sirenix.OdinInspector;
using System;
using UnityEngine;

[CsvImportable]
[CreateAssetMenu(fileName = "UpgradeItemData",menuName = "Scriptable Objects/UpgradeItemData")]
public class UpgradeItemData : ScriptableObject
{
    [ReadOnly]
    [SerializeField] private string _id;
    [SerializeField] private string _externalID;

    [SerializeField] private string _name;
    [SerializeField] private string _text;
    [SerializeField] private string _spritePath;

    public UpgradeItemData() {
        _id = Guid.NewGuid().ToString("N");
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

    public string SpritePath {
        get {
            return _spritePath;
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