using UnityEngine;

namespace Enemy_Script.Behaviour.Scriptable
{
    [CreateAssetMenu(fileName = "TrailBehaviour", menuName = "BehaviourScriptables/TrailBehaviour", order = 2)]
    public class TrailBehaviour : BehaviourScriptable
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