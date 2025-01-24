using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.FilePathAttribute;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D enemyRigidBody;

    [Header("Movement")]
    public float enemyMaxSpeed = 1f;
    public float enemyAcceleration = 1f;
    public float enemyRotationSpeed = 5f;


    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player Tag is not Set");
        }

        enemyRigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

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

        
       
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * enemyRotationSpeed);
    }
}
