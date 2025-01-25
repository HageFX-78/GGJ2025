using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] private GameObject _Sprite;

    public void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            ShootEffect();
        }
    }

    void ShootEffect()
    {
        EventManager.FireEvent(GameEvents.OnCallCamShake, ECameraProfile.PlayerShoot);
    }
    void MovementDeform(Vector3 moveDirection)
    {
        //Squeeze and stretch sprite based on movement direction

    }
}
