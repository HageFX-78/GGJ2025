using UnityEngine;

public class PrimaryProjectile : MonoBehaviour
{
    public float lifetime = 10f;
    public float damage = 2;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Enemy"))
        {

            coll.gameObject.GetComponent<IDamageable>()?.Damage(damage);
            Destroy(gameObject);
    
        }
    } 
}
