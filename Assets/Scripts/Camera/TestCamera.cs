using UnityEngine;

public class TestCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Invoke("Test", 3.0f);
    }
    void Test()
    {
        EventManager.FireEvent(GameEvents.OnCallCamShake, ECameraProfile.EnemyPop);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
