using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPoolList enemyPoolList;
    private GameObject player;
    
    private GameManager gameManager;

    public float spawnRangeMultiplier = 5f;
    
    [Tooltip("How Long until Next Spawn (in Seconds)")]
    public float spawnRate = 5f;

    public bool enableSpawning = true;
    private List<EnemyObjectPool> enemyObjectPool;

    private Camera mainCamera;
    
    private float spawnTimer = 0;
    //private int currentWave = 1; //TEMP

    public List<EnemySpawnInfo> enemySpawnInfo;

    [System.Serializable]
    public class EnemySpawnInfo
    {
        public EnemyObjectPool enemyObjectPool;
       
        [Tooltip("How rare the enemy can spawn (0.0 - 1)")]
        public float spawnChance;

        private int spawnAmount;
        public void UpdateSpawnAmount(int newAmount)
        {
            spawnAmount = newAmount;

        }
        public void UpdateSpawnChance(float newChance)
        {
            spawnChance = newChance;

        }
        public int GetSpawnAmount()
        { 
            return spawnAmount; 
        }
    }
    void Start()
    {
        gameManager = GameManager.Instance;
        player = GameObject.FindWithTag("Player");

        spawnRate = spawnRate * 50;
        mainCamera = Camera.main;
        this.enemyObjectPool = enemyPoolList.enemyObjectPool;
    }

    private void FixedUpdate()
    {
        if(enableSpawning && !gameManager.IS_GAMEOVER && !gameManager.bossModeActivated)
        {
            //Debug.Log("SPAWNING");
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        if (spawnTimer <= 0)
        {
            //Debug.Log("Current Wave: " + currentWave);
            SetEnemyChance();
            Spawn();
            spawnTimer = spawnRate;
            gameManager.currentWave++;
        }
        else
        {
            spawnTimer--;
        }

    }

    public void Spawn()
    { 
        SetEnemySpawnAmount();
        
        for(int i = 0; i < enemySpawnInfo.Count; i++) 
        {
            for (int k = 0; k < enemySpawnInfo[i].GetSpawnAmount(); k++)
            {
                Vector2 spawnPosition = GetRandomPositionOutsideViewport();

                enemySpawnInfo[i].enemyObjectPool.GetPooledEnemy(spawnPosition, Quaternion.Euler(0,0,0));
            }
        }
    }

    private void SetEnemySpawnAmount()
    {
        for (int i = 0; i < enemySpawnInfo.Count ; i++)
        {
            
            if (Random.value < enemySpawnInfo[i].spawnChance)
            {
                enemySpawnInfo[i].UpdateSpawnAmount(enemySpawnInfo[i].GetSpawnAmount() + 1);
                    
            }
         
        }

    }

    private void SetEnemyChance()
    {
        //SET NEW CHANCES HERE






    }

    private Vector3 GetRandomPositionOutsideViewport()
    {
        // Pick a random side: 0 = top, 1 = bottom, 2 = left, 3 = right
        int side = Random.Range(0, 4);
        Vector3 viewportPosition = Vector3.zero;

        switch (side)
        {
            case 0: // Top
                viewportPosition = new Vector3(Random.Range(0f, 1f * spawnRangeMultiplier), 1.1f , 0f);
                break;
            case 1: // Bottom
                viewportPosition = new Vector3(Random.Range(0f, 1f * spawnRangeMultiplier), -0.1f, 0f);
                break;
            case 2: // Left
                viewportPosition = new Vector3(-0.1f, Random.Range(0f, 1f * spawnRangeMultiplier), 0f);
                break;
            case 3: // Right
                viewportPosition = new Vector3(1.1f, Random.Range(0f, 1f * spawnRangeMultiplier), 0f);
                break;
        }

        // Convert viewport position to world position
        return Camera.main.ViewportToWorldPoint(viewportPosition);
    }

    public void TestSpawn(Vector2 spawnPos)
    {
        enemyObjectPool[0].GetPooledEnemy(spawnPos, Quaternion.Euler(0, 0, 0));
    }

}
