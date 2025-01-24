using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// My audio manager for game jams, Is it optimized? No. Is it easy to use? Probably.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Header("UI Reference (Optional)")]
    [SerializeField] GameObject audioControlPanel;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Button resetButton;
    [Header("Audio Mixer Reference")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerGroup masterGroup;
    [SerializeField] AudioMixerGroup bgmGroup;
    [SerializeField] AudioMixerGroup sfxGroup;

    [HideInInspector] public const string MasterVolumePrefKey = "MasterVolume";
    [HideInInspector] public const string BGMVolumePrefKey = "BGMVolume";
    [HideInInspector] public const string SFXVolumePrefKey = "SFXVolume";

    private const string masterMix = "Master_Mixer";
    private const string bgmMix = "BGM_Mixer";
    private const string sfxMix = "SFX_Mixer";

    [Header("Settings")]
    [SerializeField] int sfxPoolSize = 20;
    [SerializeField] int audioClampMaxSteps = 10;


    [Header("Audio Lists")]
    [NonReorderable]
    public List<Sound> BGM = new List<Sound>();
    [NonReorderable]
    public List<AudioGroupWrapper> SFX = new List<AudioGroupWrapper>();

    private AudioSource bgmSource;
    private Queue<AudioSource> sfxQueue = new Queue<AudioSource>();
    private Dictionary<string, Sound> audioDictionary = new Dictionary<string, Sound>();

    public static AudioManager instance;


#region Monobehaviour Override

    void OnValidate()
    {
        // Hide transform so less bloat
        transform.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
    }
    void Awake()
    {
        // Classic singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);
        


        //Create a dictionary for easy access
        foreach (Sound s in BGM)
        {
            audioDictionary.Add(s.name, s);
        }
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.outputAudioMixerGroup = bgmGroup;

        // SFX Object Pool
        foreach (AudioGroupWrapper group in SFX)
        {
            foreach (Sound s in group.groupAudio)
            {
                audioDictionary.Add(s.name, s);
            }
        }
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = sfxGroup;
            sfxQueue.Enqueue(sfxSource);
        }

        _PrefCheck();
        
    }
    void Start()
    {
        _UpdateMixer(); // Must be in start to ensure that the audio mixer/player prefs is loaded
        AssignSliderReferences(masterSlider, bgmSlider, sfxSlider);
        AssignResetButton(resetButton);
    }

#endregion
#region Volume
    public static void ResetAllVolume()
    {
        if(InstanceIsNull()) return;
        SetMasterVolume(1.0f);
        SetBGMVolume(1.0f);
        SetSFXVolume(1.0f);

        _UpdateSlidersUI(1.0f);
    }

    private static void _PrefCheck()
    {
        if(InstanceIsNull()) return;
        if (!PlayerPrefs.HasKey(MasterVolumePrefKey))
        {
            PlayerPrefs.SetFloat(MasterVolumePrefKey, 1.0f);
        }
        if (!PlayerPrefs.HasKey(BGMVolumePrefKey))
        {
            PlayerPrefs.SetFloat(BGMVolumePrefKey, 1.0f);
        }
        if (!PlayerPrefs.HasKey(SFXVolumePrefKey))
        {
            PlayerPrefs.SetFloat(SFXVolumePrefKey, 1.0f);
        }

        PlayerPrefs.Save();
    }

    private static void _UpdateMixer()
    {
        if(InstanceIsNull()) return;
        instance.audioMixer.SetFloat(masterMix, PlayerPrefs.GetFloat(MasterVolumePrefKey) == 0? -80f:ConvertToDecibel(PlayerPrefs.GetFloat(MasterVolumePrefKey)));
        instance.audioMixer.SetFloat(bgmMix, PlayerPrefs.GetFloat(BGMVolumePrefKey) == 0? -80f:ConvertToDecibel(PlayerPrefs.GetFloat(BGMVolumePrefKey)));
        instance.audioMixer.SetFloat(sfxMix, PlayerPrefs.GetFloat(SFXVolumePrefKey) == 0? -80f:ConvertToDecibel(PlayerPrefs.GetFloat(SFXVolumePrefKey)));
    }
