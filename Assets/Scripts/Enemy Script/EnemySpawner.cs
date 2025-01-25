using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPoolList enemyPoolList;
    public GameObject player;

    [Header("Spawning Enemies")]
    public int minSpawnAmount = 1;
    public int maxSpawnAmount = 2;

    [Tooltip("Default = 1.1")]
    public float minSpawnRange = 1.1f;
    [Tooltip("Default = 1.1")]
    public float maxSpawnRange = 1.5f;

    [Tooltip("How Long until Next Spawn (in Seconds)")]
    public float spawnRate = 5f;

    public bool isWaveModeEnabled;
    public bool enableSpawning = true;
    private List<EnemyObjectPool> enemyObjectPool;

    private Camera mainCamera;
    
    private float spawnTimer = 0;
    private int currentWave = 1; //TEMP

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        spawnRate = spawnRate * 50;
        mainCamera = Camera.main;
        this.enemyObjectPool = enemyPoolList.enemyObjectPool;
    }

    private void FixedUpdate()
    {
        if(enableSpawning)
        {
            if (spawnTimer <= 0)
            {
                Spawn();
                spawnTimer = spawnRate;
                currentWave++;
            }
            else
            {
                spawnTimer--;
            }
        }
        
    }

    public void Spawn()
    { 
        //Debug.Log("SPAWN");
        int spawnAmount = Random.Range(minSpawnAmount, maxSpawnAmount);

        float verticalMod;
        float horizontalMod;

        if (isWaveModeEnabled) 
        {
            spawnAmount = currentWave;

        }
        else if (!isWaveModeEnabled)
        {
            spawnAmount = Random.Range(minSpawnAmount, maxSpawnAmount);
        }

        for(int i = 0; i < spawnAmount; i++) 
        {
            verticalMod = Random.Range(minSpawnRange, maxSpawnRange); // Get Vertical Spawn Pos

            if (Random.Range(0f,1f) < 0.5f) 
            {
                verticalMod = (verticalMod * -1);
            }

            horizontalMod = Random.Range(minSpawnRange, maxSpawnRange); // Get Vertical Spawn Pos
            if (Random.Range(0f, 1f) < 0.5f)
            {
                horizontalMod = (horizontalMod * -1) ;
            }

            Vector2 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(verticalMod, horizontalMod, mainCamera.nearClipPlane));
                
            enemyObjectPool[0].GetPooledEnemy(spawnPosition, Quaternion.Euler(0,0,0));
        }
            
    }
}
