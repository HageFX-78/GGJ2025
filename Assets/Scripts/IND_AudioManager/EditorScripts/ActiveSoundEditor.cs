using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Audio_3DLoop))]
public class ActiveSoundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Audio_3DLoop activeSoundInstance = (Audio_3DLoop)target;

        EditorGUILayout.HelpBox("This script is used to play sounds when the player is moving. It is used to ensure that the sounds are played when the player is moving.", MessageType.Info);

        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("Change the audio with the dropdown", MessageType.Info);

        base.OnInspectorGUI();
    }
}
[CustomEditor(typeof(Audio_3DOneShot))]
public class Audio_3DOneShotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Audio_3DOneShot oneShotSoundInstance = (Audio_3DOneShot)target;

        EditorGUILayout.HelpBox("This script is used to play one shot sounds in a 3D space for attenuation", MessageType.Info);

        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("Change the audio with the dropdown", MessageType.Info);

        base.OnInspectorGUI();
    }
}
#endif
