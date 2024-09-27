using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

namespace Motivation
{
    /// <summary>
    /// 使用 <see cref="PlayerPrefs"/> 存储键盘映射的 KeyMap
    /// </summary>
    [CreateAssetMenu(fileName = "KeyMap", menuName = "Motivation/KeyMap/JsonFile")]
    public class JsonFileKeyMap : SavedJsonDictionaryKeyMap
    {
        public const string FILE_NAME = "keymap.json";

        private static string filePath = null;

        [Header("模块")]
        [SerializeField] private ControllerModule[] modules;
        [Header("其他")]
        [SerializeField] private KeyCode[] otherKeys;

        protected override string GetJson()
        {
            FileStream fs;
            filePath ??= $"{Application.persistentDataPath}/{FILE_NAME}";
            if (!File.Exists(filePath)) fs = File.Create(filePath);
            else fs = File.Open(filePath, FileMode.Open, FileAccess.Read);
            var reader = new StreamReader(fs, Encoding.UTF8);
            var json = reader.ReadToEnd();
            reader.Close();
            return json;
        }

        protected override void Save(string json)
        {
            var writer = new StreamWriter(File.OpenWrite(filePath ??= $"{Application.persistentDataPath}/{FILE_NAME}"), Encoding.UTF8);
            writer.Write(json);
            writer.Close();
        }

        public override void Clear()
        {
            var fs = File.Open(filePath ??= $"{Application.persistentDataPath}/{FILE_NAME}", FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(Encoding.UTF8.GetBytes("{}"));
            fs.Close();
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