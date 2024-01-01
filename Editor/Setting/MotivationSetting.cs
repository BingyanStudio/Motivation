using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Motivation.Editor
{
    /// <summary>
    /// Motivation 框架的配置文件
    /// </summary>
    [FilePath("ProjectSettings/Motivation.asset", FilePathAttribute.Location.ProjectFolder)]
    public class MotivationSetting : ScriptableSingleton<MotivationSetting>
    {
        private void OnEnable()
        {
            if (bitStates != null) return;

            var states = new List<MotivationBitState>();
            for (int i = 0; i < 32; i++)
            {
                if (i < 2) states.Add(new(i == 0 ? "在地面上" : "在水中", true));
                else states.Add(new(false));
            }
            bitStates = states;
        }

        private void OnDisable()
        {
            Save(true);
        }

        /// <summary>
        /// 获取各个位数的状态
        /// </summary>
        public List<MotivationBitState> States => bitStates;

        /// <summary>
        /// 获取启用的位数的数量
        /// </summary>
        public int EnabledCount => bitStates.Count(i => i.Enabled);
        [SerializeField, HideInInspector] private List<MotivationBitState> bitStates;

        /// <summary>
        /// 获取对应的 SerializedObject
        /// </summary>
        public SerializedObject GetSerializedObject() 
            => new SerializedObject(this);
    }

    [Serializable]
    public class MotivationBitState
    {
        public string Name => name;
        [SerializeField] private string name;

        public bool Enabled => enabled;
        [SerializeField] private bool enabled = false;

        public MotivationBitState(string name, bool enabled)
        {
            this.name = name;
            this.enabled = enabled;
        }

        public MotivationBitState(bool enabled)
        {
            this.enabled = enabled;
        }
    }
}