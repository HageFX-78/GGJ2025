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

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Enemy"))
        {
            if (destroy)
            {
                coll.gameObject.GetComponent<IDamageable>()?.Damage(damage);            
                Destroy(gameObject);
            }
            else
            {
                hit++;
            }
            
        }
    }
}
