using UnityEngine;

public class AutoAoE : MonoBehaviour
{
    public Collider[] enemies;
    [SerializeField] private ParticleSystem effectAoE;
    private float AoEDamage = 25f;
    private float attackRepeatSpeed;
    [SerializeField] private LayerMask whatIsEnemy;
    private void Start()
    {
        attackRepeatSpeed = 2;
    }
    private void FixedUpdate()
    {
        attackRepeatSpeed -= Time.deltaTime;
        if (attackRepeatSpeed < 0)
        {
            enemies = Physics.OverlapSphere(transform.position, 6, whatIsEnemy);
            if (enemies.Length > 0)
            {
                attackRepeatSpeed = 2;
                effectAoE.Play(true);

                Invoke(nameof(DealDamage), 0.5f);
            }
        }
    }
    void DealDamage()
    {
        foreach (var enemy in enemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(AoEDamage);
            enemyHealth.PlayEffect(4);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 6);
    }
    public void SetAoEDamage(float newDamage)
    {
        AoEDamage = newDamage;
    }
    public void ActivateAoE(float damage)
    {
        SetAoEDamage(damage);
        gameObject.SetActive(true);
    }
}