using UnityEngine;

namespace Enemy_Script.Behaviour.Scriptable
{
    [CreateAssetMenu(fileName = "SpawnEnemyBehaviour", menuName = "BehaviourScriptables/SpawnEnemyBehaviour", order = 4)]
    public class SpawnEnemyBehaviour : BehaviourScriptable
    {
        public override void Setup(Enemy attachedEnemy, GameObject target)
        {
            base.Setup(attachedEnemy, target);
        }

        public override void Start()
        {
            base.Start();
        }
        
        public override void Update()
        {
            base.Update();
        }

        public override void OnDeath()
        {
            base.OnDeath();
        }
    }
}