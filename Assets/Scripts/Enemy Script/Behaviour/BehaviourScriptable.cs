using System;
using System.Collections;
using UnityEngine;

public class BehaviourScriptable : ScriptableObject
{
    public int priority = 0;

    protected BehaviourComponent BehaviourComponentRef = null;
    protected EnemyMovement EnemyMovementRef = null;
    protected GameObject TargetPlayerRef = null;
    protected GameObject EnemyVisual = null;

    protected Coroutine timerCoroutine = null;
    
    public virtual void Setup(Enemy attachedEnemy, GameObject target)
    {
        BehaviourComponentRef = attachedEnemy.behaviourComponent;
        EnemyMovementRef = attachedEnemy.movementComponent;
        TargetPlayerRef = target;
        EnemyVisual = attachedEnemy.visualChild;
        
        if (TargetPlayerRef == null)
        {
            Debug.LogWarning("No target player assigned.");
        }
    }

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void OnDeath()
    {
        
    }

    protected void StartTimer(Action callback, float intervalSeconds, bool isRepeating)
    {
        timerCoroutine = BehaviourComponentRef.StartCoroutine(TimerCoroutine(callback, intervalSeconds, isRepeating));
    }
    
    private IEnumerator TimerCoroutine(Action callback, float seconds, bool isRepeating)
    {
        do
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        } while (isRepeating);
    }

    protected void StopTimer()
    {
        BehaviourComponentRef.StopCoroutine(timerCoroutine);
    }
}
