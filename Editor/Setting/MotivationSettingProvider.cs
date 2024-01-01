using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace Motivation.Editor
{
    /// <summary>
    /// Motivation框架的设置
    /// </summary>
    public static class MotivationSettingProvider
    {
        private static MotivationSetting config;

        [SettingsProvider]
        public static SettingsProvider Configue()
            => new("Project/Bingyan/Motivation", SettingsScope.Project)
            {
                label = "Motivation",
                guiHandler = s =>
                {
                    var serConfig = MotivationSetting.instance.GetSerializedObject();
                    LabelField("状态码设置", EditorStyles.boldLabel);
                    BeginHorizontal();
                    LabelField("位数", GUILayout.Width(100));
                    LabelField("名称");
                    LabelField("启用", GUILayout.Width(100));
                    EndHorizontal();

                    var states = serConfig.FindProperty("bitStates");
                    states.arraySize = 32;
                    for (int i = 0; i < 32; i++)
                    {
                        var item = states.GetArrayElementAtIndex(i);
                        var name = item.FindPropertyRelative("name");
                        var enabled = item.FindPropertyRelative("enabled");
                        GUI.enabled = true;

                        BeginHorizontal();
                        LabelField(i.ToString(), GUILayout.Width(100));

                        if (i < 2)
                        {
                            name.stringValue = i == 0 ? "在地面上" : "在水中";
                            enabled.boolValue = true;
                            LabelField(name.stringValue);
                            GUI.enabled = false;
                        }
                        else name.stringValue = TextField(name.stringValue);
                        Space(20);
                        enabled.boolValue = Toggle(enabled.boolValue, GUILayout.Width(100));
                        EndHorizontal();
                    }

                    serConfig.ApplyModifiedProperties();
                }
            };
    }
}