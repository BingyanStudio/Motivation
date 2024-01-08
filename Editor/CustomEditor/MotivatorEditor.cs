using UnityEngine;
using UnityEditor;

namespace Motivation.Editor
{
    [CustomEditor(typeof(Motivator))]
    public class MotivatorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isEditor && Application.isPlaying)
            {
                var host = target as Motivator;

                EditorGUILayout.Space();
                if (GUILayout.Button("重新加载模块", GUILayout.Height(30)))
                    host.LoadAllModules();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("状态信息(只读)", EditorStyles.boldLabel);
                EditorGUILayout.Vector2Field("速度", host.Velocity);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("状态码");
                var states = MotivationSetting.instance.States;
                for (int i = 0; i < 32; i++)
                {
                    if (!states[i].Enabled) continue;
                    EditorGUILayout.Toggle(states[i].Name, host.MatchAny(1u << i));
                }
            }
        }
    }
}