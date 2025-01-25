using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] private GameObject _Sprite;
    [SerializeField] private ParticleSystem _HitParticle;
    private Coroutine movementCoroutine;
    private bool isMoving = false;

    public void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            ShootEffect();
        }
        else if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            HitEffect();
        }
        MovementSquishStretch();
    }

    void ShootEffect()
    {
        EventManager.FireEvent(GameEvents.OnCallCamShake, ECameraProfile.PlayerShoot);
    }

    Tweener SquishStretch()
    {
        return _Sprite.transform.DOScale(new Vector3(1.2f, 0.8f, 1.2f), 0.15f).SetEase(Ease.InOutBounce).OnComplete(() =>
        {
            _Sprite.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutBounce);
        });
    }

    void HitEffect()
    {
        SquishStretch();
        _HitParticle.Play();
        //Might add more here
        EventManager.FireEvent(GameEvents.OnCallCamShake, ECameraProfile.PlayerPop);
        EventManager.FireEvent(GameEvents.OnPlayerDamaged);
    }

    // Suishstretch when moving but return to normal when stop
    void MovementSquishStretch()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
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
        _Sprite.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InOutBounce);
    }
}
