using UnityEngine;
using DG.Tweening;

public class NewPlayerHPUi : MonoBehaviour
{
    private float maxHp;
    public RectTransform innerHPRect;
    public RectTransform outerHPRect;
    void Start()
    {
        maxHp = 100;
        // Tween rect
        outerHPRect.DOScale(new Vector3(1.05f, 0.9f, 1.0f), 2f).SetLoops(-1, LoopType.Yoyo);

        EventManager.ConnectEvent(GameEvents.OnPlayerDamaged, TweenPlayerHp);

    }

    void OnDisable()
    {
        EventManager.DisconnectEvent(GameEvents.OnPlayerDamaged, TweenPlayerHp);
    }

    void TweenPlayerHp(object hp)
    {
        float hpValue = (float)hp;
        innerHPRect.localScale = new Vector3(hpValue / maxHp, hpValue / maxHp, 1.0f);
    }
}
