using UnityEngine;
using System.Collections.Generic;

namespace Motivation
{
    /// <summary>
    /// 使用字典存储键盘映射的 KeyMap
    /// </summary>
    public abstract class DictionaryKeyMap : KeyMap
    {
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
}