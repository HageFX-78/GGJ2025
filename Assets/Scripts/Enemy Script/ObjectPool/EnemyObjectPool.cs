using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Enemy Object Pool")]


public class EnemyObjectPool : ScriptableObject
{
    public GameObject enemyPrefab;

    public int totalMax;

    public Queue<GameObject> spawnedEnemies;

    private Transform parent;


    public void InitiateEnemyPool()
    {
        spawnedEnemies = new Queue<GameObject>(); //bugged, dunno why need to initialize here
        if (spawnedEnemies == null || spawnedEnemies.Count == 0)
        {
            
            spawnedEnemies = new Queue<GameObject>();
            
        }

        if (spawnedEnemies.Count >= totalMax)
        {
            return;
        }

        if (!parent)
        {
            parent = new GameObject(name).transform;
        }

        while (spawnedEnemies.Count < totalMax)
        {
            GameObject obj = Instantiate(enemyPrefab, parent);
            obj.SetActive(false);
            spawnedEnemies.Enqueue(obj);
        }
        
    }

    public GameObject GetPooledEnemy(Vector2 position, Quaternion rotation)
    {
        if (spawnedEnemies == null || spawnedEnemies.Count == 0)
        {
            InitiateEnemyPool();
            Debug.LogWarning("Enemies Pool is not Spawned in the Start");
        }

        GameObject newEnemy = spawnedEnemies.Dequeue();

        spawnedEnemies.Enqueue(newEnemy);
        newEnemy.SetActive(false);

        newEnemy.transform.position = position;
        newEnemy.transform.rotation = rotation;
        newEnemy.SetActive(true);

        return newEnemy;
    }
}