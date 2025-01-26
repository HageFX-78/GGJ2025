using UnityEngine;
using System.Collections;
using DG.Tweening.Core.Easing;

public class EnemyHit : MonoBehaviour
{
    GameManager gameManager;
    public void Start()
    {
        gameManager = GameManager.Instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        bool isCombining = false;

        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamageable>()?.Damage(GetComponent<Enemy>().damage);
            collision.gameObject.GetComponent<PlayerEffect>().HitEffect();

            gameObject.SetActive(false);
        }

        else if (collision.CompareTag("Enemy"))
        {
            Enemy collidedEnemy = collision.GetComponent<Enemy>();

            if (collidedEnemy.enemySize > gameObject.GetComponent<Enemy>().enemySize && collidedEnemy.canCombine)
            {
                isCombining= true;
               
            }
            else if (collidedEnemy.enemySize == gameObject.GetComponent<Enemy>().enemySize && collidedEnemy.canCombine)    //need cleaning (if not lazy)
            {
                isCombining = true;
            }
            if (gameManager != null)
            {
                if (isCombining && collidedEnemy.canCombine && !gameManager.bossModeActivated)
                {

                    ICombineable combineable = collision.GetComponent<ICombineable>();
                    if (combineable != null && collision.isActiveAndEnabled)
                    {
                        combineable.Combine(gameObject.GetComponent<Enemy>().enemySize);
                        gameObject.SetActive(false);
                    }
                }

            }
           
            
        }

    }
}

