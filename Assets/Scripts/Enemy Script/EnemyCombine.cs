using UnityEngine;

public class EnemyCombine : MonoBehaviour, ICombineable
{

    public GameObject enemyPrefab;
    public float combineSizeMultiplier = 0.7f;
    public void Combine(float enemySize)
    {
        float newEnemySize = (enemySize + gameObject.GetComponent<Enemy>().enemySize) * combineSizeMultiplier;

        GameObject newEnemy = Instantiate(enemyPrefab);

        newEnemy.GetComponent<Enemy>().enemySize = newEnemySize;
        newEnemy.GetComponent<Enemy>().SetupEnemy();
        newEnemy.GetComponent<Rigidbody2D>().linearVelocity = gameObject.GetComponent<Rigidbody2D>().linearVelocity;
        Destroy(gameObject);
    }

   

 

}
