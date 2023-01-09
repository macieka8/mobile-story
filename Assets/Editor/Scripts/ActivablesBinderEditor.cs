using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(ActivablesBinder))]
    public class ActivablesBinderEditor : Editor
    {
        [SerializeField] int _testCustomEditor;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = target as ActivablesBinder;

            EditorGUILayout.LabelField("Add Inital Activable");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("AttackData"))
            {
                script.AddInitalActivable(new AttackDataReference());
            }
            if (GUILayout.Button("ActivableItem"))
            {
                script.AddInitalActivable(new ActivableItemReference());
            }
            GUILayout.EndHorizontal();
        }
    }
}
