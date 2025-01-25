using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public float health = 1;

    public void Damage(float healthDamage)
    {
        health = health - healthDamage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
