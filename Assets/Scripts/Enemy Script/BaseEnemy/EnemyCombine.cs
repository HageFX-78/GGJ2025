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

        float currentSize = GetComponent<Enemy>().enemySize;
        
        GameObject newEnemy;
     
        if(GetComponent<Enemy>().canCombine)
        {
            if (newEnemySize > 2 && currentSize < 2)
            {
                newEnemy = enemyObjectPool[1].GetPooledEnemy(transform.position, transform.rotation);
                newEnemy.GetComponent<Enemy>().SetupEnemy(newEnemySize);
                newEnemy.GetComponent<Rigidbody2D>().linearVelocity = gameObject.GetComponent<Rigidbody2D>().linearVelocity;
                gameObject.SetActive(false);
            }
           
            GetComponent<Enemy>().SetupEnemy(newEnemySize);
           
            //Debug.Log(newEnemySize);
        }
    }
}
