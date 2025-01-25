using UnityEngine;

namespace Enemy_Script.Behaviour.Scriptable
{
    [CreateAssetMenu(fileName = "SpawnEnemyBehaviour", menuName = "BehaviourScriptables/SpawnEnemyBehaviour", order = 4)]
    public class SpawnEnemyBehaviour : BehaviourScriptable
    {
        [Header("Spawn Configs")]
        [SerializeField] private GameObject spawnedEnemy;
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private float spawnMaxCooldown = 2f;
        [SerializeField] private float spawnInnerRadius = 3f;
        [SerializeField] private float spawnOuterRadius = 5f;
        
        private float SpawnDowntime => Random.Range(0, spawnMaxCooldown);
        private CalculateAnnulus CalculateAnnulus;
        
        public override void Setup(Enemy attachedEnemy, GameObject target)
        {
            base.Setup(attachedEnemy, target);
            CalculateAnnulus = new CalculateAnnulus(attachedEnemy.transform.position, spawnInnerRadius, spawnOuterRadius);
        }

        public override void Start()
        {
            base.Start();
            
            StartTimer(SpawnEnemy, spawnInterval + SpawnDowntime, true);
        }
        
        public override void Update()
        {
            base.Update();
        }

        public override void OnDeath()
        {
            base.OnDeath();
        }

        private void SpawnEnemy()
        {
            var randomizedSpawnPosition = CalculateAnnulus.GetRandomPointInAnnulus2D();
            
            //TODO: SPAWN ENEMY WITH OBJECT POOLING
            Instantiate(spawnedEnemy, randomizedSpawnPosition, Quaternion.identity);
        }
    }
}