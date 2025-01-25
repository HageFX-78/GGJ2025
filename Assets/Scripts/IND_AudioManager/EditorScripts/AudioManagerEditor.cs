
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;
using System;
using System.Text;

[System.Serializable]
public class AudioGroupWrapper
{
    public string groupName;
    [NonReorderable]
    public List<Sound> groupAudio = new List<Sound>();
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    bool shouldRemoveOldAudio= false;
    bool showAllAudio;
    private string searchQuery = "";

    private Texture2D logoTexture;
    private bool logoLoaded = false; // Flag to check if the logo is loaded

    // Cool background color
    Color searchSectionBackgroundColor = new Color(0.12f, 0.12f, 0.12f, 1f);
    Color oldColor;


    private const string SearchQueryKey = "AudioManager_SearchQuery";
    private const string ShowAllAudioKey = "AudioManager_ShowAllAudio";
#region InspecterGUI
    public override void OnInspectorGUI()
    {
        
        AudioManager am = (AudioManager)target;
        oldColor  = GUI.backgroundColor;

        // Load stored values from EditorPrefs
        searchQuery = EditorPrefs.GetString(SearchQueryKey, "");
        showAllAudio = EditorPrefs.GetBool(ShowAllAudioKey, false);

        DrawLogoSection(am);
        DrawSearchBar(am);

        DrawButtons(am);
        DrawLineSplit();
   
#region Default Inspector
        EditorGUILayout.HelpBox("For setting slider references, try using AudioManager.AssignSliderReferences() instead of drag and drop here.", MessageType.Info);
        // Draw the default inspector
        DrawDefaultInspector();
#endregion
    }
#endregion


#region UIFunctions
#region DrawLogo
    public void DrawLogoSection(AudioManager am)
    {

        // Only load the logo once
        if (!logoLoaded)
        {
            string scriptPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)));
            string logoPath = Path.Combine(scriptPath, "funnyLogo.png"); 

            if (File.Exists(logoPath))
            {
                byte[] fileData = File.ReadAllBytes(logoPath);
                logoTexture = new Texture2D(2, 2);
                logoTexture.LoadImage(fileData);
                logoLoaded = true; 
            }
            else
            {
                EditorGUILayout.LabelField("Logo not found!", EditorStyles.boldLabel);
            }
        }

        // Display the logo if it has been loaded
        if (logoTexture != null)
        {
            float aspectRatio = (float)logoTexture.height / logoTexture.width;
            float logoHeight = EditorGUIUtility.currentViewWidth * aspectRatio; // Adjust height based on inspector width
            GUILayout.Label(logoTexture, GUILayout.Width(EditorGUIUtility.currentViewWidth), GUILayout.Height(logoHeight));
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Drag MG_AudioLoader prefab into each scene instead of the Manager for safety!", MessageType.Info);
        EditorGUILayout.Space();

    }
