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
            SpawnObjectAtMousePosition();
        }
    }

    void SpawnObjectAtMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        
        enemySpawner.TestSpawn(worldPosition);
        
    }
}
