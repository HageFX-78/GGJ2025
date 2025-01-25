using UnityEngine;

public class AltProjectile : MonoBehaviour
{
    public float lifetime = 10f;
    public float damage = 2;
    public float penetrationPow = 4;
    private float hit = 0;

    private bool destroy = false;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if(hit >= penetrationPow)
        {
            destroy = true;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (destroy)
            {
                other.gameObject.GetComponent<IDamageable>()?.Damage(damage);            
                Destroy(gameObject);
            }
            else
            {
                hit++;
            }
        }
    }
}
