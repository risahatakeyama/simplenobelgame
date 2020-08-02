using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerPrefsEditor : EditorWindow
{
    [MenuItem("Tools/PlayerPrefsEditor")]
    private static void ShowWindow()
    {
        GetWindow<PlayerPrefsEditor>();
    }
    private void OnGUI()
    {
        if (GUILayout.Button("DeleteAllPlayerPrefs"))
        {
            PlayerPrefsUtil.DeleteAll();
            Debug.Log("Complete Delete All PlaterPrefs");
        }
    }
}
