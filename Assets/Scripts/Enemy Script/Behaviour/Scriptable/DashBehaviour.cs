using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "DashBehaviour", menuName = "BehaviourScriptables/DashBehaviour", order = 1)]
public class DashBehaviour : BehaviourScriptable
{
    [Header("Dash Config")]
    [SerializeField] private float castTime = 2f;
    [SerializeField] private float dashForce = 5f;
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private ForceMode2D dashForceMode = ForceMode2D.Force;
    
    [Header("Shake Config")]
    [SerializeField] private float strength = 1f;
    [SerializeField] private int vibrato = 1;
    [SerializeField] private float randomness = 45f;
    [SerializeField] private bool snapping = false;
    [SerializeField] private bool fadeOut = true;
    [SerializeField] private ShakeRandomnessMode shakeMode = ShakeRandomnessMode.Full;

    public Action<float> OnDash = null;
        
    public override void Setup(Enemy attachedEnemy, GameObject target)
    {
        base.Setup(attachedEnemy, target);
    }

    public override void Start()
    {
        base.Start();
        
        StartTimer(Dash, dashCooldown + castTime + dashDuration, true);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnDeath()
    {
        base.OnDeath();
    }

    private void Dash()
    {
        BehaviourComponentRef.StartCoroutine(CastingDash());
    }
    
    IEnumerator CastingDash()
    {
        //during cast time, pause movement and shake visual
        EnemyMovementRef.PauseMovement(castTime + dashDuration);
        EnemyVisual.transform.DOShakePosition(castTime, strength, vibrato, randomness, snapping, fadeOut, shakeMode);
        yield return new WaitForSeconds(castTime);
        
        var directionalForce = EnemyMovementRef.CalculateDirectionalForce(EnemyMovementRef.cachedDirectionToPlayer);
        EnemyMovementRef.enemyRigidBody.AddForce(
            new Vector2(directionalForce.movementX * dashForce, directionalForce.movementY * dashForce), dashForceMode);
        
        OnDash?.Invoke(dashDuration);
    }
}
