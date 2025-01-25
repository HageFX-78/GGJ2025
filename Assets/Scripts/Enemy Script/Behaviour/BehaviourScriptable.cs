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
    
    public virtual void Setup(Enemy enemyRef, GameObject target)
    {
        BehaviourComponentRef = enemyRef.behaviourComponent;
        EnemyMovementRef = enemyRef.movementComponent;
        TargetPlayerRef = target;
        EnemyVisual = enemyRef.visualChild;
        
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

    protected void StartTimer(Action callback, float seconds, bool isRepeating)
    {
        timerCoroutine = BehaviourComponentRef.StartCoroutine(TimerCoroutine(callback, seconds, isRepeating));
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
