using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IHeal
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

    public void Heal(float healAmount) 
    {
        health = health + healAmount;

    }
}
