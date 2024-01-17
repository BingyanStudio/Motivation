using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUILayout;
using Bingyan;
using LitJson;
using System.Collections.Generic;
using System;

namespace Motivation.Editor
{
    /// <summary>
    /// <see cref="EditorKeyMap"/> 的编辑器<br/>
    /// 与 <see cref="EditorPrefs"/> 直接交互即发生在这里
    /// </summary>
    [CustomEditor(typeof(EditorKeyMap))]
    public class EditorKeyMapEditor : UnityEditor.Editor
    {
        public const string EDITORPREFS_NAME = "MotivatorKeyMap";

        public override void OnInspectorGUI()
        {
            var keyPairs = serializedObject.FindProperty("keyPairs").FindPropertyRelative("keyPairs");

            // 从 EditorPrefs 读取
            var json = EditorPrefs.GetString(EDITORPREFS_NAME, "");
            (int, int)[] list = json.Length > 0 ? JsonMapper.ToObject<(int, int)[]>(json) : new (int, int)[0];
            keyPairs.arraySize = list.Length;
            for (int i = 0; i < list.Length; i++)
            {
                var item = keyPairs.GetArrayElementAtIndex(i);
                item.FindPropertyRelative("raw").enumValueIndex = list[i].Item1;
                item.FindPropertyRelative("mapped").enumValueIndex = list[i].Item2;
            }
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
            Space(40);
            LabelField("快速生成", EditorStyles.boldLabel);

            var controllers = serializedObject.FindProperty("controllers");
            PropertyField(controllers);

            if (GUILayout.Button("生成", GUILayout.Height(25))
                && DialogUtils.Show("确认生成", "你确定要生成键位吗？这会抹除目前设定的部分键位", isErr: false))
            {
                var requiredKeys = new List<KeyCode>();
                for (int i = 0; i < controllers.arraySize; i++)
                {
                    var module = controllers.GetArrayElementAtIndex(i).objectReferenceValue as ControllerModule;
                    if (!module) continue;
                    foreach (var item in module.GetRequiredKeys())
                        if (!requiredKeys.Contains(item)) requiredKeys.Add(item);
                }

                for (int i = keyPairs.arraySize - 1; i >= 0; i--)
                {
                    var item = keyPairs.GetArrayElementAtIndex(i).FindPropertyRelative("mapped");
                    var itemKeyCode = (KeyCode)Enum.Parse(typeof(KeyCode), item.enumNames[item.enumValueIndex]);

                    // 如果出现了不需要的，就删除
                    if (!requiredKeys.Contains(itemKeyCode)) keyPairs.DeleteArrayElementAtIndex(i);

                    // 反之，则是先前已经设定过一遍了，可以复用
                    else requiredKeys.Remove(itemKeyCode);
                }

                // 如果最后还有 required 没有被添加，则直接添加
                if (requiredKeys.Count > 0)
                {
                    keyPairs.arraySize += requiredKeys.Count;
                    for (int i = 0; i < requiredKeys.Count; i++)
                    {
                        var item = keyPairs.GetArrayElementAtIndex(keyPairs.arraySize - requiredKeys.Count + i);
                        var mapped = item.FindPropertyRelative("mapped");
                        mapped.enumValueIndex = new List<string>(mapped.enumNames).IndexOf(requiredKeys[i].ToString());

                        item.FindPropertyRelative("raw").enumValueIndex = 0;
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();

            // 存入 EditorPref
            list = new (int, int)[keyPairs.arraySize];
            for (int i = 0; i < keyPairs.arraySize; i++)
            {
                var item = keyPairs.GetArrayElementAtIndex(i);
                list[i] = (item.FindPropertyRelative("raw").enumValueIndex, item.FindPropertyRelative("mapped").enumValueIndex);
            }
            EditorPrefs.SetString(EDITORPREFS_NAME, JsonMapper.ToJson(list));
        }
    }
}