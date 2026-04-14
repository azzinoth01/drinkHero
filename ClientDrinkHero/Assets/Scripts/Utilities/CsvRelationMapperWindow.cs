using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ColumnMapping
{
    public string Header;

    public string AField;
    public string BField;

    public MappingMode Mode;
}

public enum MappingMode
{
    Ignore,
    AField,
    BField,
    RelationAId,
    RelationBId
}


public class CsvRelationMapperWindow : EditorWindow
{
    private string[] _headers;
    private int _headerAIndex;
    private int _headerBIndex;
    private FieldInfo _matchAField;
    private FieldInfo _matchBField;
    private FieldInfo _listAField;
    private FieldInfo _listBField;
    private string _csvText;
    private string _csvPath;
    private Type _typeA;
    private Type _typeB;
    private List<Type> _types;
    private string[] _typeNames;
    private int _typeIndexA;
    private int _typeIndexB;

    [MenuItem("Tools/CSV Relation Mapper")]
    public static void Open() {
        GetWindow<CsvRelationMapperWindow>("CSV Mapper");
    }

    private void OnEnable() {
        _types = TypeCache.GetTypesDerivedFrom<ScriptableObject>().Where(t => !t.IsAbstract && t.GetCustomAttribute<CsvImportableAttribute>() != null).ToList();

        _typeNames = _types.Select(t => t.Name).ToArray();
    }

    private void OnGUI() {
        DrawCSV();
        DrawTypes();

        if(_headers != null)
            DrawMappingUI();

        if(GUILayout.Button("Import"))
            Import();
    }

    private void DrawCSV() {
        if(GUILayout.Button("Select CSV")) {
            _csvPath = EditorUtility.OpenFilePanel("CSV",Application.dataPath,"csv");

            if(string.IsNullOrEmpty(_csvPath)) {
                return;
            }
            _csvText = File.ReadAllText(_csvPath);
            _headers = _csvText.Split('\n')[0].Split(';').Select(h => h.Trim()).ToArray();

        }

        if(string.IsNullOrEmpty(_csvPath)) {
            return;
        }
        EditorGUILayout.LabelField(_csvPath);
    }

    private void DrawTypes() {
        EditorGUILayout.Space();

        _typeIndexA = EditorGUILayout.Popup("Type A",_typeIndexA,_typeNames);
        _typeIndexB = EditorGUILayout.Popup("Type B",_typeIndexB,_typeNames);

        _typeA = _types[_typeIndexA];
        _typeB = _types[_typeIndexB];
    }


    private void DrawMappingUI() {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Matching",EditorStyles.boldLabel);


        EditorGUILayout.LabelField("Type A",EditorStyles.boldLabel);
        _headerAIndex = EditorGUILayout.Popup("CSV",_headerAIndex,_headers);
        _matchAField = DrawFieldPopup("Field",_typeA,_matchAField);


        EditorGUILayout.LabelField("Type B",EditorStyles.boldLabel);
        _headerBIndex = EditorGUILayout.Popup("CSV",_headerBIndex,_headers);
        _matchBField = DrawFieldPopup("Field",_typeB,_matchBField);


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Relations",EditorStyles.boldLabel);
        _listAField = DrawListPopup("List side A",_typeA,_typeB,_listAField,true);
        _listBField = DrawListPopup("List side B",_typeB,_typeA,_listBField,true);
    }

    private FieldInfo DrawFieldPopup(string label,Type type,FieldInfo current) {
        FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        string[] names = fields.Select(f => f.Name).ToArray();
        int index = 0;
        if(current != null) {
            index = Array.IndexOf(names,current.Name);
        }

        index = EditorGUILayout.Popup(label,index,names);
        if(fields.Length <= index) {
            return null;
        }

        return fields[index];
    }

    private FieldInfo DrawListPopup(string label,Type owner,Type target,FieldInfo current,bool allowNone) {
        List<FieldInfo> fields = owner.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(f =>
                f.FieldType.IsGenericType &&
                f.FieldType.GetGenericTypeDefinition() == typeof(List<>) &&
                f.FieldType.GetGenericArguments()[0] == target)
            .ToList();

        List<string> names = new();

        if(allowNone) {
            names.Add("None");
        }
        names.AddRange(fields.Select(f => f.Name));

        int index = 0;

        if(current != null) {
            index = names.IndexOf(current.Name);
        }

        index = EditorGUILayout.Popup(label,index,names.ToArray());

        if(allowNone && index == 0) {
            return null;
        }
        int fieldIndex = index;
        if(allowNone) {
            fieldIndex = fieldIndex - 1;
        }
        if(fields.Count <= fieldIndex || fieldIndex < 0) {
            return null;
        }
        return fields[fieldIndex];
    }

    private void Import() {
        var lines = _csvText.Split('\n');

        var aAssets = LoadAll(_typeA);
        var bAssets = LoadAll(_typeB);

        var aLookup = BuildLookupTable(aAssets,_matchAField);
        var bLookup = BuildLookupTable(bAssets,_matchBField);


        for(int i = 1; i < lines.Length; i++) {
            if(string.IsNullOrWhiteSpace(lines[i])) continue;

            var values = lines[i].Split(';');

            string aKey = values[_headerAIndex].Trim();
            string bKey = values[_headerBIndex].Trim();

            if(aLookup.TryGetValue(aKey,out var aObj) == false) {
                continue;
            }
            if(bLookup.TryGetValue(bKey,out var bObj) == false) {
                continue;
            }
            if(_listAField != null) {
                AddToList(aObj,_listAField,bObj);
            }
            if(_listBField != null) {
                AddToList(bObj,_listBField,aObj);
            }

        }

        AssetDatabase.SaveAssets();
        Debug.Log("Import complete");
    }

    private Dictionary<string,ScriptableObject> BuildLookupTable(List<ScriptableObject> assets,FieldInfo field) {
        var lookupTable = new Dictionary<string,ScriptableObject>();

        foreach(var a in assets) {
            string value = field.GetValue(a).ToString();

            if(string.IsNullOrEmpty(value)) {
                continue;
            }
            lookupTable[value] = a;
        }

        return lookupTable;
    }

    private void AddToList(ScriptableObject obj,FieldInfo field,ScriptableObject value) {
        IList list = field.GetValue(obj) as IList;

        if(list.Contains(value) == true) {
            return;
        }
        list.Add(value);
        EditorUtility.SetDirty(obj);
    }

    private List<ScriptableObject> LoadAll(Type type) {
        GUID[] guids = AssetDatabase.FindAssetGUIDs($"t:{type.Name}");
        List<ScriptableObject> list = new List<ScriptableObject>();
        foreach(GUID guid in guids) {
            list.Add(AssetDatabase.LoadAssetByGUID<ScriptableObject>(guid));
        }
        return list;
    }
}