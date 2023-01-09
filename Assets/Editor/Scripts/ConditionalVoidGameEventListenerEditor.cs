using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(ConditionalVoidGameEventListener))]
    public class ConditionalVoidGameEventListenerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = target as ConditionalVoidGameEventListener;
            
            EditorGUILayout.LabelField("Change Variable Type");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Bool"))
            {
                script.SetCondition(new BoolStoryVariableChange());
            }
            if (GUILayout.Button("Int"))
            {
                script.SetCondition(new IntStoryVariableChange());
            }
            if (GUILayout.Button("String"))
            {
                script.SetCondition(new StringStoryVariableChange());
            }
            GUILayout.EndHorizontal();

        }
    }
}
