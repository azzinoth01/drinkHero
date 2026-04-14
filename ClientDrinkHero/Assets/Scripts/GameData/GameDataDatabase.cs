using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "GameDataDatabase",menuName = "Scriptable Objects/GameDataDatabase")]
public class GameDataDatabase : ScriptableObject
{
    [ReadOnly]
    [SerializeField] private List<HeroData> _heroData;
    [ReadOnly]
    [SerializeField] private List<CardData> _cardData;
    [ReadOnly]
    [SerializeField] private List<CardEffectData> _cardEffectData;
    [ReadOnly]
    [SerializeField] private List<GachaData> _gachaData;
    [ReadOnly]
    [SerializeField] private List<GachaCategoryData> _gachaCategorieData;
    [ReadOnly]
    [SerializeField] private List<UpgradeItemData> _upgradeItemData;
    [ReadOnly]
    [SerializeField] private List<EnemyData> _enemyData;

    public List<HeroData> HeroData {
        get {
            return _heroData;
        }
    }

    public List<CardData> CardData {
        get {
            return _cardData;
        }
    }

    public List<CardEffectData> CardEffectData {
        get {
            return _cardEffectData;
        }
    }

    public List<GachaData> GachaData {
        get {
            return _gachaData;
        }
    }

    public List<GachaCategoryData> GachaCategorieData {
        get {
            return _gachaCategorieData;
        }
    }

    public List<UpgradeItemData> UpgradeItemData {
        get {
            return _upgradeItemData;
        }
    }

    public List<EnemyData> EnemyData {
        get {
            return _enemyData;
        }
    }

    public Dictionary<string,ScriptableObject> GetDatabase() {

        Dictionary<string,ScriptableObject> database = new Dictionary<string,ScriptableObject>();
        foreach(HeroData data in _heroData) {
            database[data.Id] = data;
        }
        foreach(CardData data in _cardData) {
            database[data.Id] = data;
        }
        foreach(CardEffectData data in _cardEffectData) {
            database[data.Id] = data;
        }
        foreach(GachaData data in _gachaData) {
            database[data.Id] = data;
        }
        foreach(GachaCategoryData data in _gachaCategorieData) {
            database[data.Id] = data;
        }
        foreach(UpgradeItemData data in _upgradeItemData) {
            database[data.Id] = data;
        }
        foreach(EnemyData data in _enemyData) {
            database[data.Id] = data;
        }
        return database;
    }

#if UNITY_EDITOR
    [Button("Update Lists")]
    private void UpdateLists() {
        _heroData = LoadAssetList<HeroData>();
        _cardData = LoadAssetList<CardData>();
        _cardEffectData = LoadAssetList<CardEffectData>();
        _gachaData = LoadAssetList<GachaData>();
        _gachaCategorieData = LoadAssetList<GachaCategoryData>();
        _upgradeItemData = LoadAssetList<UpgradeItemData>();
        _enemyData = LoadAssetList<EnemyData>();
    }

    private List<T> LoadAssetList<T>() where T : ScriptableObject {
        List<T> list = new List<T>();
        GUID[] guids = AssetDatabase.FindAssetGUIDs($"t:{typeof(T).Name}");
        foreach(GUID guid in guids) {
            T data = AssetDatabase.LoadAssetByGUID<T>(guid);
            list.Add(data);
        }

        return list;
    }

    [Button("Save Asset")]
    private void SaveAsset() {
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }
#endif
}