#endregion
#region  Getter Setters
    // Public setters if needed
    public static void SetMasterVolume(float volume, bool clampToStep = false)
    {
        if(InstanceIsNull()) return;
        if(clampToStep)
            volume = ClampToSteps(volume);
        PlayerPrefs.SetFloat(MasterVolumePrefKey, volume);
        instance.audioMixer.SetFloat(masterMix, volume == 0? -80f:ConvertToDecibel(volume));
    }
    public static void SetBGMVolume(float volume, bool clampToStep = false)
    {
        if(InstanceIsNull()) return;
        if(clampToStep)
            volume = ClampToSteps(volume);
        PlayerPrefs.SetFloat(BGMVolumePrefKey, volume);
        instance.audioMixer.SetFloat(bgmMix, volume == 0? -80f:ConvertToDecibel(volume));
    }
    public static void SetSFXVolume(float volume, bool clampToStep = false)
    {
        if(InstanceIsNull()) return;
        if(clampToStep)
            volume = ClampToSteps(volume);
        PlayerPrefs.SetFloat(SFXVolumePrefKey, volume);
        instance.audioMixer.SetFloat(sfxMix, volume == 0? -80f:ConvertToDecibel(volume));
    }
    // Public getters if needed
    public static float GetMasterVolume()
    {
        if(InstanceIsNull()) return 0f;
        return PlayerPrefs.GetFloat(MasterVolumePrefKey);
    }
    public static float GetBGMVolume()
    {
        if(InstanceIsNull()) return 0f;
        return PlayerPrefs.GetFloat(BGMVolumePrefKey);
    }
    public static float GetSFXVolume()
    {
        if(InstanceIsNull()) return 0f;
        return PlayerPrefs.GetFloat(SFXVolumePrefKey);
    }

    // Get SFX group for footsteps seetup
    public static AudioMixerGroup GetSFXGroup()
    {
        if(InstanceIsNull()) return null;
        return instance.sfxGroup;
    }

    public static AudioClip GetAudioClip(string name)
    {
        if(InstanceIsNull()) return null;
        if (!instance.audioDictionary.ContainsKey(name))
        {
            C_Debug("Audio not found in dictionary");
            return null;
        }
        return instance.audioDictionary[name].clip;
    }
    public static AudioClip GetAudioClip(AEnum enumName)
    {
        if(InstanceIsNull()) return null;
        if (!instance.audioDictionary.ContainsKey(enumName.ToString()))
        {
            C_Debug("Audio not found in dictionary");
            return null;
        }
        return instance.audioDictionary[enumName.ToString()].clip;
    }
#endregion
#region UI Settings
    /// <summary>
    /// Update UI based on vale
    /// </summary>
    /// <param name="val"></param>
    private static void _UpdateSlidersUI(float val = -1f)
    {
        if(InstanceIsNull()) return;

        
        //Defaults to -1 if val is <=0, meaning use player prefs value
        if(instance.masterSlider!=null)
        {
            instance.masterSlider.value = val<=0?GetMasterVolume():val;
        }
        if(instance.bgmSlider!=null)
        {
            instance.bgmSlider.value = val<=0?GetBGMVolume():val;
        }
        if(instance.sfxSlider!=null)
        {
            instance.sfxSlider.value = val<=0?GetSFXVolume():val;
        }
    }

    

    /// <summary>
    /// IMPORTANT: CALL THIS FUNCTION TO REASSIGN SLIDER REFERENCES WHEN SCENE CHANGES
    /// </summary>
    /// <param name="mSlider"></param>
    /// <param name="bSlider"></param>
    /// <param name="sSlider"></param>
    public static void AssignSliderReferences(Slider mSlider, Slider bSlider, Slider sSlider)
    {
        if(InstanceIsNull()) return;
        instance.masterSlider = mSlider;
        instance.bgmSlider = bSlider;
        instance.sfxSlider = sSlider;

        _UpdateSlidersUI();

        mSlider.onValueChanged.AddListener(delegate { SetMasterVolume(mSlider.value, true); });
        bSlider.onValueChanged.AddListener(delegate { SetBGMVolume(bSlider.value, true); });
        sSlider.onValueChanged.AddListener(delegate { SetSFXVolume(sSlider.value, true); });

        mSlider.onValueChanged.AddListener(val => mSlider.value = ClampToSteps(val));
        bSlider.onValueChanged.AddListener(val => bSlider.value = ClampToSteps(val));
        sSlider.onValueChanged.AddListener(val => sSlider.value = ClampToSteps(val));
    }
    private static float ClampToSteps(float value)
    {
        // Clamps value to 10 steps between 1 and 2 (or based on `audioClampMaxSteps`)
        float stepSize = 2.0f / instance.audioClampMaxSteps;
        return Mathf.Round(value / stepSize) * stepSize;
    }
    public static void AssignResetButton(Button button)
    {
        if(InstanceIsNull()) return;
        if (instance.resetButton == null) return;
        button.onClick.RemoveAllListeners();
        instance.resetButton = button;
        button.onClick.AddListener(delegate { ResetAllVolume(); });
    }

    public static void ToggleAudioControlPanel(bool enable)
    {
        if(InstanceIsNull()) return;
        if(instance.audioControlPanel!=null)
            instance.audioControlPanel.SetActive(enable);
    }
