using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CsvImporterWindow : EditorWindow
{
    private string _csvPath;
    private string _csvText;

    private List<Type> _types;
    private string[] _typeNames;
    private int _selectedIndex;

    private string _folderPath = "Assets";

    [MenuItem("Tools/CSV Assets Importer")]
    public static void Open() {
        GetWindow<CsvImporterWindow>("CSV Importer");
    }

    private void OnEnable() {
        _types = TypeCache.GetTypesDerivedFrom<ScriptableObject>().Where(t => !t.IsAbstract && t.GetCustomAttribute<CsvImportableAttribute>() != null).ToList();
        _typeNames = _types.Select(t => t.Name).ToArray();
    }

    private void OnGUI() {
        DrawCsvSelection();
        DrawTypeSelection();
        DrawFolderSelection();
        DrawImportButton();
    }

    private void DrawCsvSelection() {
        EditorGUILayout.LabelField("CSV File",EditorStyles.boldLabel);

        if(GUILayout.Button("Select CSV")) {
            _csvPath = EditorUtility.OpenFilePanel("Select CSV",Application.dataPath,"csv");

            if(!string.IsNullOrEmpty(_csvPath)) {
                _csvText = File.ReadAllText(_csvPath);
            }
        }

        if(!string.IsNullOrEmpty(_csvPath)) {
            EditorGUILayout.LabelField(_csvPath);
        }
    }

    private void DrawTypeSelection() {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Target Type",EditorStyles.boldLabel);

        _selectedIndex = EditorGUILayout.Popup(_selectedIndex,_typeNames);
    }

    private void DrawFolderSelection() {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Output Folder",EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(_folderPath);

        if(GUILayout.Button("Select",GUILayout.Width(80))) {
            string abs = EditorUtility.OpenFolderPanel("Select Folder",Application.dataPath,"");

            if(!string.IsNullOrEmpty(abs) && abs.StartsWith(Application.dataPath)) {
                _folderPath = "Assets" + abs.Substring(Application.dataPath.Length);
            }
            else {
                Debug.LogError("Folder must be inside Assets/");
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawImportButton() {
        EditorGUILayout.Space();

        GUI.enabled = !string.IsNullOrEmpty(_csvText);

        if(GUILayout.Button("Import")) {
            Import();
        }

        GUI.enabled = true;
    }


    private void Import() {
        var type = _types[_selectedIndex];

        CSVImporter.ImportAssetList(_csvText,_folderPath,type);

    }

}