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
        public const string FULL_PATH = "Assets/Plugins/Bingyan/Motivation/Resources/Config.asset";
        public const string RES_PATH = "Config";

        /// <summary>
        /// 获取各个位数的状态
        /// </summary>
        public List<MotivationBitState> States => bitStates;

        /// <summary>
        /// 获取已经启用的位数的状态
        /// </summary>
        public List<MotivationBitState> EnabledStates => bitStates.FindAll(i => i.Enabled);

        /// <summary>
        /// 获取启用的位数的数量
        /// </summary>
        public int EnabledCount => bitStates.Count(i => i.Enabled);
        [SerializeField, HideInInspector] private List<MotivationBitState> bitStates = new List<MotivationBitState>();

        /// <summary>
        /// 获取对应的 SerializedObject
        /// </summary>
        public SerializedObject GetSerializedObject() => new SerializedObject(this);


        private void OnDisable()
        {
            Save(true);
        }
    }

    [Serializable]
    public class MotivationBitState
    {
        public string Name => name;
        [SerializeField] private string name;

        public bool Enabled => enabled;
        [SerializeField] private bool enabled = false;
    }
}