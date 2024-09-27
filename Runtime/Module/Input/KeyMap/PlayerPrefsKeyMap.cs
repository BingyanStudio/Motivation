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
    public class PlayerPrefsKeyMap : SavedJsonDictionaryKeyMap
    {
        public const string KEY = "MotivationKeyMap";

        [Header("模块")]
        [SerializeField] private ControllerModule[] modules;
        [Header("其他")]
        [SerializeField] private KeyCode[] otherKeys;

        public override void Init()
        {
            base.Init();
            Debug.Log("gg");
        }

        protected override string GetJson() => PlayerPrefs.GetString(KEY, "");
        protected override void Save(string json) => PlayerPrefs.SetString(KEY, json);

        public override void Clear()
        {
            PlayerPrefs.DeleteKey(KEY);
            Init();
        }

        protected override HashSet<KeyCode> GetReqestedKeys()
        {
            var defaultMap = new HashSet<KeyCode>();
            foreach (var item in modules)
                foreach (var key in item.GetRequiredKeys())
                    defaultMap.Add(key);
            foreach (var item in otherKeys)
                defaultMap.Add(item);
            return defaultMap;
        }
    }
}