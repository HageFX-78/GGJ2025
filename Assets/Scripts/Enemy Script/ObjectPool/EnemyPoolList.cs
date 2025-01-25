using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Enemy Pool List")]


public class EnemyPoolList : ScriptableObject
{
    [SerializeField]
    public List<EnemyObjectPool> enemyObjectPool = new List<EnemyObjectPool>();

}