using Sirenix.OdinInspector;
using System;
using UnityEngine;

[CsvImportable]
[CreateAssetMenu(fileName = "EnemyData",menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [ReadOnly]
    [SerializeField] private string _id;
    [SerializeField] private string _externalID;

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _shield;
    [SerializeField] private string _spritePath;
    [SerializeField] private bool _isBoss;
    [SerializeField] private int _moneyDrop;

    public EnemyData() {
        _id = Guid.NewGuid().ToString("N");
    }

    public string Id {
        get {
            return _id;
        }
    }

    public int MaxHealth {
        get {
            return _maxHealth;
        }
    }

    public int Shield {
        get {
            return _shield;
        }
    }

    public string SpritePath {
        get {
            return _spritePath;
        }
    }

    public bool IsBoss {
        get {
            return _isBoss;
        }
    }

    public int MoneyDrop {
        get {
            return _moneyDrop;
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