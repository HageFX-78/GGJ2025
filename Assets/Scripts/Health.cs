using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IHeal
{
    public float maxHealth = 1;
    public float health = 1;

    private bool isDead = false;


    public void Damage(float healthDamage)
    {
        health = health - healthDamage;
        if (gameObject.CompareTag("Player"))
        {
            EventManager.FireEvent(GameEvents.OnPlayerDamaged, health);
            Debug.Log("PLAYER HEALTH: " + health);
            if(health <= 0 && !isDead)
            {
                isDead = true;
                EventManager.FireEvent(GameEvents.OnPlayerDeathStart);
            }
        }

        if (gameObject.CompareTag("Enemy"))
        {
            if (health <= 0 && !isDead)
            {
                isDead = true;

                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                gameObject.transform.Find("Enemy Hitbox").GetComponent<CircleCollider2D>().enabled = false;
                
                gameObject.GetComponent<Enemy>().canCombine = false;
                if (gameObject.GetComponent<Enemy>().enemySize > 2)
                {
                    gameObject.GetComponent<EnemyEffect>().DisplayDeathSequence();
                   
                    Invoke("InvokeDeath", 2.0f);
                    EventManager.FireEvent(GameEvents.IncreasePopCount);
                }
                else
                {
                    gameObject.GetComponent<EnemyEffect>().Explode(false);
                   
                    Invoke("InvokeDeath", 1.0f);
                    EventManager.FireEvent(GameEvents.IncreasePopCount);
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
    
    public void InvokeDeath()
    {
        isDead = false;
        gameObject.SetActive(false);
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
