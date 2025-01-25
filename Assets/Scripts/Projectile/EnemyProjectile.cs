using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime = 5f;
    public float damage = 2;
        
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
        
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamageable>()?.Damage(damage);
            other.gameObject.GetComponent<PlayerEffect>().HitEffect();
            Destroy(gameObject);
        }
    }
}