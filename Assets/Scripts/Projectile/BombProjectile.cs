using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    [SerializeField] private float detonateTime = 2f;
    [SerializeField] private float detonateRadius = 3f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private EnemyEffect enemyEffect;//Effects script

    private void Start()
    {
        Detonate();
        enemyEffect.DisplayDeathSequence();
    }

    private void Detonate()
    {
        var circleOverlap = Physics2D.OverlapCircleAll(transform.position, detonateRadius);

        foreach (var hit in circleOverlap)
        {
            if (hit.CompareTag("Player"))
            {
                IDamageable damageComponent = hit.GetComponent<IDamageable>();
                if (damageComponent != null)
                {
                    damageComponent.Damage(damage);
                }
            }
        }
        
        Destroy(gameObject, detonateTime);
    }
}