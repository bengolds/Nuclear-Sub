using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;
using System.IO;

[CustomEditor(typeof(AssetArray))]
public class AssetArrayEditor : Editor {
    
    protected ReorderableList list;

    private void OnEnable()
    {
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("objects"),
                true, true, true, true);
        list.drawElementCallback = DrawElement;
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Assets");
        };
    }

    private string GetFullBasePath()
    {
        string path = "Assets/" + serializedObject.FindProperty("path").stringValue;
        if (path.Last() == '/' || path.Last() == ' ')
        {
            path = path.Remove(path.Length - 1);
        }
        return path;
    }
   
    private string GetFullFilePath(string shortFilePath)
    {
        return GetFullBasePath() + "/" + shortFilePath;
    }

    private string GetShortFilePath(string fullFilePath)
    {
        string fullBasePath = GetFullBasePath() + "/";
        int index = fullFilePath.IndexOf(fullBasePath);
        if (index < 0)
        {
            return string.Empty;
        }
        return fullFilePath.Substring(index + fullBasePath.Length);
    }

    protected string[] GetOptions()
    {
        string path = GetFullBasePath();
        if (path != "Assets" && !AssetDatabase.IsValidFolder(path))
        {
            return new string[]{ "Invalid path."};
        }

        var guids = AssetDatabase.FindAssets("", new string[] { path });
        return guids.Select(guid =>
        {
            return GetShortFilePath(AssetDatabase.GUIDToAssetPath(guid));
        }).ToArray();
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        string[] options = GetOptions();
        
        int currValueIndex = Array.IndexOf(options, GetShortFilePath(element.stringValue));
        if (currValueIndex < 0)
        {
            currValueIndex = 0;
        }
        int newValueIndex = EditorGUI.Popup(rect, currValueIndex, options);
        element.stringValue = GetFullFilePath(options[newValueIndex]);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
