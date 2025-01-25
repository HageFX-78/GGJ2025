using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootProjectile : MonoBehaviour
{
    public GameObject projectile1;
    public GameObject projectile2;
    public Transform arrowOffSet;
    public float projectileSpeed;
    public float projectile2Speed;
    public float fireRate = 0.5f;
    public float coolDownTime = 5f;
    
    [SerializeField] private InputActionReference attackBtn;
    [SerializeField] private InputActionReference altAttackBtn;
    private float nextFireTime = 0f;
    private float altFireTime = 0f;
    private bool altCoolDown = false;

    private void OnEnable()
    {
        if(altAttackBtn)
            altAttackBtn.action.performed += ChargedAttack;
    }

    private void OnDisable()
    {
        if(altAttackBtn)
            altAttackBtn.action.performed -= ChargedAttack;
    }

    private void ChargedAttack(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            AltShoot(arrowOffSet);
            altCoolDown = true;
        }
    }

    void Update()
    {
        if (attackBtn != null && attackBtn.action.IsPressed())
        {
            if(Time.time >= nextFireTime)
            {
                Shoot(arrowOffSet);
                nextFireTime = Time.time + fireRate;
            }
           
        }

        if (altCoolDown)
        {
            if(Time.time >= altFireTime)
            {
                altCoolDown = false;
                altFireTime = Time.time + coolDownTime;
            }

        }
    }
    
    public void Shoot(Transform projectileTransform)
    {
        GameObject projectile = Instantiate(projectile1, projectileTransform.position, projectileTransform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if(rb != null)
        {
            rb.linearVelocity = projectileTransform.right * projectileSpeed;
        }
    }

    public void AltShoot(Transform projectileTransform)
    {
        GameObject projectile = Instantiate(projectile2, projectileTransform.position, projectileTransform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = projectileTransform.right * projectile2Speed;
        }
    }
}
