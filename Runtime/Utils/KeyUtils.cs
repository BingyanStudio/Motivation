using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Motivation
{
    public static class KeyUtils
    {
        public static Key KeyCodeToKey(KeyCode keyCode)
        {
            if (keycodeToKeyMap.TryGetValue(keyCode, out var val)) return val;
            throw new Exception($"暂不支持 {keyCode} 转为 Key枚举，请提交 Issue!");
        }

        public static KeyCode KeyToKeyCode(Key key)
        {
            if (keyToKeyCodeMap.TryGetValue(key, out var val)) return val;
            throw new Exception($"暂不支持 {key} 转为 KeyCode枚举，请提交 Issue!");
        }

        private static Dictionary<KeyCode, Key> keycodeToKeyMap = new()
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
        };

        private static Dictionary<Key, KeyCode> keyToKeyCodeMap = new()
        {
            { Key.A , KeyCode.A },
            { Key.B , KeyCode.B },
            { Key.C , KeyCode.C },
            { Key.D , KeyCode.D },
            { Key.E , KeyCode.E },
            { Key.F , KeyCode.F },
            { Key.G , KeyCode.G },
            { Key.H , KeyCode.H },
            { Key.I , KeyCode.I },
            { Key.J , KeyCode.J },
            { Key.K , KeyCode.K },
            { Key.L , KeyCode.L },
            { Key.M , KeyCode.M },
            { Key.N , KeyCode.N },
            { Key.O , KeyCode.O },
            { Key.P , KeyCode.P },
            { Key.Q , KeyCode.Q },
            { Key.R , KeyCode.R },
            { Key.S , KeyCode.S },
            { Key.T , KeyCode.T },
            { Key.U , KeyCode.U },
            { Key.V , KeyCode.V },
            { Key.W , KeyCode.W },
            { Key.X , KeyCode.X },
            { Key.Y , KeyCode.Y },
            { Key.Z , KeyCode.Z },
            { Key.Enter , KeyCode.Return },
            { Key.Space , KeyCode.Space },
            { Key.Backspace , KeyCode.Backspace },
        };
    }
}