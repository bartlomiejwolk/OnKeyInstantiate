using UnityEngine;
using System.Collections;
using UnityEditor;

namespace OnKeyInstantiateModule {

    [CustomEditor(typeof (OnKeyInstantiate))]
    public class OnKeyInstantiateEditor : Editor {

        /* Serialized properties */
        private SerializedProperty _breakAtInstatiate;
        private SerializedProperty _parent;
        private SerializedProperty _key;
        private SerializedProperty _keyType;
        private SerializedProperty _updateFunc;
        private SerializedProperty prefabSlots;

        private void OnEnable() {
            _breakAtInstatiate =
                serializedObject.FindProperty("_breakAtInstatiate");
            _parent = serializedObject.FindProperty("_parent");
            _key = serializedObject.FindProperty("_key");
            _keyType = serializedObject.FindProperty("_keyType");
            _updateFunc = serializedObject.FindProperty("_updateFunc");
            prefabSlots = serializedObject.FindProperty("prefabSlots");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            // Get target script
            OnKeyInstantiate script = (OnKeyInstantiate) target;

            // Create GUI layout
            //script.Key = EditorGUILayout.TextField("Key", script.Key);
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 30;
            // TODO Add tooltip that is uses string name, not KeyCode.
            EditorGUILayout.PropertyField(_key);
            EditorGUIUtility.labelWidth = 60;
            EditorGUILayout.PropertyField(_keyType);
            EditorGUIUtility.labelWidth = 0;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(
                prefabSlots,
                new GUIContent(
                    "Prefab Slots",
                    ""),
                true);

            //script.ObjectToInstantiate =
            //    (GameObject) EditorGUILayout.ObjectField(
            //        "Object to instantiate",
            //        script.ObjectToInstantiate,
            //        typeof (GameObject),
            //        true);
            //script.TargetPoint = (Transform) EditorGUILayout.ObjectField(
            //    "Target point",
            //    script.TargetPoint,
            //    typeof (Transform),
            //    true);
            //EditorGUILayout.PropertyField(_parent);
            //script.InstantiateDelay = EditorGUILayout.FloatField(
            //    "Instantiate delay",
            //    script.InstantiateDelay);

            EditorGUILayout.PropertyField(_updateFunc);
            script.AutoInstantiate = EditorGUILayout.Toggle(
                "Auto instantiate",
                script.AutoInstantiate);
            EditorGUILayout.PropertyField(_breakAtInstatiate);
            if (GUILayout.Button("Instantiate objects")) {
                script.InstantiateObjects();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
