using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IHeal
{
    public float maxHealth = 1;
    public float health = 1;

    private bool isDead = false;


    public void Damage(float healthDamage)
    {
        health = health - healthDamage;

        if (health <= 0)
        {
            
        }

        if (gameObject.tag == "Enemy")
        {
            if (health <= 0 && !isDead)
            {
                isDead = true;
                gameObject.GetComponent<Enemy>().canCombine = false;
                if (gameObject.GetComponent<Enemy>().enemySize > 2)
                {
                    gameObject.GetComponent<EnemyEffect>().DisplayDeathSequence();
                    Invoke("InvokeDeath", 2.0f);
                }
                else
                {
                    gameObject.GetComponent<EnemyEffect>().Explode(false);
                    Invoke("InvokeDeath", 1.0f);
                }
                
            }
            else
            {
                //gameObject.GetComponent<Enemy>().SetupEnemy(health);
            }
        }

    }

  
    public void ResetHealth()
    {
        health = maxHealth;
    }
   /* public void SetHealth(float totalHealth)

    public void InvokeDeath()
    {
        isDead = false;
        gameObject.SetActive(false);
    }

    public void SetHealth(float totalHealth)

    {
        health = totalHealth;
    }*/

    public void Heal(float healAmount) 
    {
        health = health + healAmount;

    }
}
