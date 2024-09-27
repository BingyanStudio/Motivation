using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System.Linq;
using System;

namespace Motivation
{
    /// <summary>
    /// 使用字典存储键盘映射的 KeyMap
    /// </summary>
    public abstract class DictionaryKeyMap : KeyMap
    {
        protected Dictionary<KeyCode, KeyCode> Current => rawToMapped;
        private Dictionary<KeyCode, KeyCode> rawToMapped, mappedToRaw;

        /// <summary>
        /// 将保存的按键映射应用于当前 <see cref="KeyMap"/> 上。
        /// </summary>
        /// <param name="rawToMappedDict">保存的映射，以键值对的方式传递</param>
        public virtual void ApplyKeyMap(IDictionary<KeyCode, KeyCode> rawToMappedDict)
        {
            rawToMapped = new(rawToMappedDict);
            mappedToRaw = new();
            foreach (var item in rawToMappedDict)
            {
                KeyCode raw = item.Key, mapped = item.Value;
                if (mapped == KeyCode.None) continue;
                if (raw == KeyCode.None) raw = mapped;
                if (!mappedToRaw.TryAdd(mapped, raw))
                {
                    rawToMapped.Remove(item.Key);
                    Debug.LogWarning("有多个按键映射到了同一个按键上！");
                }
            }
        }

        public override KeyCode GetMappedKey(KeyCode rawKey)
        {
            if (rawToMapped.TryGetValue(rawKey, out var result)) return result;
            else if (mappedToRaw.TryGetValue(rawKey, out result) && rawKey == result) return rawKey;
            return KeyCode.None;
        }

        public override KeyCode GetRawKey(KeyCode mappedKey)
            => mappedToRaw.TryGetValue(mappedKey, out var result) ? result : mappedKey;
    }

    public abstract class SavedDictionaryKeyMap : DictionaryKeyMap
    {
        public event Action Modified;

        public override void Init()
        {
            base.Init();
            ApplyKeyMap(Load());
        }

        public abstract void Clear();
        protected abstract Dictionary<KeyCode, KeyCode> Load();
        protected abstract void Save(Dictionary<KeyCode, KeyCode> dict);
        public void Save() => Save(Current);

        public void Modify(KeyCode from, KeyCode to, Action<KeyCode, KeyCode> onOtherAffected = null)
        {
            var keymap = Current;
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
                onOtherAffected?.Invoke(to, from);
            }

            keymap.Add(to, fromMapped);
            ApplyKeyMap(keymap);
            Save();
            Modified?.Invoke();
        }
    }

    public abstract class SavedJsonDictionaryKeyMap : SavedDictionaryKeyMap
    {
        protected abstract string GetJson();
        protected abstract void Save(string json);
        protected abstract HashSet<KeyCode> GetReqestedKeys();

        protected override Dictionary<KeyCode, KeyCode> Load()
        {
            Dictionary<KeyCode, KeyCode> result;
            try
            {
                var rawDict = JsonMapper.ToObject<Dictionary<string, int>>(GetJson());
                var req = GetReqestedKeys();

                if (rawDict == null) return req.ToDictionary(i => i, i => i);
                else
                {
                    var dict = rawDict.ToDictionary(i => (KeyCode)int.Parse(i.Key),
                                                    j => (KeyCode)j.Value);
                    foreach (var key in req)
                        if (!dict.ContainsValue(key))
                            dict.Add(key, key);

                    return dict;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("读取到不正确的 Json");
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
                result = GetReqestedKeys().ToDictionary(i => i, i => i);
            }
            return result;
        }

        protected override void Save(Dictionary<KeyCode, KeyCode> dict)
            => Save(JsonMapper.ToJson(dict.ToDictionary(k => (int)k.Key, v => (int)v.Value)));
    }
}