using System;
using System.Collections.Generic;
using Bingyan;
using UnityEngine;

namespace Motivation
{
    /// <summary>
    /// 用于控制按键映射的数据类<br/>
    /// 这个类构建了一个【用户输入按键】与【控制模块需求按键】的双向映射<br/>
    /// TODO: 支持鼠标输入的映射
    /// </summary>
    public abstract class KeyMap : ScriptableObject
    {
        /// <summary>
        /// 将 【用户输入按键】 映射为 【控制模块需求按键】
        /// </summary>
        /// <param name="rawKey">用户输入</param>
        /// <returns>实际提供给控制模块的 <see cref="KeyCode"/></returns>
        public abstract KeyCode GetMappedKey(KeyCode rawKey);

        /// <summary>
        /// 将 【控制模块需求按键】 映射为 【用户输入按键】<br/>
        /// 这个方法用于告诉输入模块，究竟需要监听用户输入的哪几个按键
        /// </summary>
        /// <param name="mappedKey">控制模块需要的按键</param>
        /// <returns>用户输入的按键</returns>
        public abstract KeyCode GetRawKey(KeyCode mappedKey);

        /// <summary>
        /// 初始化回调，会在安装至 <see cref="Motivator" /> 时调用
        /// </summary>
        public virtual void Init() { }
    }

    /// <summary>
    /// 用于保存所有按键映射的模型类，用于与编辑器交互<br/>
    /// 相当于向编辑器暴露一个【字典】
    /// </summary>
    [Serializable]
    public class KeyPairs
    {
        [SerializeField, HideInInspector] private KeyPair[] keyPairs;

        /// <summary>
        /// 将当前 <see cref="KeyPairs"/> 存储的映射转换为字典
        /// </summary>
        /// <returns>键盘映射字典</returns>
        public Dictionary<KeyCode, KeyCode> GetKeyMap()
        {
            var dict = new Dictionary<KeyCode, KeyCode>();
            foreach (var item in keyPairs)
            {
                if (item.Raw == KeyCode.None || item.Mapped == KeyCode.None) continue;
                if (!dict.TryAdd(item.Raw, item.Mapped)) Debug.LogWarning($"按键 {item.Raw} 被映射到了两个不同的按键上！");
            }
            return dict;
        }
    }

    /// <summary>
    /// 用于保存按键映射的模型类，用于与编辑器交互
    /// </summary>
    [Serializable]
    public class KeyPair
    {
        [SerializeField, Title("用户输入按键")] private KeyCode raw;
        [SerializeField, Title("映射按键")] private KeyCode mapped;

        /// <summary>
        /// 用户输入的按键
        /// </summary>
        public KeyCode Raw => raw;

        /// <summary>
        /// 映射后的按键
        /// </summary>
        public KeyCode Mapped => mapped;
    }
}