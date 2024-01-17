using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUI;
using Bingyan.Editor;
using System;
using Bingyan;

namespace Motivation
{
    [CustomPropertyDrawer(typeof(KeyPairs))]
    public class KeyPairsDrawer : LinedPropertyDrawer
    {
        private float horiSpacing = 10, delBtnWidth = 50, addBtnWidth = 80;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);

            var list = property.FindPropertyRelative("keyPairs");

            GUI.Box(new(pos.x - 2, pos.y + 2, pos.width + 4, 2 + (lineHeight + SPACING) * (GetListLines(list) + 2)), "");

            LabelField(GetLeft(), "用户输入");
            LabelField(GetRight(), "映射");
            Next();
            if (list.arraySize == 0)
            {
                HelpBox(new(pos.x, pos.y, pos.width, 2 * lineHeight + SPACING), "暂无按键映射", MessageType.Info);
                Next(2);
            }
            else for (int i = 0; i < list.arraySize; i++)
                {
                    var raw = list.GetArrayElementAtIndex(i).FindPropertyRelative("raw");
                    var mapped = list.GetArrayElementAtIndex(i).FindPropertyRelative("mapped");
                    raw.enumValueIndex = Popup(GetLeft(), raw.enumValueIndex, raw.enumDisplayNames);
                    mapped.enumValueIndex = Popup(GetRight(), mapped.enumValueIndex, mapped.enumDisplayNames);

                    if (GUI.Button(new(pos.x + pos.width - delBtnWidth, pos.y, delBtnWidth, pos.height), "删除"))
                        if (DialogUtils.Show("确认删除", $"你确定要删除从 {raw.enumDisplayNames[raw.enumValueIndex]} 到 {mapped.enumDisplayNames[mapped.enumValueIndex]} 的映射吗？", isErr: false))
                            list.DeleteArrayElementAtIndex(i);

                    Next();
                }
            if (GUI.Button(new(pos.x, pos.y, addBtnWidth, pos.height), "添加"))
                list.arraySize++;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var list = property.FindPropertyRelative("keyPairs");

            var height = lineHeight + SPACING;
            return height * (GetListLines(list) + 2);
        }

        private int GetListLines(SerializedProperty list) => list.arraySize == 0 ? 2 : list.arraySize;

        private Rect GetLeft()
            => new(pos.position, Vector2.right * (pos.width - delBtnWidth - 2 * horiSpacing) / 2 + Vector2.up * pos.height);

        private Rect GetRight()
            => new(pos.position + Vector2.right * (pos.width - delBtnWidth) / 2, Vector2.right * (pos.width - delBtnWidth - 2 * horiSpacing) / 2 + Vector2.up * pos.height);
    }
}