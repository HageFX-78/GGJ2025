using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IHeal
{
    public float health = 1;

    public void Damage(float healthDamage)
    {
        health = health - healthDamage;

        if (health <= 0)
        {
            if(gameObject.tag == "Enemy")
            {
                gameObject.SetActive(false);
            }
            // NEED CHANGE BECAUSE ENEMY DOESNT DESTROY
        }
    }

    public void Heal(float healAmount) 
    {
        health = health + healAmount;

    }
}
