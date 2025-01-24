using UnityEngine;
using System.Collections;

public class EnemyHit : MonoBehaviour
{
    private float priorityID;

    public void Start()
    {
        priorityID = Random.Range(0f, 100f); //set ID for combining purposes
    }
    public void FixedUpdate()
    {
        
    }

    public float GetPriorityID()
    {
        return priorityID;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Player")
        {



        }

        else if (collision.tag == "Enemy")
        {
            EnemyHit enemyCombine = collision.GetComponent<EnemyHit>();

            if (enemyCombine.GetPriorityID() > this.priorityID)
            {
                ICombineable combineable= collision.GetComponent<ICombineable>();
                if (combineable != null)
                {
                    combineable.Combine(gameObject.GetComponent<Enemy>().enemySize);
                }
                
                Destroy(gameObject);
            }
     
        }
  
    }

 
}
