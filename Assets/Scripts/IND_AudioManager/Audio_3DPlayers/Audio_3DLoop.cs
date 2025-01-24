using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class Audio_3DLoop : MonoBehaviour
{
    [field: SerializeField]  public AEnum movementSound {get; private set;}
    public AudioMixerGroup sfxMixerGroup;

    [field: SerializeField] public AudioSource audioSource {get; private set;}
    public void Start()
    {
        if(AudioManager.GetAudioClip(movementSound) == null)
        {
            AudioManager.C_Debug("Audio Clip is null");
            return;
        }
        audioSource.clip = AudioManager.GetAudioClip(movementSound);
        audioSource.Play();
        DisableSound();
    }

    void Reset()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1;
        audioSource.outputAudioMixerGroup = sfxMixerGroup;
        audioSource.loop = true;
    }


    public void EnableSound()
    {
        audioSource.UnPause();
    }
    public void DisableSound()
    {
        audioSource.Pause();
    }
}
