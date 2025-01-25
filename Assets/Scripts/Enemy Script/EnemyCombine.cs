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
       
        //Vector3 oldSize = new Vector3(GetComponent<Enemy>().enemySize, GetComponent<Enemy>().enemySize, GetComponent<Enemy>().enemySize);

        //TEMP----------------------------------
        GameObject newEnemy = new GameObject();
        if (gameObject.GetComponent<Enemy>())
        {
            newEnemySize = (enemySize * combineSizeMultiplier + gameObject.GetComponent<Enemy>().enemySize);
            if (newEnemySize < 2)
            {
                newEnemy = enemyObjectPool[0].GetPooledEnemy(transform.position, transform.rotation);
            }
            else
            {
                newEnemy = enemyObjectPool[1].GetPooledEnemy(transform.position, transform.rotation);
            }

        }
       


        //newEnemy.GetComponent<Enemy>().enemySize = newEnemySize;
        //newEnemy.GetComponent<Enemy>().SetupEnemy();


        Vector3 newSize = new Vector3(newEnemySize, newEnemySize, newEnemySize);
        newEnemy.GetComponent<Enemy>().SetupEnemy(newSize);
        newEnemy.GetComponent<Rigidbody2D>().linearVelocity = gameObject.GetComponent<Rigidbody2D>().linearVelocity;

        //newEnemy.GetComponent<Enemy>().SetSize(oldSize, newSize);
        Debug.Log(newEnemySize);
        gameObject.SetActive(false);

    }


}
