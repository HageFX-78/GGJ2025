using UnityEngine;
using DG.Tweening;

public class EnemyEffect : MonoBehaviour
{
    [SerializeField] private GameObject _Sprite;
    [SerializeField] private GameObject _ExplodeParticle;
    [SerializeField] private GameObject _FizzParticle;
    [SerializeField] private GameObject _HitParticlePivot;
    [SerializeField] private ParticleSystem _HitParticleSystem;
    [SerializeField] private float _inflateScale = 1.2f;

    private ParticleSystem.MainModule _fizzParticleMain;
    private ParticleSystem.ShapeModule _fizzParticleShape;
    void OnEnable()
    {
        _fizzParticleMain = _FizzParticle.GetComponent<ParticleSystem>().main;
        _fizzParticleShape = _FizzParticle.GetComponent<ParticleSystem>().shape;
    }

    void OnDisable()
    {
        _FizzParticle.SetActive(false);
        _ExplodeParticle.SetActive(false);
        _HitParticleSystem.gameObject.SetActive(false);
        _Sprite.SetActive(true);
    }

#if UNITY_EDITOR
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayHitParticles(Vector3.left);
            DisplayDeathSequence();
            //SquishStretch();

        }
    }
#endif

    public void DisplayDeathSequence()
    {
        _fizzParticleMain.startSize = new ParticleSystem.MinMaxCurve(0.3f * transform.localScale.x, 0.5f * transform.localScale.x);
        _fizzParticleShape.radius = 0.5f * transform.localScale.x;
        _FizzParticle.SetActive(true);
        _Sprite.transform.DOScale(Vector3.one * _inflateScale, 1f).SetEase(Ease.OutBounce);
        _Sprite.transform.DOShakePosition(1f, 0.2f, 50, 90, false, true).OnComplete(()=>Explode(true));
    }

    // INstant explode if needed
    public void Explode(bool isBig)
    {
        EventManager.FireEvent(GameEvents.OnCallCamShake,isBig?ECameraProfile.EnemyBigPop: ECameraProfile.EnemyPop);
        _ExplodeParticle.SetActive(true);
        _Sprite.SetActive(false);

        _Sprite.transform.DORewind();
        _Sprite.transform.localScale = Vector3.one;
    }

    public void DisplayHitParticles(Vector3 hitDirection)
    {
        _HitParticlePivot.transform.rotation = Quaternion.LookRotation(hitDirection);
        _HitParticleSystem.gameObject.SetActive(true);
        _HitParticleSystem.Play();
    }

    public void SquishStretch()
    {
        _Sprite.transform.DOScale(new Vector3(1.2f, 0.8f, 1.2f), 0.1f).SetEase(Ease.InOutBounce).OnComplete(() =>
        {
            _Sprite.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InOutBounce);
        });
    }


    [ContextMenu("MergeSequence")] // Test only
    public void MergeSequence()
    {
        _Sprite.transform.DOScale(transform.localScale * _inflateScale, 1f).SetEase(Ease.InOutBounce);
    }

    /// <summary>
    /// Merge the sprite into the given position (The big boy it assimilates into)
    /// </summary>
    /// <param name="mergeToPosition"></param>
    public void MergeInto(Vector3 mergeToPosition)
    {
        SquishStretch();
        _Sprite.transform.DOMove(mergeToPosition, 0.5f).SetEase(Ease.InCubic);
    }
    public void MergeGrowSequence(Vector3 finalScale)
    {
        _Sprite.transform.DOScale(finalScale, 1f).SetEase(Ease.InOutBounce);
    }
}
