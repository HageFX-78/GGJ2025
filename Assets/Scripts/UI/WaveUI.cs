using System;
using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    TextMeshProUGUI waveCountUI = null;

    private void Awake()
    {
        waveCountUI = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        waveCountUI.text = "Current Wave: 0";
        EventManager.ConnectEvent(GameEvents.OnWaveUpdate, HandleUpdateWave);
    }

    private void OnDisable()
    {
        EventManager.DisconnectEvent(GameEvents.OnWaveUpdate, HandleUpdateWave);
    }

    private void HandleUpdateWave(object obj)
    {
        waveCountUI.text = $"Current Wave: {(int)obj}";
    }
}
