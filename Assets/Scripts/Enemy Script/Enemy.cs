using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemySize = 1;

    private void Awake()
    {
        SetupEnemy();
    }
  
    void Start()
    {
        
    }

    public void SetupEnemy()
    {
        gameObject.transform.localScale = new Vector3(enemySize, enemySize, enemySize);

    }


 
    void Update()
    {
        
    }
}
