using System.Collections.Generic;
using UnityEngine;

public class EnemyCombine : MonoBehaviour, ICombineable
{
    public EnemyPoolList enemyPoolList;
    public GameManager gameManager;

    private List<EnemyObjectPool> enemyObjectPool;


    public void Start()
    {
        gameManager = GameManager.Instance;
        this.enemyObjectPool = enemyPoolList.enemyObjectPool;
    }

    public float combineSizeMultiplier = 0.7f;
    public void Combine(float enemySize)
    {
        float newEnemySize = 1;
        float addedSize = enemySize * combineSizeMultiplier;
        newEnemySize = (addedSize + gameObject.GetComponent<Enemy>().enemySize);

        float currentSize = GetComponent<Enemy>().enemySize;
        
        GameObject newEnemy;
     
        if(GetComponent<Enemy>().canCombine)
        {
            bool doCreateNewEnemy = false;
            int newEnemyTier = 1;

            if (newEnemySize > 2 && currentSize < 2)
            {
                doCreateNewEnemy = true;
                newEnemyTier = 1;
            }
            else if (newEnemySize > 3 && currentSize < 3)
            {
                doCreateNewEnemy = true;
                newEnemyTier = 2;
            }
            else if (newEnemySize > 4 && currentSize < 4)
            {
                doCreateNewEnemy = true;
                newEnemyTier = 3;
                EventManager.FireEvent(GameEvents.OnBossSpawn);

            }
            
            if(doCreateNewEnemy)
            {
                newEnemy = enemyObjectPool[newEnemyTier].GetPooledEnemy(transform.position, transform.rotation);
                newEnemy.GetComponent<Rigidbody2D>().linearVelocity = gameObject.GetComponent<Rigidbody2D>().linearVelocity;

                gameObject.SetActive(false);
            }

            GetComponent<Health>().Heal(addedSize);
            GetComponent<Enemy>().SetSize(newEnemySize);

        }
    }
}
