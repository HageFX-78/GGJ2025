using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    public GameObject player;

    private Rigidbody2D enemyRigidBody;

    [Header("Movement")]
    public float enemyMaxSpeed = 1;
    public float enemyAcceleration = 1;
 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }



    private void Move()
    {
        Vector2 directionToPlayer = (player.transform.position - transform.position);

        Vector2 _targetSpeed = directionToPlayer.normalized * enemyMaxSpeed;
        Vector2 _speedDifferences = _targetSpeed - enemyRigidBody.linearVelocity;
      
        float _movementX = Mathf.Abs(_speedDifferences.x) * enemyAcceleration * Mathf.Sign(_speedDifferences.x);
        float _movementY = Mathf.Abs(_speedDifferences.y) * enemyAcceleration * Mathf.Sign(_speedDifferences.y);

        enemyRigidBody.AddForce(new Vector2(_movementX, _movementY));

    }
}
