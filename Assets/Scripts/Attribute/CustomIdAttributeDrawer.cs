#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System;
[CustomPropertyDrawer(typeof(CustomIdAttribute))]
public class CustomIdAttributeDrawer : PropertyDrawer
{
    private CustomIdAttribute itemIdAttribute
    {
        get
        {
            return (CustomIdAttribute)attribute;
        }
    }
    private bool isInitialized = false;
    private int[] itemIds = null;
    private string[] itemLabels = null;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //初期化
        if (!isInitialized)
        {
            CustomIdAttribute idAttribute = (CustomIdAttribute)attribute;


            Dictionary<int, string> items = GetLabels(idAttribute._dataName);


            itemIds = new int[items.Count];
            itemLabels = new string[items.Count];
            items.Keys.CopyTo(itemIds, 0);
            items.Values.CopyTo(itemLabels, 0);//nameがあるもののみ

            isInitialized = true;
        }
        property.intValue = EditorGUI.IntPopup(position, label.text, property.intValue, itemLabels, itemIds);
    }

    public static Dictionary<int, string> GetLabels(string dataName)
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        result[0] = "0";
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", dataName));
        if (guids.Length == 0)
        {
            return result;
        }
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var memo = string.Empty;

            var scriptableObject=AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            if(scriptableObject is IMasterBookData)
            {
                var data = scriptableObject as IMasterBookData;
                memo = data.Memo;
            }


                //フォルダによるしぼりこみ
                Match match = Regex.Match(assetPath, string.Format("Assets/Resources/MasterBook/{0}/(.*?).asset", dataName));
                foreach (Group g in match.Groups)
                {
                    if (g.Index == 0)
                    {
                        continue;
                    }
                    int targetId = 0;
                    if (int.TryParse(g.Value, out targetId))
                    {
                        //表示項目を"ID: アイテム名"にしている
                        result[targetId] = string.Format("{0}: {1}", targetId, memo);
                    }
                }
            }
            return result;
        }
    
}
#endif
