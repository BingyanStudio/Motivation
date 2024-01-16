using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Bingyan;

namespace Motivation
{
    [CreateAssetMenu(fileName = "KeyMap", menuName = "Motivation/KeyMap/Static")]
    public class StaticKeyMap : DictionaryKeyMap
    {
        [SerializeField, Header("按键映射")] private List<KeyPair> keymap;

        public override void Init()
        {
            ApplyKeyMap(keymap.ToDictionary(i => i.Raw, i => i.Mapped));
        }

        [Serializable]
        protected class KeyPair
        {
            [SerializeField, Title("用户输入按键")] private KeyCode raw;
            [SerializeField, Title("映射按键")] private KeyCode mapped;

            public KeyCode Raw => raw;
            public KeyCode Mapped => mapped;
        }
    }


}