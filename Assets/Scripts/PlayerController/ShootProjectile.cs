using System;
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
    public float chargeTime = 2f; // Time required to fully charge the attack

    [SerializeField] private InputActionReference attackBtn;
    [SerializeField] private InputActionReference altAttackBtn;
    [SerializeField] private PlayerEffect playerEffect;

    private float nextFireTime = 0f;
    private float altFireTime = 0f;
    private float chargingStartTime = 0f;
    private bool isCharging = false;
    private bool altCoolDown = false;

    private void OnEnable()
    {
        if (altAttackBtn)
        {
            altAttackBtn.action.started += StartCharging;
            altAttackBtn.action.canceled += PerformChargedAttack;
        }
    }

    private void OnDisable()
    {
        if (altAttackBtn)
        {
            altAttackBtn.action.started -= StartCharging;
            altAttackBtn.action.canceled -= PerformChargedAttack;
        }
    }

    private void StartCharging(InputAction.CallbackContext context)
    {
        if (!altCoolDown)
        {
            isCharging = true;
            chargingStartTime = Time.time; // Start the charging timer
            playerEffect.ToggleChargeParticle(true); // Turn on the charging particle effect
        }
    }

    private void PerformChargedAttack(InputAction.CallbackContext context)
    {
        if (isCharging)
        {
            isCharging = false;
            float chargingDuration = Time.time - chargingStartTime; // Calculate how long the button was held

            if (chargingDuration >= chargeTime) // Check if the charge duration is sufficient
            {
                AltShoot(arrowOffSet); // Perform the alternate fire
                playerEffect.ToggleChargeParticle(false); // Turn off the particle effect
                playerEffect.ChargeBling(); // Play the charge bling effect
                altCoolDown = true;
                altFireTime = Time.time + coolDownTime; // Start cooldown
                EventManager.FireEvent(GameEvents.AltFireSetCD, coolDownTime);
            }
            else
            {
                // Charging was canceled before the charge time was met
                Debug.Log("Charged attack was not fully charged!");
                playerEffect.ToggleChargeParticle(false); // Turn off the particle effect
            }
        }
    }

    private void Update()
    {
        if (attackBtn != null && attackBtn.action.IsPressed())
        {
            if (Time.time >= nextFireTime)
            {
                Shoot(arrowOffSet);
                nextFireTime = Time.time + fireRate;
            }
        }

        if (altCoolDown && Time.time >= altFireTime)
        {
            altCoolDown = false; // Reset cooldown
        }
    }

    public void Shoot(Transform projectileTransform)
    {
        GameObject projectile = Instantiate(projectile1, projectileTransform.position, projectileTransform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
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
