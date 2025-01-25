using UnityEngine;

public class PrimaryProjectile : MonoBehaviour
{
    public float lifetime = 10f;
    public float damage = 2;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IDamageable>()?.Damage(damage);
            Destroy(gameObject);
    
        }
    } 
}
