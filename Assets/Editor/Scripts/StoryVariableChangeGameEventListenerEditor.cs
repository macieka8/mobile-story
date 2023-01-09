using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(StoryVariableChangeGameEventListener))]
    public class StoryVariableChangeGameEventListenerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = target as StoryVariableChangeGameEventListener;
            
            EditorGUILayout.LabelField("Add Variable Change");
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Bool"))
            {
                script.AddVariableChange(new BoolStoryVariableChange());
            }
            if (GUILayout.Button("Int"))
            {
                script.AddVariableChange(new IntStoryVariableChange());
            }
            if (GUILayout.Button("String"))
            {
                script.AddVariableChange(new StringStoryVariableChange());
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("List Modifications");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Element"))
            {
                script.AddVariableChange(new ListAddElementStoryVariableChange());
            }
            GUILayout.EndHorizontal();

            DrawDefaultInspector();
        }
    }
}
