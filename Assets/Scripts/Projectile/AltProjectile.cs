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
            other.gameObject.GetComponent<IDamageable>()?.Damage(damage);
            other.gameObject.GetComponent<EnemyEffect>()?.DisplayHitParticles(other.transform.position - transform.position);            
            if (destroy)
            {
                Destroy(gameObject);
                EventManager.FireEvent(GameEvents.IncreasePopCount);
            }
            else
            {
                hit++;
            }
        }
    }
}
