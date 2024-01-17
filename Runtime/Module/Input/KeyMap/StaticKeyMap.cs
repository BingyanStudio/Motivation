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
        [SerializeField, Header("按键映射")] private KeyPairs keyPairs;

        public override void Init()
        {
            ApplyKeyMap(keyPairs.GetKeyMap());
        }
    }
}