#endregion

#region Play BGM
    public static AudioSource PlayBGM(string name)
    {
        if(InstanceIsNull()) return null;
        if (!instance.audioDictionary.ContainsKey(name))
        {
            C_Debug("Audio not found in dictionary");
            return null;
        }

        instance.bgmSource.clip = instance.audioDictionary[name].clip;
        instance.bgmSource.volume = instance.audioDictionary[name].volume;
        instance.bgmSource.pitch = instance.audioDictionary[name].pitch;
        instance.bgmSource.Play();

        return instance.bgmSource;
        
    }
    public static AudioSource PlayBGM(AEnum enumName)
    {
        return PlayBGM(enumName.ToString());
    }

    public static void PauseBGM()
    {
        if(InstanceIsNull()) return;
        instance.bgmSource.Pause();
    }
    public static void ResumeBGM()
    {
        if(InstanceIsNull()) return;
        instance.bgmSource.UnPause();
    }
#endregion
#region Play SFX
    public static AudioSource PlaySFX(string name)
    {
        return _PlaySFXWithParams(name);
    }
    public static AudioSource PlaySFX(AEnum enumName)
    {
        return PlaySFX(enumName.ToString());
    }


    public static AudioSource PlaySFXPitchVaried(string name, float pitchRange = 0.1f)
    {
        return _PlaySFXWithParams(name, pitchRange);
    }
    public static AudioSource PlaySFXPitchVaried(AEnum enumName, float pitchRange = 0.1f)
    {
        return PlaySFXPitchVaried(enumName.ToString(), pitchRange);
    }

    private static AudioSource _PlaySFXWithParams(string name, float pitchRange = 0f)
    {
        if(InstanceIsNull()) return null;
        if(instance.sfxQueue.Count == 0)
        {
            C_Debug("No available audio source in pool");
            return null;
        }
        if (!instance.audioDictionary.ContainsKey(name))
        {
            C_Debug("Audio not found in dictionary");
            return null;
        }

        AudioSource sfxSource = instance.sfxQueue.Dequeue();

        // Settings and stuff
        sfxSource.enabled = true;
        sfxSource.clip = instance.audioDictionary[name].clip;
        sfxSource.volume = instance.audioDictionary[name].volume;
        float pitch = instance.audioDictionary[name].pitch;
        sfxSource.pitch = UnityEngine.Random.Range(pitch - pitchRange, pitch + pitchRange);

        // Play and start timer
        sfxSource.Play();
        instance.StartCoroutine(instance._WaitForAudioToFinish(sfxSource));

        return sfxSource;
    }

    private IEnumerator _WaitForAudioToFinish(AudioSource source)
    {
        while(source.isPlaying)
        {
            yield return null;
        }
        source.enabled = false;
        instance.sfxQueue.Enqueue(source);
    }
    public static void StopBGM()
    {
        if(InstanceIsNull()) return;
        instance.bgmSource.Stop();
    }
    public static void StopAllSFX()
    {
        if(InstanceIsNull()) return;
        foreach (AudioSource sfxSource in instance.sfxQueue)
        {
            sfxSource.Stop();
            sfxSource.enabled = false;
        }
    }
#endregion PlayStop
#region Helper
    public static void C_Debug(string message)
    {
        Debug.Log("<color=#ffc31f>AudioManager - </color>" + message);
    }
    public static bool InstanceIsNull()
    {
        if (instance == null)
        {
            C_Debug("AudioManager instance is null");
            return true;
        }
        return false;
    }

    public static float ConvertToDecibel(float linear)
    {
        return Mathf.Log10(linear) * 20;
    }
    public static float ConvertToLinear(float decibel)
    {
        return Mathf.Pow(10, decibel / 20);
    }

#endregion 
    
}
