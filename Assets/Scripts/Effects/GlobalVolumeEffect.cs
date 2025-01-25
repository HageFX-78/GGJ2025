using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeEffect : MonoBehaviour
{

    private Volume v;
    private ChromaticAberration chromaticAberration;
    private Bloom bloom;
    private Vignette vignette;

    public void Start()
    {        
        EventManager.ConnectEvent(GameEvents.OnPlayerDamaged, PlayerDamaged);

        v = GetComponent<Volume>();
        v.profile.TryGet(out chromaticAberration);
        v.profile.TryGet(out bloom);
        v.profile.TryGet(out vignette);
    }
    public void OnDestroy()
    {
        EventManager.DisconnectEvent(GameEvents.OnPlayerDamaged, PlayerDamaged);
    }

    public void PlayerDamaged()
    {
        chromaticAberration.intensity.value = 1;
        bloom.tint.value = Color.red;
        vignette.color.value = Color.red;
        //Lerp back with doTween

        DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, 0, 0.5f);
        DOTween.To(() => bloom.tint.value, x => bloom.tint.value = x, Color.white, 0.5f);
        DOTween.To(() => vignette.color.value, x => vignette.color.value = x, Color.black, 0.5f);

    }
}
