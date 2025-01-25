using UnityEngine;

namespace Enemy_Script.Behaviour.Scriptable
{
    [CreateAssetMenu(fileName = "ProjectileShootBehaviour", menuName = "BehaviourScriptables/ProjectileShootBehaviour", order = 3)]
    public class ProjectileShootBehaviour : BehaviourScriptable
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float shootInterval = 2f;
        [SerializeField] private float shootOffset = 1f;
        [SerializeField] private float projectileSpeed = 5f;
        
        public override void Setup(Enemy attachedEnemy, GameObject target)
        {
            base.Setup(attachedEnemy, target);
        }

        public override void Start()
        {
            base.Start();
            
            StartTimer(ShootProjectile, shootInterval, true);
        }
        
        public override void Update()
        {
            base.Update();
        }

        public override void OnDeath()
        {
            base.OnDeath();
        }

        private void ShootProjectile()
        {
            Shoot(projectilePrefab);
        }
        
        public void Shoot(GameObject projectile)
        {
            if (!EnemyMovementRef)
            {
                Debug.LogWarning("Trying to shoot while EnemyMovementRef is null");
                return;
            }

            var directionToPlayer = EnemyMovementRef.GetDirectionToPlayerNormalized();
            
            Vector3 spawnedLocation = EnemyMovementRef.transform.position +
                                      new Vector3(directionToPlayer.x, directionToPlayer.y).normalized
                                      * shootOffset;
            
            GameObject spawnProjectile = Instantiate(projectilePrefab, spawnedLocation, Quaternion.identity);
            
            Rigidbody2D rb = spawnProjectile.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                rb.linearVelocity = directionToPlayer * projectileSpeed;
            }
            
        }
    }
}