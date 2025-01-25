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

  /*  public void SetSize(Vector3 oldSize, Vector3 newSize)
    {
    
        transform.localScale = Vector3.Lerp(oldSize, newSize , Time.deltaTime * 0.001f);
        enemySize = newSize.x;
    }*/

    public void SetupEnemy()
    {
        //transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale = new Vector3(enemySize, enemySize, enemySize), Time.deltaTime * 10);
        gameObject.transform.localScale = new Vector3(enemySize, enemySize, enemySize);
    }

    public void SetupEnemy(Vector3 newSize)
    {
        //transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale = new Vector3(enemySize, enemySize, enemySize), Time.deltaTime * 10);
        gameObject.transform.localScale = newSize;
        enemySize = newSize.x;

    }


    public float GetPriorityID()
    {
        return priorityID;
    }

}
