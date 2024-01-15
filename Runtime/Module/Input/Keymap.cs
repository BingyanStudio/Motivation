using UnityEngine;

namespace Motivation
{
    /// <summary>
    /// 用于控制按键映射的数据类<br/>
    /// 这个类构建了一个【用户输入按键】与【控制模块需求按键】的双向映射
    /// TODO: 支持向鼠标映射
    /// </summary>
    public abstract class KeyMap : ScriptableObject
    {
        /// <summary>
        /// 将 【用户输入按键】 映射为 【控制模块需求按键】
        /// </summary>
        /// <param name="rawKey">用户输入</param>
        /// <returns>实际提供给控制模块的 KeyCode</returns>
        public abstract KeyCode GetMappedKey(KeyCode rawKey);

        /// <summary>
        /// 将 【控制模块需求按键】 映射为 【用户输入按键】<br/>
        /// 这个方法用于告诉输入模块，究竟需要监听用户输入的哪几个按键
        /// </summary>
        /// <param name="mappedKey">控制模块需要的按键</param>
        /// <returns>用户输入的按键</returns>
        public abstract KeyCode GetRawKey(KeyCode mappedKey);
    }
}