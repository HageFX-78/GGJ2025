using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BehaviourComponent : MonoBehaviour
{
    public List<BehaviourScriptable> behavioursToAttach = new List<BehaviourScriptable>();
    
    private List<BehaviourScriptable> currentBehaviours = new List<BehaviourScriptable>();
    private GameObject targetPlayer = null;
    private Enemy attachedEnemy = null;
    
    public void SetupBehaviours(Enemy enemyRef, GameObject target)
    {
        //cache target player and sort by priority before setup
        attachedEnemy = enemyRef;
        targetPlayer = target;

        foreach (BehaviourScriptable behaviour in behavioursToAttach)
        {
            currentBehaviours.Add(Instantiate(behaviour));
        }
        
        SortBehavioursByPriority();
        foreach (BehaviourScriptable behaviour in currentBehaviours)
        {
            behaviour.Setup(attachedEnemy, targetPlayer);
        }
        
        foreach (BehaviourScriptable behaviour in currentBehaviours)
        {
            behaviour.Start();
        }
    }

    public void Update()
    {
        foreach (BehaviourScriptable behaviour in currentBehaviours)
        {
            behaviour.Update();
        }
    }

    public void OnDeath()
    {
        foreach (BehaviourScriptable behaviour in currentBehaviours)
        {
            behaviour.OnDeath();
        }
    }

    private void SortBehavioursByPriority()
    {
        currentBehaviours = currentBehaviours.OrderBy(x=> x.priority).ToList();
    }

    public T GetBehaviourScriptable<T>() where T : BehaviourScriptable
    {
        return (T)currentBehaviours.FirstOrDefault(x => x.GetType() == typeof(T));
    }
    
}
