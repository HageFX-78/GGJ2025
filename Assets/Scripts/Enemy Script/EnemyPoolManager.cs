using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{

    public EnemyPoolList enemyPoolList;

    private List<EnemyObjectPool> enemyObjectPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.enemyObjectPool = enemyPoolList.enemyObjectPool;

        for (int index = 0; index < enemyObjectPool.Count; index++)
        {
            enemyObjectPool[index].InitiateEnemyPool();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
