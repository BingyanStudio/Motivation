using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System.Linq;
using System;

namespace Motivation
{
    /// <summary>
    /// 使用 <see cref="PlayerPrefs"/> 存储键盘映射的 KeyMap
    /// </summary>
    [CreateAssetMenu(fileName = "KeyMap", menuName = "Motivation/KeyMap/PlayerPrefs")]
    public class PlayerPrefsKeyMap : DictionaryKeyMap
    {
        public const string KEY = "MotivationKeyMap";

        public event Action Modified;

        [Header("模块")]
        [SerializeField] private ControllerModule[] modules;

        private Dictionary<KeyCode, KeyCode> keymap;

        public override void Init()
        {
            base.Init();
            var savedKeymap = PlayerPrefs.GetString(KEY, "");
            if (savedKeymap.Length > 0)
            {
                var loaded = JsonMapper.ToObject<Dictionary<string, int>>(savedKeymap);
                keymap = loaded.ToDictionary(i => (KeyCode)int.Parse(i.Key), j => (KeyCode)j.Value);
            }
            else
            {
                keymap = new();
                foreach (var item in modules)
                    foreach (var key in item.GetRequiredKeys())
                        keymap.TryAdd(key, key);
                Save();
            }
            ApplyKeyMap(keymap);
        }

        public void Modify(KeyCode from, KeyCode to, Action<KeyCode, KeyCode> onOtherAffected = null)
        {
            if (!keymap.TryGetValue(from, out var fromMapped))
            {
                Debug.LogWarning($"键位映射中不包含 {from} !");
                return;
            }

            keymap.Remove(from);

            if (keymap.TryGetValue(to, out var toMapped))
            {
                keymap.Remove(to);
                keymap.Add(from, toMapped);
                onOtherAffected.Invoke(to, from);
            }

            keymap.Add(to, fromMapped);

            Modified?.Invoke();

            ApplyKeyMap(keymap);
            Save();
        }

        public void Save()
        {
            var json = JsonMapper.ToJson(keymap.ToDictionary(k => (int)k.Key, v => (int)v.Value));
            PlayerPrefs.SetString(KEY, json);
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(KEY);
            Init();
        }
    }
}