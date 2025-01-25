using UnityEngine;

public class Resource : MonoBehaviour
{

    private void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided");
            coll.gameObject.GetComponent<IHeal>()?.Heal(1);
            Destroy(gameObject);
        }
        
    }
 

}
