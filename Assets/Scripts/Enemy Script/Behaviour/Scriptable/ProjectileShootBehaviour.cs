using UnityEngine;

namespace Enemy_Script.Behaviour.Scriptable
{
    [CreateAssetMenu(fileName = "ProjectileShootBehaviour", menuName = "BehaviourScriptables/ProjectileShootBehaviour", order = 3)]
    public class ProjectileShootBehaviour : BehaviourScriptable
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float shootInterval = 2f;
        [SerializeField] private float shootOffset = 1f;
        
        
        private ShootProjectile projectileComponent = null;
        
        public override void Setup(Enemy attachedEnemy, GameObject target)
        {
            base.Setup(attachedEnemy, target);

            if (projectileComponent == null)
            {
                projectileComponent = attachedEnemy.gameObject.AddComponent<ShootProjectile>();
                if (projectileComponent == null) 
                    Debug.LogWarning("ProjectileComponent is still null");
            }
        }

        public override void Start()
        {
            base.Start();
        }
        
        public override void Update()
        {
            base.Update();
            
            StartTimer(ShootProjectile, shootInterval, true);
        }

        public override void OnDeath()
        {
            base.OnDeath();
        }

        private void ShootProjectile()
        {
            Transform offsetTransform = EnemyMovementRef.transform;
            projectileComponent.Shoot(offsetTransform);
        }
    }
}