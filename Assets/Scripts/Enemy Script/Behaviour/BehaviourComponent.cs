using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BehaviourComponent : MonoBehaviour
{
    public List<BehaviourScriptable> behaviours = new List<BehaviourScriptable>();
    
    private GameObject targetPlayer = null;
    private Enemy attachedEnemy = null;
    
    public void SetupBehaviours(Enemy enemyRef, GameObject target)
    {
        //cache target player and sort by priority before setup
        attachedEnemy = enemyRef;
        targetPlayer = target;
        SortBehavioursByPriority();
        
        foreach (BehaviourScriptable behaviour in behaviours)
        {
            behaviour.Setup(attachedEnemy, targetPlayer);
        }
    }

    public void Start()
    {
        foreach (BehaviourScriptable behaviour in behaviours)
        {
            behaviour.Start();
        }
    }

    public void Update()
    {
        foreach (BehaviourScriptable behaviour in behaviours)
        {
            behaviour.Update();
        }
    }

    public void OnDeath()
    {
        foreach (BehaviourScriptable behaviour in behaviours)
        {
            behaviour.OnDeath();
        }
    }

    private void SortBehavioursByPriority()
    {
        behaviours = behaviours.OrderBy(x=> x.priority).ToList();
    }

    public T GetBehaviourScriptable<T>() where T : BehaviourScriptable
    {
        return (T)behaviours.FirstOrDefault(x => x.GetType() == typeof(T));
    }
    
}
