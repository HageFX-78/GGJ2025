using UnityEngine;

public class ToturialScript : MonoBehaviour
{
    public float removeTime = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("DestroyTotu", removeTime);
    }

    void DestroyTotu()
    {
        Destroy(gameObject);
    }
}
