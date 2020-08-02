using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    public static class PlayerPrefsUtil
    {

        /// <summary>
        /// どのprefixに何のキーが入っているかを管理するキー
        /// </summary>
        private const string KeyDictionary = "keyDictionary";


        /// <summary>
        /// 何番目のセーブデータかが入る
        /// </summary>
        private const string KeyPrefix = "KeyPrefix";

        public static bool CheckIsEmptySlot(string keyPrefix)
        {
            return string.IsNullOrEmpty(PlayerPrefs.GetString(keyPrefix));
        }

        public static void DeleteSlot(string keyPrefix)
        {
            var keyDictionary = GetKeyDictionary();
            if (keyDictionary != null && keyDictionary.ContainsKey(keyPrefix))
            {
                foreach (var key in keyDictionary[keyPrefix])
                {
                    PlayerPrefs.DeleteKey(keyPrefix);
                }
            }
        }
        public static void DeleteAll()
        {
        PlayerPrefs.DeleteAll();
         }

        private static string GetKeyPrefix()
        {
            return PlayerPrefs.GetString(KeyPrefix);
        }

        /// <summary>
        /// 値を取得する
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>(string key)
        {
            var ret = default(T);
            var currentKey = GetKeyPrefix() + "_" + key;
            if (PlayerPrefs.HasKey(currentKey) == false)
            {
                return ret;
            }

            var value = PlayerPrefs.GetString(currentKey, null);
            if (value == null)
            {
                return ret;
            }

            ret = typeof(T).IsPrimitive
                ? (T)Convert.ChangeType(value, typeof(T))
                : JsonConvert.DeserializeObject<T>(value);

            return ret;
        }

        public static void SetKeyPrefix(string keyPrefix)
        {
            PlayerPrefs.SetString(KeyPrefix, keyPrefix);
        }

        /// <summary>
        /// 値を保存する
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetValue<T>(string key, T value)
        {
            var currentKey = GetKeyPrefix() + "_" + key;
            //保存したキーをリストに保存しておく
            SetKeyDictionary(key);

            var data = typeof(T).IsPrimitive
                ? value.ToString()
                : JsonConvert.SerializeObject(value);

        Debug.Log("Save:" + data);

        PlayerPrefs.SetString(currentKey, data);
        }

        private static Dictionary<string, List<string>> GetKeyDictionary()
        {
            var jsonData = PlayerPrefs.GetString(KeyDictionary);
            if (string.IsNullOrEmpty(jsonData))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonData);
        }

        private static void SetKeyDictionary(string value)
        {
            var key = GetKeyPrefix();
            var keyDictionary = GetKeyDictionary();
            if (keyDictionary == null)
            {
                keyDictionary = new Dictionary<string, List<string>>();
                keyDictionary.Add(key, new List<string> { value });
            }
            else
            {
                if (keyDictionary.ContainsKey(key))
                {
                    var keyName = keyDictionary[key].FirstOrDefault(data => data == value);

                    if (string.IsNullOrEmpty(keyName))
                    {
                        keyDictionary[key].Add(value);
                    }
                }
                else
                {
                    keyDictionary.Add(key, new List<string> { value });
                }
            }

            var jsonData = JsonConvert.SerializeObject(keyDictionary);
        
            PlayerPrefs.SetString(KeyDictionary, jsonData);
        }
    }



