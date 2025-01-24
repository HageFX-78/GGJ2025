using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(AMLoader))]
public class AMLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AMLoader amLoader = (AMLoader)target;

        EditorGUILayout.HelpBox("This script is used to load the AudioManager prefab into the scene. It is used to ensure that the AudioManager prefab is loaded into the scene at runtime.", MessageType.Info);
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("This is the AudioManager prefab that will be loaded into the scene at runtime. Do Not Edit", MessageType.Info);
        amLoader.amPrefab = (AudioManager)EditorGUILayout.ObjectField("Audio Manager Prefab", amLoader.amPrefab, typeof(AudioManager), false);

        EditorGUILayout.Space();

        GUI.backgroundColor = Color.blue;
        if (GUILayout.Button("Go to AudioManager Prefab"))  // Button to go to AudioManager prefab
        {
            AudioManager am = amLoader.amPrefab;
            if (am != null)
            {
                Selection.activeObject = am;
            }
            else
            {
                Debug.LogWarning("AudioManager prefab is not assigned.");
            }
        }
    }
}
#endif