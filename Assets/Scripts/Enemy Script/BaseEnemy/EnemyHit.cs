using UnityEngine;
using System.Collections;

public class EnemyHit : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Player")
        {

        }

        else if (collision.tag == "Enemy")
        {
            Enemy collidedEnemy = collision.GetComponent<Enemy>();

         
            if (collidedEnemy.enemySize > gameObject.GetComponent<Enemy>().enemySize)
            {
           
                ICombineable combineable = collision.GetComponent<ICombineable>();
                if (combineable != null && collision.isActiveAndEnabled)
                {
                    combineable.Combine(gameObject.GetComponent<Enemy>().enemySize);
                }

                gameObject.SetActive(false);

            }
            else if (collidedEnemy.enemySize == gameObject.GetComponent<Enemy>().enemySize)    //need cleaning (if not lazy)
            {
                if (collidedEnemy.GetPriorityID() > gameObject.GetComponent<Enemy>().GetPriorityID())
                {
                    ICombineable combineable = collision.GetComponent<ICombineable>();
                    if (combineable != null && collision.isActiveAndEnabled)
                    {
                        combineable.Combine(gameObject.GetComponent<Enemy>().enemySize);
                    }

                    gameObject.SetActive(false);
                }
            }
            
        }
  
    }

 
}