#endregion
#region Search Field
    public void DrawSearchBar(AudioManager am)
    {
        

        EditorGUILayout.LabelField("Search Audio Files:", EditorStyles.boldLabel);
        searchQuery = EditorGUILayout.TextArea(searchQuery);
        EditorPrefs.SetString(SearchQueryKey, searchQuery); // Store the search query
        EditorGUILayout.Space();

        showAllAudio = GUILayout.Toggle(showAllAudio, "Show all when blank");
        EditorPrefs.SetBool(ShowAllAudioKey, showAllAudio); // Store the toggle state

        if (!string.IsNullOrEmpty(searchQuery) || showAllAudio)
        {
            SerializedObject serializedObject = new SerializedObject(am);
            SerializedProperty bgmList = serializedObject.FindProperty("BGM");
            SerializedProperty sfxList = serializedObject.FindProperty("SFX");

            serializedObject.Update();

            Rect bgmRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(bgmRect, searchSectionBackgroundColor);
            DrawAudioSection("BGM Audio Clips", bgmList, searchQuery);
            DrawSfxSection("SFX Audio Clips", sfxList, searchQuery);
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
    }

    private void DrawAudioSection(string label, SerializedProperty audioList, string searchQuery)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"[ {label} ]", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        for (int i = 0; i < audioList.arraySize; i++)
        {
            SerializedProperty sound = audioList.GetArrayElementAtIndex(i);
            SerializedProperty soundName = sound.FindPropertyRelative("name");

            if (soundName.stringValue.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            {
                DrawSoundProperty(sound, soundName, audioList, i);
            }
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.Space();
    }

    private void DrawSfxSection(string label, SerializedProperty sfxList, string searchQuery)
    {
        EditorGUILayout.LabelField($"[ {label} ]", EditorStyles.boldLabel);

        for (int i = 0; i < sfxList.arraySize; i++)
        {
            SerializedProperty groupWrapper = sfxList.GetArrayElementAtIndex(i);
            SerializedProperty groupName = groupWrapper.FindPropertyRelative("groupName");
            SerializedProperty groupAudioList = groupWrapper.FindPropertyRelative("groupAudio");

            bool groupMatchesSearch = groupName.stringValue.Contains(searchQuery, StringComparison.OrdinalIgnoreCase);

            if (groupMatchesSearch)
            {
                EditorGUILayout.LabelField($"[ {groupName.stringValue} ]", EditorStyles.boldLabel);
            }

            EditorGUI.indentLevel++;

            for (int j = 0; j < groupAudioList.arraySize; j++)
            {
                SerializedProperty sfxSound = groupAudioList.GetArrayElementAtIndex(j);
                SerializedProperty sfxName = sfxSound.FindPropertyRelative("name");

                if (groupMatchesSearch || sfxName.stringValue.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                {
                    DrawSoundProperty(sfxSound, sfxName, groupAudioList, j);
                }
            }

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
    }

    private void DrawSoundProperty(SerializedProperty sound, SerializedProperty soundName, SerializedProperty list, int index)
    {
        float propertyHeight = EditorGUI.GetPropertyHeight(sound, true);
        Rect fullRect = EditorGUILayout.GetControlRect(true, propertyHeight);

        Rect propertyRect = new Rect(fullRect) { width = fullRect.width - 65 };
        Rect buttonRect = new Rect(fullRect) { x = fullRect.xMax - 60, width = 60 };

        bool expanded = EditorGUI.PropertyField(propertyRect, sound, new GUIContent(soundName.stringValue), true);

        if (expanded)
        {
            EditorGUILayout.PropertyField(soundName, true);
        }

        GUI.backgroundColor = Color.red;
        if (!expanded && GUI.Button(buttonRect, "Remove"))
        {
            AudioManager.C_Debug("Removing: " + soundName.stringValue);
            list.DeleteArrayElementAtIndex(index);
        }
        GUI.backgroundColor = oldColor;
    }
#endregion
#region Buttons
    public void DrawButtons(AudioManager am)
    {
        // Get full width for flexible layout
        float fullWidth = EditorGUIUtility.currentViewWidth;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        // Button to delete all audio files
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Delete all audio", GUILayout.Width(fullWidth * 0.45f)))
        {
            am.BGM.Clear();
            am.SFX.Clear();
            PrefabUtility.SavePrefabAsset(am.gameObject);
        }

        GUILayout.FlexibleSpace();

        // Button to clear player preferences
        if (GUILayout.Button("Clear player prefs", GUILayout.Width(fullWidth * 0.45f)))
        {
            PlayerPrefs.DeleteKey(AudioManager.MasterVolumePrefKey);
            PlayerPrefs.DeleteKey(AudioManager.BGMVolumePrefKey);
            PlayerPrefs.DeleteKey(AudioManager.SFXVolumePrefKey);
            PlayerPrefs.Save();
            AudioManager.C_Debug("PlayerPrefs Cleared");
        }
        GUI.backgroundColor = oldColor;
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        // Button to refresh audio files
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Refresh Audio Files", GUILayout.Height(40)))
        {
            // Ensure necessary folders exist in the Resources path
            CreateRequiredFolders();

            if (shouldRemoveOldAudio)
            {
                am.BGM.Clear();
                am.SFX.Clear();
            }

            // Load BGM files
            LoadBGMFiles(am);

            // Load SFX files by category
            LoadSFXFiles(am);

            // Add clips directly in "Audio/SFX" folder to the "General" group
            AddGeneralSFXClips(am);

            PrefabUtility.SavePrefabAsset(am.gameObject);
        }
        GUI.backgroundColor = oldColor;

        // Toggle to remove old audio files when refreshing
        EditorGUILayout.Space();
        shouldRemoveOldAudio = GUILayout.Toggle(shouldRemoveOldAudio, "Remove old audio files when refreshing");
        EditorGUILayout.Space();

        // Button to generate EAudio file
        DrawGenerateEAudio(am);


    }
#endregion
#region Create Required Folders
    private void CreateRequiredFolders()
    {
        string[] requiredFolders = {
            "Assets/Resources",
            "Assets/Resources/Audio",
            "Assets/Resources/Audio/BGM",
            "Assets/Resources/Audio/SFX"
        };

        foreach (var folder in requiredFolders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                string parent = Path.GetDirectoryName(folder);
                string newFolder = Path.GetFileName(folder);
                AssetDatabase.CreateFolder(parent, newFolder);
                AudioManager.C_Debug($"<color=#ffc31f>AudioManager - </color>Created missing folder: {folder}");
            }
        }
    }
#endregion
#region Audio Loading
    private void LoadBGMFiles(AudioManager am)
    {
        AudioClip[] bgmClips = Resources.LoadAll<AudioClip>("Audio/BGM");

        foreach (AudioClip audioClip in bgmClips)
        {
            if (EnumHelper.TryMakeStringEnumCompatible(audioClip.name, out string sanitizedClipName))
            {
                if (am.BGM.All(s => s.name != sanitizedClipName))
                {
                    am.BGM.Add(new Sound { name = sanitizedClipName, clip = audioClip });
                }
            }
            else
            {
                AudioManager.C_Debug($"Failed to rewrite name for: {audioClip.name} - Skipping, rename the file to an easier-to-use unique name.");
            }
        }
    }

    private void LoadSFXFiles(AudioManager am)
    {
        string sfxFolderPath = "Assets/Resources/Audio/SFX";
        string[] sfxSubfolders = AssetDatabase.GetSubFolders(sfxFolderPath);

        foreach (string subfolder in sfxSubfolders)
        {
            string categoryName = Path.GetFileName(subfolder);
            AudioGroupWrapper group = am.SFX.FirstOrDefault(g => g.groupName == categoryName) ?? new AudioGroupWrapper { groupName = categoryName };

            if (!am.SFX.Contains(group))
                am.SFX.Add(group);

            AudioClip[] sfxClips = Resources.LoadAll<AudioClip>($"Audio/SFX/{categoryName}");
            AddAudioClips(sfxClips, group);
        }
    }

    private void AddGeneralSFXClips(AudioManager am)
    {
        string sfxFolderPath = "Assets/Resources/Audio/SFX";
        string[] allSfxClipPaths = AssetDatabase.FindAssets("t:AudioClip", new[] { sfxFolderPath });
        HashSet<string> subfolderPaths = new HashSet<string>(AssetDatabase.GetSubFolders(sfxFolderPath).Select(f => $"Assets/Resources/Audio/SFX/{Path.GetFileName(f)}"));

        // Check for or create the "General" group
        AudioGroupWrapper generalGroup = am.SFX.FirstOrDefault(g => g.groupName == "General") ?? new AudioGroupWrapper { groupName = "General" };
        if (!am.SFX.Contains(generalGroup))
            am.SFX.Add(generalGroup);

        foreach (string clipPath in allSfxClipPaths)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(clipPath);
            if (!subfolderPaths.Any(f => assetPath.StartsWith(f)))
            {
                AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
                string sanitizedName;
                if(EnumHelper.TryMakeStringEnumCompatible(audioClip.name, out sanitizedName))
                {
                    
                    if (audioClip != null && generalGroup.groupAudio.All(s => s.name != sanitizedName))
                    {

                        generalGroup.groupAudio.Add(new Sound { name = sanitizedName, clip = audioClip });
                    }
                }
                
            }
        }
    }

    private void AddAudioClips(AudioClip[] clips, AudioGroupWrapper group)
    {
        foreach (AudioClip clip in clips)
        {
            if (EnumHelper.TryMakeStringEnumCompatible(clip.name, out string clipName))
            {
                if (group.groupAudio.All(s => s.name != clipName))
                {
                    group.groupAudio.Add(new Sound { name = clipName, clip = clip });
                }
            }
            else
            {
                AudioManager.C_Debug($"Failed to rewrite name for: {clip.name} - Skipping, rename the file to an easier-to-use unique name.");
            }
        }
    }
#endregion
#region Generate EAudio file
    public void DrawGenerateEAudio(AudioManager am)
    {
        EditorGUILayout.HelpBox("Optional feature to generate an Enum file you can use in place of string names when playing audio. Will reload domain and is not fully tested.", MessageType.Info);
        GUI.backgroundColor = Color.cyan;
        if (GUILayout.Button("Generate New EAudio file", GUILayout.Height(40)))
        {
             GenerateNewEnumFile(am);
        }
        if (GUILayout.Button("Regenerate EAudio file", GUILayout.Height(40)))
        {
             //RegenerateEnumFile(am);
             AudioManager.C_Debug("Regenerate EAudio file is disabled for now.");
        }
        GUI.backgroundColor = oldColor;
    }

    private void GenerateNewEnumFile(AudioManager am)
    {
        // Get the current directory of the script
            string scriptPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)));
            string targetDirectory = Path.Combine(scriptPath, "_Generated");

            string enumFileContent = "\n///////////////////////\n//\tGenerated script by AudioManager, do not edit!\n///////////////////////\n\npublic enum EAudio\n{\n";
            // Add all BGM names
            foreach (Sound bgm in am.BGM)
            {
                string enumName;
                if(EnumHelper.TryMakeStringEnumCompatible(bgm.name, out enumName))
                {
                    enumFileContent += $"\t{enumName},\n";
                }
                else
                {
                    AudioManager.C_Debug("Failed to generate enum name for: " + bgm.name);
                }
            }

            // Add all SFX names
            foreach (AudioGroupWrapper group in am.SFX)
            {
                foreach (Sound sfx in group.groupAudio)
                {
                    string enumName;
                    if(EnumHelper.TryMakeStringEnumCompatible(sfx.name, out enumName))
                    {
                        enumFileContent += $"\t{enumName},\n";
                    }
                    else
                    {
                        AudioManager.C_Debug("Failed to generate enum name for: " + sfx.name);
                    }
                }
            }

            enumFileContent += "}\n";

            if (!Directory.Exists(targetDirectory)) // Change this line
            {
                Directory.CreateDirectory(targetDirectory); // Change this line
            }

            string fileName = "EAudio.cs"; // Specify the file name
            string filePath = Path.Combine(targetDirectory, fileName);

            File.WriteAllText(filePath, enumFileContent);
            AssetDatabase.Refresh();
    }
    private void RegenerateEnumFile(AudioManager am)
    {
        // Get the current directory of the script
        string scriptPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)));
        string targetDirectory = Path.Combine(scriptPath, "_Generated");

        string fileName = "EAudio.cs";
        string filePath = Path.Combine(targetDirectory, fileName);

        HashSet<string> currentEnumNames = new HashSet<string>(); // Current valid enum names
        HashSet<string> existingEnumNames = new HashSet<string>(); // Names from the existing file

        // Collect all valid names from the AudioManager
        foreach (Sound bgm in am.BGM)
        {
            if (EnumHelper.TryMakeStringEnumCompatible(bgm.name, out string enumName))
            {
                currentEnumNames.Add(enumName);
            }
        }

        foreach (AudioGroupWrapper group in am.SFX)
        {
            foreach (Sound sfx in group.groupAudio)
            {
                if (EnumHelper.TryMakeStringEnumCompatible(sfx.name, out string enumName))
                {
                    currentEnumNames.Add(enumName);
                }
            }
        }

        // Read existing enum values if the file exists
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (line.Trim().StartsWith("//") || !line.Contains(",")) continue; // Skip comments or invalid lines
                string trimmedLine = line.Trim().TrimEnd(',');
                existingEnumNames.Add(trimmedLine);
            }
        }

        // Determine which enums to keep
        HashSet<string> finalEnumNames = new HashSet<string>(currentEnumNames); // Start with valid names
        finalEnumNames.IntersectWith(existingEnumNames); // Retain only those still in use

        // Add new enum names
        foreach (string newName in currentEnumNames)
        {
            if (!existingEnumNames.Contains(newName))
            {
                finalEnumNames.Add(newName);
            }
        }

        // Generate the content for the enum file
        string enumFileContent = "\n///////////////////////\n//\tGenerated script by AudioManager, do not edit!\n///////////////////////\n\npublic enum EAudio\n{\n";

        // Start enum declaration
        enumFileContent += "public enum EAudio\n{\n";

        // Add final enums
        foreach (string name in finalEnumNames)
        {
            enumFileContent += $"\t{name},\n";
        }

        // Close the enum declaration
        enumFileContent += "}\n";

        // Ensure the directory exists
        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }

        // Write the updated file
        File.WriteAllText(filePath, enumFileContent);
        AssetDatabase.Refresh();
    }
#endregion

    public void DrawLineSplit()
    {
                // Section split line
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        Rect lineRect = EditorGUILayout.GetControlRect(false, 2); // Height of 2 for a thin line
        EditorGUI.DrawRect(lineRect, Color.grey); // Gray line color
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }
#endregion
}
#endif