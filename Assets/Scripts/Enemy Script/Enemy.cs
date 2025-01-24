using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemySize = 1;
    private float priorityID;

    private void Awake()
    {
        SetupEnemy();
        priorityID = Random.Range(0f, 100f); //set ID for combining purposes
    }
  
    void Start()
    {
        
    }

    public void SetupEnemy()
    {
        gameObject.transform.localScale = new Vector3(enemySize, enemySize, enemySize);

    }
    public float GetPriorityID()
    {
        return priorityID;
    }

}
