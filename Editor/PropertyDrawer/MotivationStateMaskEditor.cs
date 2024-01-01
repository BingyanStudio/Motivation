using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUI;
using Bingyan.Editor;

namespace Motivation.Editor
{
    /// <summary>
    /// 绘制状态码的属性绘制器
    /// </summary>
    [CustomPropertyDrawer(typeof(MotivationStateMask))]
    public class MotivationStateMaskEditor : LinedPropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            var req = property.FindPropertyRelative("requirements");
            req.arraySize = 32;
            LabelField(pos, "启用时状态", EditorStyles.boldLabel);
            Next();

            var states = MotivationSetting.instance.States;
            for (int i = 0; i < 32; i++)
            {
                var item = req.GetArrayElementAtIndex(i);
                if (!states[i].Enabled)
                {
                    item.enumValueIndex = 0;
                    continue;
                }
                LabelField(GetLeftRect(), states[i].Name);
                item.enumValueIndex = (int)(MaskRequirement)EnumPopup(GetRightRect(), (MaskRequirement)item.enumValueIndex);
                Next();
            }
        }

        private Rect GetLeftRect() => new Rect(pos.position, new Vector2(pos.width / 2, pos.height));
        private Rect GetRightRect() => new Rect(pos.position + Vector2.right * pos.width / 2, new Vector2(pos.width / 2, pos.height));

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (lineHeight + SPACING)
                    * (1 + MotivationSetting.instance.EnabledCount);
        }
    }
}