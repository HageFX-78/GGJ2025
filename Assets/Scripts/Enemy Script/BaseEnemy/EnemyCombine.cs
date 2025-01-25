using System.Collections.Generic;
using UnityEngine;

public class EnemyCombine : MonoBehaviour, ICombineable
{
    public EnemyPoolList enemyPoolList;
    private List<EnemyObjectPool> enemyObjectPool;


    public void Start()
    {
        this.enemyObjectPool = enemyPoolList.enemyObjectPool;
    }

    public float combineSizeMultiplier = 0.7f;
    public void Combine(float enemySize)
    {
        float newEnemySize = 1;
        newEnemySize = (enemySize * combineSizeMultiplier + gameObject.GetComponent<Enemy>().enemySize);
        

        //TEMP----------------------------------
        GameObject newEnemy = new GameObject();
        if (gameObject.GetComponent<Enemy>().isActiveAndEnabled)
        {
            if (newEnemySize < 2)
            {
               //newEnemy = enemyObjectPool[0].GetPooledEnemy(transform.position, transform.rotation);
            }
            else
            {
                //newEnemy = enemyObjectPool[1].GetPooledEnemy(transform.position, transform.rotation);
            }

        }
       
        //newEnemy.GetComponent<Enemy>().enemySize = newEnemySize;
        //newEnemy.GetComponent<Enemy>().SetupEnemy();

        //newEnemy.GetComponent<Enemy>().SetupEnemy(newEnemySize);
        //newEnemy.GetComponent<Rigidbody2D>().linearVelocity = gameObject.GetComponent<Rigidbody2D>().linearVelocity;

        Debug.Log(newEnemySize);
        //gameObject.SetActive(false);

    }


}
