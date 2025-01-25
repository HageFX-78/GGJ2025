using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject projectile1;
    public Transform arrowOffSet;
    public float projectileSpeed;
    public float fireRate = 5f;

    private float nextFireTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(Time.time >= nextFireTime)
            {
                Shoot(arrowOffSet);
                nextFireTime = Time.time + fireRate;
            }
           
        }   
    }
    
    void Shoot(Transform projectileTransform)
    {
        GameObject projectile = Instantiate(projectile1, projectileTransform.position, projectileTransform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            rb.linearVelocity = arrowOffSet.right * projectileSpeed;
        }
    }
}
