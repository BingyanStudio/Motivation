using UnityEngine;

namespace Motivation
{
    /// <summary>
    /// 使用 <see cref="UnityEditor.EditorPrefs"/> 存储键位的 <see cref="KeyMap"/><br/>
    /// 非常适合用于多人协作时使用，让每个人在编辑器内都能使用自己习惯的键位进行操作<br/>
    /// 【不可以】打包进发行版中
    /// </summary>
    [CreateAssetMenu(fileName = "KeyMap", menuName = "Motivation/KeyMap/Editor")]
    public class EditorKeyMap : StaticKeyMap
    {
        // 用于获取当前控制模块所需的按键的工具属性，仅应当被编辑器拓展调用
        [SerializeField, HideInInspector] private ControllerModule[] controllers;
    }
}