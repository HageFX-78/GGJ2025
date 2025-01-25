using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IHeal
{
    public float health = 1;

    public void Damage(float healthDamage)
    {
        health = health - healthDamage;

        if (health <= 0)
        {
            
        }

        if (gameObject.tag == "Enemy")
        {
            if (health <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                //gameObject.GetComponent<Enemy>().SetupEnemy(health);
            }
        }

    }

    public void SetHealth(float totalHealth)
    {
        health = totalHealth;
    }

    public void Heal(float healAmount) 
    {
        health = health + healAmount;

    }
}
