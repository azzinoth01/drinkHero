using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class CSVImporter
{

    public static List<T> ImportList<T>(string csvText) where T : new() {

        string[] lines = csvText.Split('\n');
        if(lines.Length < 2) {
            Debug.LogWarning("imported empty List");
            return new List<T>();
        }
        string[] headers = lines[0].Split(';');

        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Dictionary<int,FieldInfo> fieldMap = new Dictionary<int,FieldInfo>();

        for(int i = 0; i < headers.Length; i++) {

            string header = headers[i].Trim();

            foreach(FieldInfo field in fields) {
                if(field.Name.Equals(header,StringComparison.OrdinalIgnoreCase)) {
                    fieldMap[i] = field;
                    break;
                }
            }
        }
        List<T> result = new List<T>();

        for(int i = 1; i < lines.Length; i++) {

            if(string.IsNullOrWhiteSpace(lines[i])) {
                continue;
            }

            string[] values = lines[i].Split(';');
            T instance = new T();

            foreach(KeyValuePair<int,FieldInfo> pair in fieldMap) {
                int index = pair.Key;
                FieldInfo field = pair.Value;

                if(index >= values.Length) {
                    continue;
                }
                object parsedValue = ParseValue(values[index],field.FieldType);
                field.SetValue(instance,parsedValue);
            }
            result.Add(instance);
        }


        return result;
    }

    private static object ParseValue(string value,Type type) {
        value = value.Trim();

        if(type == typeof(string)) {
            return value;
        }
        else if(type == typeof(int)) {
            return int.Parse(value);
        }
        else if(type == typeof(float)) {
            return float.Parse(value);
        }
        else if(type == typeof(bool)) {
            if(bool.TryParse(value,out bool result)) {
                return result;
            }
            if(value == "1") {
                return true;
            }
            else {
                return false;
            }
        }
        else if(type.IsEnum) {
            return Enum.Parse(type,value);
        }
        else if(typeof(ScriptableObject).IsAssignableFrom(type)) {
            GUID[] guids = AssetDatabase.FindAssetGUIDs($"t:{type.Name}");
            foreach(GUID guid in guids) {
                object data = AssetDatabase.LoadAssetByGUID(guid,type);
                FieldInfo externalID = type.GetField("_externalID",BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if(externalID.GetValue(data).ToString() == value) {
                    return data;
                }
            }
            return null;
        }
        else {
            Debug.LogError("unsuported type: " + type.Name);
            return null;
        }
    }

    public static void ImportAssetList(string csvText,string folderPath,Type type) {

        string[] lines = csvText.Split('\n');
        if(lines.Length < 2) {
            Debug.LogWarning("imported empty List");
            return;
        }
        string[] headers = lines[0].Split(';');

        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Dictionary<int,FieldInfo> fieldMap = new Dictionary<int,FieldInfo>();
        int idIndex = -1;
        for(int i = 0; i < headers.Length; i++) {

            string header = headers[i].Trim();

            if(header == "_externalID") {
                idIndex = i;
            }

            foreach(FieldInfo field in fields) {
                if(field.Name.Equals(header,StringComparison.OrdinalIgnoreCase)) {
                    fieldMap[i] = field;
                    break;
                }
            }
        }
        if(idIndex == -1) {
            Debug.LogError("No _externalID found");
            return;
        }

        for(int i = 1; i < lines.Length; i++) {

            if(string.IsNullOrWhiteSpace(lines[i])) {
                continue;
            }


            string[] values = lines[i].Split(';');


            string assetPath = folderPath + "/" + values[idIndex] + ".asset";

            ScriptableObject asset = (ScriptableObject) AssetDatabase.LoadAssetAtPath(assetPath,type);
            if(asset == null) {
                asset = ScriptableObject.CreateInstance(type);
                AssetDatabase.CreateAsset(asset,assetPath);
            }

            foreach(KeyValuePair<int,FieldInfo> pair in fieldMap) {
                int index = pair.Key;
                FieldInfo field = pair.Value;

                if(index >= values.Length) {
                    continue;
                }
                object parsedValue = ParseValue(values[index],field.FieldType);
                field.SetValue(asset,parsedValue);
            }
            EditorUtility.SetDirty(asset);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
