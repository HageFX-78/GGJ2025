using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] private GameObject _Sprite;
    [SerializeField] private ParticleSystem _HitParticle;
    [SerializeField] private GameObject _ChargeParticle;
    [SerializeField] private Animator _Animator;
    private Coroutine movementCoroutine;
    private bool isMoving = false;
    private Vector3 originalScale;
    void Start()
    {
        originalScale = _Sprite.transform.localScale;
        EventManager.ConnectEvent(GameEvents.OnPlayerDeathStart, Death);
    }

    void OnDisable()
    {
        EventManager.DisconnectEvent(GameEvents.OnPlayerDeathStart, Death);
    }

    void ShootEffect()
    {
        EventManager.FireEvent(GameEvents.OnCallCamShake, ECameraProfile.PlayerShoot);
    }

    Tweener SquishStretch()
    {
        return _Sprite.transform.DOScale(new Vector3(originalScale.x * 1.2f, originalScale.y * 0.8f, originalScale.z * 1.2f), 0.15f).SetEase(Ease.InOutBounce).OnComplete(() =>
        {
            _Sprite.transform.DOScale(originalScale, 0.15f).SetEase(Ease.InOutBounce);
        });
    }

    public void HitEffect()
    {
        SquishStretch();
        _HitParticle.Play();
        //Might add more here
        EventManager.FireEvent(GameEvents.OnCallCamShake, ECameraProfile.PlayerPop);
    }

    // Suishstretch when moving but return to normal when stop
    public void MovementSquishStretch(bool hasVelocity)
    {
        if(hasVelocity)
        {
            isMoving = true;
            if(movementCoroutine == null)
            {
                movementCoroutine = StartCoroutine(MovementCoroutine());
            }
        }
        else
        {
            isMoving = false;
        }
    }

    public IEnumerator MovementCoroutine()
    {
        while(isMoving)
        {
            SquishStretch();
            yield return new WaitForSeconds(0.3f);
        }
        movementCoroutine = null;
        _Sprite.transform.DOScale(originalScale, 0.1f).SetEase(Ease.InOutBounce);
    }

    public void ToggleChargeParticle(bool isActive)
    {
        _ChargeParticle.SetActive(isActive);
        // Shake pos when charging
        if(isActive)
        {
            //repeat shake
            _Sprite.transform.DOShakePosition(0.5f, 0.2f, 50, 90, false, true).SetLoops(-1);
        }
        // Stop shake
        else
        {
            _Sprite.transform.DOKill();
            _Sprite.transform.localPosition = Vector3.zero;
        }
    }

    public void ChargeBling()
    {
        //Play bling effect
        _Sprite.transform.DOScale(originalScale * 1.5f, 0.5f).SetEase(Ease.InOutBounce).OnComplete(() =>
        {
            _Sprite.transform.DOScale(originalScale, 0.5f).SetEase(Ease.InOutBounce);
        });
    }

    public void Death()
    {
        // Play death effect
        _Sprite.transform.DOScale(originalScale * 2f, 0.8f).SetEase(Ease.InOutBounce);
        _Sprite.transform.DOShakePosition(1f, 0.2f, 50, 90, false, true).OnComplete(() =>
        {
            _Animator.SetTrigger("Dies");
            EventManager.FireEvent(GameEvents.OnLoseGame);
        });
    }
}
