using UnityEngine;

namespace Enemy_Script.Behaviour.Scriptable
{
    [CreateAssetMenu(fileName = "ProjectileShootBehaviour", menuName = "BehaviourScriptables/ProjectileShootBehaviour", order = 3)]
    public class ProjectileShootBehaviour : BehaviourScriptable
    {
        public override void Setup(Enemy enemyRef, GameObject target)
        {
            base.Setup(enemyRef, target);
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