﻿using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

// Summary:
//      The following class will make any class that inherits from it a singleton ScriptableObject automatically
//      Furthermore it will automatically create (if necessary) and load a corresponding asset where
//       project-wide persistent settings may be specified
//
// Example Usage:
//      public class CLASSNAME : SettingsObject<CLASSNAME> { }
//
public class SettingsObject<T> : ScriptableObject where T : ScriptableObject
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
#if UNITY_EDITOR
                string settingsFile = "Resources/" + SettingsFile;
                _instance = LoadCustomAsset<T>(settingsFile);
                if (_instance == null)
                {
                    CreateAsset<T>(settingsFile);
                    _instance = LoadCustomAsset<T>(settingsFile);
                }
#else
                string settingsFile = System.IO.Path.GetFileNameWithoutExtension(SettingsFile);
                _instance = Resources.Load<T>(settingsFile);
                if (_instance == null)
                {
                    _instance = ScriptableObject.CreateInstance<T>();
                }
#endif
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }


    // The settings file is named using the type name T
    static string SettingsFile
    {
        get
        {
            string typeStr = (typeof(T)).ToString();
            typeStr = typeStr.Replace('.', ' ');
            return typeStr + ".asset";
        }
    }

#if UNITY_EDITOR
    static T1 LoadCustomAsset<T1>(string assetPath) where T1 : ScriptableObject
    {
        string assetPathAndName = "Assets/" + assetPath;

        Debug.Log(" ~~~~~~~~  LoadCustomAsset: " + assetPathAndName);

        return (T1)AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(T1));
    }

    static void CreateAsset<T1>(string assetPath) where T1 : ScriptableObject
    {
        T1 asset = CreateInstance<T1>();
        string assetPathAndName = "Assets/" + assetPath;

        Debug.Log(" ~~~~~~~~  CreateAsset: " + assetPathAndName);

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
#endif
}