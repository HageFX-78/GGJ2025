using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.FilePathAttribute;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    private GameObject player;
    [HideInInspector] public Rigidbody2D enemyRigidBody;

    [Header("Movement")]
    public float enemyMaxSpeed = 1f;
    public float enemyAcceleration = 1f;
    public float enemyRotationSpeed = 5f;
    public bool canMove = true;

    public Vector2 cachedDirectionToPlayer;
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
        cachedDirectionToPlayer = GetDirectionToPlayer();

        if (canMove)
        {
            Move();
        }
        
        //update rotation based on directionToPlayer
        UpdateRotation(cachedDirectionToPlayer);
    }


    private void Move()
    {
        //get direction, calculate the directional force
        var directionalForce = CalculateDirectionalForce(cachedDirectionToPlayer);
        enemyRigidBody.AddForce(new Vector2(directionalForce.movementX, directionalForce.movementY));
    }

    public void PauseMovement(float seconds)
    {
        StartCoroutine(PauseMovementCoroutine(seconds));
    }
    
    private IEnumerator PauseMovementCoroutine(float seconds) 
    {
        //pause all movement and prepare to dash
        enemyRigidBody.linearVelocity = Vector2.zero;
        canMove = false;
        yield return new WaitForSeconds(seconds);
        canMove = true;
    }

    public Vector2 GetDirectionToPlayer()
    {
        return (player.transform.position - transform.position);
    }

    public void UpdateRotation(Vector2 directionToPlayer)
    {
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * enemyRotationSpeed);
    }

    public(float movementX, float movementY) CalculateDirectionalForce(Vector2 directionToPlayer)
    {
        Vector2 _targetSpeed = directionToPlayer.normalized * enemyMaxSpeed;
        Vector2 _speedDifferences = _targetSpeed - enemyRigidBody.linearVelocity;

        float _movementX = Mathf.Abs(_speedDifferences.x) * enemyAcceleration * Mathf.Sign(_speedDifferences.x);
        float _movementY = Mathf.Abs(_speedDifferences.y) * enemyAcceleration * Mathf.Sign(_speedDifferences.y);
        
        return (_movementX, _movementY);
    }
}
