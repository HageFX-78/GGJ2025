using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private float healthGained = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IHeal>()?.Heal(healthGained);
            Destroy(gameObject);
        }
    }
}
