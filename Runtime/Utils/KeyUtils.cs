using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Motivation
{
    /// <summary>
    /// 按键工具类
    /// </summary>
    public static class KeyUtils
    {
        /// <summary>
        /// 将旧输入系统的 <see cref="KeyCode"/> 转化为新输入系统的 <see cref="Key"/><br/>
        /// 目前支持所有字母，回车，空格，退格，Esc
        /// </summary>
        /// <param name="keyCode">要转化的 KeyCode</param>
        /// <returns>转化后的 Key</returns>
        public static Key KeyCodeToKey(KeyCode keyCode)
        {
            if (KeycodeToKeyMap.TryGetValue(keyCode, out var val)) return val;
            throw new Exception($"暂不支持 {keyCode} 转为 Key枚举，请提交 Issue!");
        }

        /// <summary>
        /// 将新输入系统的 <see cref="Key"/> 转化为旧输入系统的 <see cref="KeyCode"/><br/>
        /// 目前支持所有字母，回车，空格，退格，Esc
        /// </summary>
        /// <param name="key">要转化的 Key</param>
        /// <returns>转化后的 KeyCode</returns>
        public static KeyCode KeyToKeyCode(Key key)
        {
            if (KeyToKeyCodeMap.TryGetValue(key, out var val)) return val;
            throw new Exception($"暂不支持 {key} 转为 KeyCode枚举，请提交 Issue!");
        }

        public static readonly Dictionary<KeyCode, Key> KeycodeToKeyMap = new()
        {
            { KeyCode.A , Key.A },
            { KeyCode.B , Key.B },
            { KeyCode.C , Key.C },
            { KeyCode.D , Key.D },
            { KeyCode.E , Key.E },
            { KeyCode.F , Key.F },
            { KeyCode.G , Key.G },
            { KeyCode.H , Key.H },
            { KeyCode.I , Key.I },
            { KeyCode.J , Key.J },
            { KeyCode.K , Key.K },
            { KeyCode.L , Key.L },
            { KeyCode.M , Key.M },
            { KeyCode.N , Key.N },
            { KeyCode.O , Key.O },
            { KeyCode.P , Key.P },
            { KeyCode.Q , Key.Q },
            { KeyCode.R , Key.R },
            { KeyCode.S , Key.S },
            { KeyCode.T , Key.T },
            { KeyCode.U , Key.U },
            { KeyCode.V , Key.V },
            { KeyCode.W , Key.W },
            { KeyCode.X , Key.X },
            { KeyCode.Y , Key.Y },
            { KeyCode.Z , Key.Z },
            { KeyCode.Return , Key.Enter },
            { KeyCode.Space , Key.Space },
            { KeyCode.Backspace , Key.Backspace },
            { KeyCode.Escape , Key.Escape },
            { KeyCode.LeftShift , Key.LeftShift },
            { KeyCode.RightShift , Key.RightShift },
            { KeyCode.LeftAlt , Key.LeftAlt },
            { KeyCode.RightAlt , Key.RightAlt },
            { KeyCode.LeftControl , Key.LeftCtrl },
            { KeyCode.RightControl , Key.RightCtrl },
            { KeyCode.LeftCommand , Key.LeftCommand },
            { KeyCode.RightCommand , Key.RightCommand },
            { KeyCode.LeftArrow , Key.LeftArrow },
            { KeyCode.RightArrow , Key.RightArrow },
            { KeyCode.UpArrow , Key.UpArrow },
            { KeyCode.DownArrow , Key.DownArrow },
            { KeyCode.Tab , Key.Tab },
        };

        public static readonly Dictionary<Key, KeyCode> KeyToKeyCodeMap = KeycodeToKeyMap.ToDictionary(i => i.Value, j => j.Key);

        // private static Dictionary<Key, KeyCode> keyToKeyCodeMap = new()
        // {
        //     { Key.A , KeyCode.A },
        //     { Key.B , KeyCode.B },
        //     { Key.C , KeyCode.C },
        //     { Key.D , KeyCode.D },
        //     { Key.E , KeyCode.E },
        //     { Key.F , KeyCode.F },
        //     { Key.G , KeyCode.G },
        //     { Key.H , KeyCode.H },
        //     { Key.I , KeyCode.I },
        //     { Key.J , KeyCode.J },
        //     { Key.K , KeyCode.K },
        //     { Key.L , KeyCode.L },
        //     { Key.M , KeyCode.M },
        //     { Key.N , KeyCode.N },
        //     { Key.O , KeyCode.O },
        //     { Key.P , KeyCode.P },
        //     { Key.Q , KeyCode.Q },
        //     { Key.R , KeyCode.R },
        //     { Key.S , KeyCode.S },
        //     { Key.T , KeyCode.T },
        //     { Key.U , KeyCode.U },
        //     { Key.V , KeyCode.V },
        //     { Key.W , KeyCode.W },
        //     { Key.X , KeyCode.X },
        //     { Key.Y , KeyCode.Y },
        //     { Key.Z , KeyCode.Z },
        //     { Key.Enter , KeyCode.Return },
        //     { Key.Space , KeyCode.Space },
        //     { Key.Backspace , KeyCode.Backspace },
        //     { Key.Escape , KeyCode.Escape },
        // };
    }
}