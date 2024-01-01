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
                EditorGUILayout.Space();
                if (GUILayout.Button("重新加载模块"))
                    ((Motivator)target).LoadAllModules();
            }
        }
    }
}