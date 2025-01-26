using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private List<CameraShakeProfile> _shakeProfiles = new List<CameraShakeProfile>();
    
    private Dictionary<ECameraProfile, CameraShakeProfile> _shakeProfileDict = new Dictionary<ECameraProfile, CameraShakeProfile>();

    public const float defaultShakeDuration = 0.3f;
    public const float defaultAmplitudeGain = 1.2f;
    public const float defaultFrequencyGain = 5.0f;

    private float _shakeTimer;
    private float _startingIntensity;
    private float _startingFrequency;
    private CinemachineCamera _cinemachineCam;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    public void Awake()
    {
        instance = this;

        _cinemachineCam = GetComponent<CinemachineCamera>();
        _cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>(); // Get component only in CM3.1
    }
    
    public void Start()
    {
        InitializeShakeProfileDict();
        EventManager.ConnectEvent(GameEvents.OnCallCamShake, ShakeCameraAction);
        EventManager.ConnectEvent(GameEvents.OnPlayerDeathStart, DeathCamera);
    }
    public void OnDestroy()
    {
        EventManager.DisconnectEvent(GameEvents.OnCallCamShake, ShakeCameraAction);
        EventManager.DisconnectEvent(GameEvents.OnPlayerDeathStart, DeathCamera);
    }

    private void InitializeShakeProfileDict()
    {
        foreach (CameraShakeProfile profile in _shakeProfiles)
        {
            _shakeProfileDict.Add(profile.profileID, profile);
        }
    }

    public void ShakeCameraAction(object indexRaw)
    {
        if (indexRaw == null)
        {
            ShakeCamera();
        }
        else if(_shakeProfileDict.TryGetValue((ECameraProfile)indexRaw, out CameraShakeProfile shakeProf))
        {
            StartCoroutine(Shake(shakeProf.shakeDuration, shakeProf.amplitudeGain, shakeProf.frequencyGain));
        }
    }

    [ContextMenu("Shake Camera")]
    public static void ShakeCamera()
    {
        instance.StartCoroutine(instance.Shake());
    }

    public IEnumerator Shake( float _duration = defaultShakeDuration, float _intensity = defaultAmplitudeGain, float _frequencyGain = defaultFrequencyGain)
    {
        //settings shake values
        _cinemachineBasicMultiChannelPerlin.AmplitudeGain = _intensity;
        _cinemachineBasicMultiChannelPerlin.FrequencyGain = _frequencyGain;
        
        //caching
        _shakeTimer = 0;
        _startingIntensity = _intensity;
        _startingFrequency = _frequencyGain;
        
        while (_shakeTimer < _duration)
        {
            _shakeTimer += Time.deltaTime;

            _cinemachineBasicMultiChannelPerlin.AmplitudeGain = Mathf.Lerp(_startingIntensity, 0, _shakeTimer / _duration);
            _cinemachineBasicMultiChannelPerlin.FrequencyGain = Mathf.Lerp(_startingFrequency, 0, _shakeTimer / _duration);

            if (_shakeTimer >= _duration)
            {   
                yield break;
            }
            yield return null;
        }
    }

    public void DeathCamera()
    {
        _cinemachineCam.Lens.OrthographicSize = 4;
    }
}
