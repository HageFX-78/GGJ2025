using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BehaviourComponent))]
public class Enemy : MonoBehaviour
{
    public float enemySize = 1;
    private float priorityID;

    [HideInInspector] public BehaviourComponent behaviourComponent;
    [HideInInspector] public EnemyMovement movementComponent;
    [HideInInspector] public GameObject visualChild;
    
    private void Awake()
    {
        SetupComponent();
        SetupEnemy();
        priorityID = Random.Range(0f, 100f); //set ID for combining purposes

        if (behaviourComponent == null)
        {
            Debug.LogWarning("Error, should not be possible to have behaviourComponent- null");
        }
    }
  
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        if (behaviourComponent != null)
        {
            behaviourComponent.SetupBehaviours(this, player);
        }
    }

    public void SetupEnemy()
    {
        gameObject.transform.localScale = new Vector3(enemySize, enemySize, enemySize);
    }

    public void SetupEnemy(float newSize)
    {
        gameObject.transform.localScale = new Vector3(enemySize, enemySize, enemySize);

        enemySize = newSize;
    }

    public float GetPriorityID()
    {
        return priorityID;
    }

    private void ProcessBehaviour()
    {
        behaviourComponent?.Update();
    }

    private void OnDestroy()
    {
        behaviourComponent?.OnDeath();
    }

    private void SetupComponent()
    {
        movementComponent = GetComponent<EnemyMovement>();
        behaviourComponent = GetComponent<BehaviourComponent>();
        visualChild = GetComponentInChildren<SpriteRenderer>().gameObject;
    }
}
