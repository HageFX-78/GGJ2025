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
            
            var dashBehaviour = BehaviourComponentRef?.GetBehaviourScriptable<DashBehaviour>();
            if (dashBehaviour)
            {
                dashBehaviour.OnDash -= SpawnTrail;
            }
        }

        private void SpawnTrail(float dashDuration)
        {
            var InverseDirection = -EnemyMovementRef.cachedDirectionToPlayer;
            var spawnInterval = dashDuration / spawnAmount;
            
            BehaviourComponentRef.StartCoroutine(SpawnCoroutine(spawnInterval, spawnAmount, InverseDirection));
        }

        private IEnumerator SpawnCoroutine(float spawnInterval, float trailAmount, Vector2 InverseDirection)
        {
            var currentSpawnCount = 0;

            while (currentSpawnCount < trailAmount)
            {
                Vector3 spawnedLocation = EnemyMovementRef.transform.position +
                                          new Vector3(InverseDirection.x, InverseDirection.y) * spawnOffset;
                Instantiate(trailPrefab, spawnedLocation, Quaternion.identity);
                currentSpawnCount++;
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}