using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class Audio_3DOneShot : MonoBehaviour
{
    [SerializeField] private AudioClip onetimeSoundToPlay;
    public AudioMixerGroup sfxMixerGroup;

    [field: SerializeField] public AudioSource audioSource {get; private set;}

    void Reset()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1;
        audioSource.outputAudioMixerGroup = sfxMixerGroup;
        audioSource.loop = false;
    }

    /// <summary>
    /// Play the sound attached to the object !ASSIGN TO THIS SCRIPT
    /// </summary>
    public void PlaySound(bool hasPitchVariance = false)
    {
        if (onetimeSoundToPlay == null)
        {
            AudioManager.C_Debug("Audio Clip for object " + gameObject.name + " is null, assign one in the inspector");
            return;
        }
        else
        {
            audioSource.clip = onetimeSoundToPlay;
        }
        if(hasPitchVariance)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
        }
        audioSource.Play();
    }
    /// <summary>
    /// Play given sound by name
    /// </summary>
    /// <param name="soundName"></param>
    public void PlaySound(string soundName, bool hasPitchVariance = false)
    {
        AudioClip clip = AudioManager.GetAudioClip(soundName);
        if(clip == null)
        {
            AudioManager.C_Debug("Audio Clip for object " + gameObject.name + " is null");
            return;
        }
        audioSource.clip = clip;
        if(hasPitchVariance)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
        }
        audioSource.Play();
    }

    /// <summary>
    /// Play given sound by enum
    /// </summary>
    /// <param name="soundName"></param>
    public void PlaySound(EAudio soundName, bool hasPitchVariance = false)
    {
        AudioClip clip = AudioManager.GetAudioClip(soundName);
        if(clip == null)
        {
            AudioManager.C_Debug("Audio Clip for object " + gameObject.name + " is null");
            return;
        }
        audioSource.clip = clip;
        if(hasPitchVariance)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
        }
        audioSource.Play();
    }
    public void StopSound()
    {
        audioSource.Stop();
    }
}
