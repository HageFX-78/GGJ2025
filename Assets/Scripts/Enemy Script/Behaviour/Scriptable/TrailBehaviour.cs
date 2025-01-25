using System.Collections;
using UnityEngine;

namespace Enemy_Script.Behaviour.Scriptable
{
    [CreateAssetMenu(fileName = "TrailBehaviour", menuName = "BehaviourScriptables/TrailBehaviour", order = 2)]
    public class TrailBehaviour : BehaviourScriptable
    {
        [SerializeField] private GameObject trailPrefab;
        [SerializeField] private int spawnAmount = 5;
        [SerializeField] private float spawnOffset = 1f;
        
        public override void Setup(Enemy attachedEnemy, GameObject target)
        {
            base.Setup(attachedEnemy, target);

            var dashBehaviour = BehaviourComponentRef.GetBehaviourScriptable<DashBehaviour>();
            
            if (dashBehaviour)
            {
                dashBehaviour.OnDash += SpawnTrail;
            }
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

            if (BehaviourComponentRef != null)
            {
                var dashBehaviour = BehaviourComponentRef.GetBehaviourScriptable<DashBehaviour>();
                if (dashBehaviour)
                {
                    dashBehaviour.OnDash -= SpawnTrail;
                }
            }
        }

        private void SpawnTrail(float dashDuration)
        {
            var inverseDirection = -EnemyMovementRef.GetDirectionToPlayerNormalized();
            var spawnInterval = dashDuration / spawnAmount;
            
            BehaviourComponentRef.StartCoroutine(SpawnCoroutine(spawnInterval, spawnAmount, inverseDirection));
        }

        private IEnumerator SpawnCoroutine(float spawnInterval, float trailAmount, Vector2 inverseDirection)
        {
            var currentSpawnCount = 0;

            while (currentSpawnCount < trailAmount)
            {
                if (!EnemyMovementRef)
                {
                    yield break;
                }
                
                Vector3 spawnedLocation = EnemyMovementRef.transform.position +
                                          new Vector3(inverseDirection.x, inverseDirection.y) * spawnOffset;
                Instantiate(trailPrefab, spawnedLocation, Quaternion.identity);
                currentSpawnCount++;
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}