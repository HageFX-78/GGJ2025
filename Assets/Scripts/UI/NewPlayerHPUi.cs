using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;

public class NewPlayerHPUi : MonoBehaviour
{
    private float maxHp;
    public RectTransform innerHPRect;
    public RectTransform outerHPRect;
    void Start()
    {
        // Tween rect so it's like a jelly hearbeat
        outerHPRect.DOScale(new Vector3(1.05f, 0.9f, 1.0f), 2f).SetLoops(-1, LoopType.Yoyo);
    }

    void SetPlayerHp(float hp)
    {
        innerHPRect.localScale = Vector3.one * (hp / maxHp);
    }
}
