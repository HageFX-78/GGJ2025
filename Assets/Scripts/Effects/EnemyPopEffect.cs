using UnityEngine;
using DG.Tweening;

public class EnemyPopEffect : MonoBehaviour
{
    [SerializeField] private GameObject _Sprite;
    [SerializeField] private GameObject _ExplodeParticle;
    [SerializeField] private float _inflateScale = 1.2f;
    void Start()
    {
        DeathSequence();
    }

    public void DeathSequence()
    {
        _Sprite.transform.DOScale(transform.localScale * _inflateScale, 1f).SetEase(Ease.OutBounce);
        _Sprite.transform.DOShakePosition(1f, 0.2f, 50, 90, false, true).OnComplete(Explode);
    }
    void Explode()
    {
        EventManager.FireEvent(GameEvents.OnCallCamShake, ECameraProfile.EnemyPop);
        _ExplodeParticle.SetActive(true);
        _Sprite.SetActive(false);

        _Sprite.transform.DORewind();
        _Sprite.transform.localScale = Vector3.one;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
