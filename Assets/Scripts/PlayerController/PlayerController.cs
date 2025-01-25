using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float drag = 0.3f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Health health;
    private float growSize = 1 ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = drag;
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveInput != Vector2.zero)
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }

        
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Resource"))
        {
           PlayerGrow();
        }
    }

    public void PlayerGrow()
    {
        float currentHealth = health.health;
        float shouldGrow;
        if(currentHealth > 50f)
        {
            shouldGrow = (currentHealth - 50) % 10;
            if(shouldGrow == 0)
            {
                growSize += 0.1f;
            }
        }
        transform.localScale = new Vector3(growSize, growSize,1);
    }
}
