using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;

[RequireComponent(typeof(BehaviourComponent))]
public class Enemy : MonoBehaviour
{
    public bool canCombine = false;
    public float enemySize = 1;
    public bool isABoss = false;
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
        gameObject.GetComponent<Health>().ResetHealth();
    }

    public void SetSize(float newSize)
    {
        enemySize = newSize;
        gameObject.transform.localScale = new Vector3(enemySize, enemySize, enemySize);

    }

    private void OnEnable()
    {
        SetupEnemy();
        StartCoroutine(CombineCountdown());
    }

    private void OnDisable()
    {
        if(isABoss)
        {
            EventManager.FireEvent(GameEvents.OnBossDefeated);
        }
        
        canCombine = false;
    }

    IEnumerator CombineCountdown()
    {
        canCombine = false; // Start cooldown
        yield return new WaitForSeconds(0.1f); // Wait for the cooldown duration
        canCombine = true; // End cooldown

        //print(canCombine);
    }

    public float GetPriorityID()
    {
        return priorityID;
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

    public void OnColliderEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
        // If player apply health dmg and die
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().Damage(gameObject.GetComponent<Health>().health);
            gameObject.GetComponent<Health>().Damage(999); // Suicide
        }
    }
}
