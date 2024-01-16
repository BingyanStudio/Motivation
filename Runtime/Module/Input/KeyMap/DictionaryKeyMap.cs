using UnityEngine;
using System.Collections.Generic;

namespace Motivation
{
    public abstract class DictionaryKeyMap : KeyMap
    {
        private Dictionary<KeyCode, KeyCode> rawToMapped, mappedToRaw;

        /// <summary>
        /// 将保存的按键映射应用于当前 KeyMap 上。
        /// </summary>
        /// <param name="rawToMappedDict">保存的映射，以键值对的方式传递</param>
        public virtual void ApplyKeyMap(IDictionary<KeyCode, KeyCode> rawToMappedDict)
        {
            rawToMapped = new(rawToMappedDict);
            mappedToRaw = new();
            foreach (var item in rawToMappedDict)
            {
                if (item.Key == KeyCode.None || item.Value == KeyCode.None) rawToMapped.Remove(item.Value);
                else if (!mappedToRaw.TryAdd(item.Value, item.Key))
                {
                    rawToMapped.Remove(item.Key);
                    Debug.LogWarning("有多个按键映射到了同一个按键上！");
                }
            }
        }

        public override KeyCode GetMappedKey(KeyCode rawKey)
            => rawToMapped.TryGetValue(rawKey, out var result) ? result : rawKey;

        public override KeyCode GetRawKey(KeyCode mappedKey)
            => mappedToRaw.TryGetValue(mappedKey, out var result) ? result : mappedKey;
    }
}