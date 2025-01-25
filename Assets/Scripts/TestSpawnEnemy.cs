using UnityEngine;

public class TestSpawnEnemy : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public EnemyPoolList enemyPoolList; // Assign the prefab in the Inspector
    public Camera mainCamera; // Assign the main camera (or use Camera.main in Start)

    void Start()
    {
        enemySpawner.Spawn();
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Automatically find the main camera
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            //SpawnObjectAtMousePosition();
        }
    }

    void SpawnObjectAtMousePosition()
    {



        Vector3 mousePosition = Input.mousePosition;


        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        //worldPosition.z = 0f; // Set z to 0 for 2D or adjust for your needs

        enemySpawner.Spawn();
        
        /*GameObject newEnemy = enemyPoolList.enemyObjectPool[0].GetPooledEnemy(worldPosition, Quaternion.identity);

        newEnemy.GetComponent<Enemy>().enemySize = 1;
        newEnemy.GetComponent<Enemy>().SetupEnemy();*/
        //Instantiate(objectToSpawn, worldPosition, Quaternion.identity);
    }
}
