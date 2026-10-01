using System.Collections;
using UnityEngine;

public class PlayerAutoAttacking : MonoBehaviour
{
    #region COMPONENTS
    [SerializeField] private Player_Movement movement;
    [SerializeField]private Transform attackRangeTransform;
    [SerializeField] private float meleeAttackRange;
    public bool IsInMeleeRange {  get; private set; }
    public float MeleeAttackSpeed;
    private float lastAttackTime;
    private Animator animator;
    #endregion

    Collider[] enemies;
    [SerializeField] private LayerMask whatIsEnemy;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();
        lastAttackTime -= Time.deltaTime;
    }
    
    public void StateMachine()
    {
        enemies = Physics.OverlapSphere(attackRangeTransform.position, meleeAttackRange, whatIsEnemy);
        IsInMeleeRange = enemies.Length > 0;
        animator.SetBool("InRangeForMeleeAttack", IsInMeleeRange);

        if (IsInMeleeRange)
        {
            float[] distances = new float[enemies.Length];
            foreach (var enemy in enemies)
            {
                float distanceToPlayer = Vector3.Distance(enemy.transform.position, attackRangeTransform.position);
                distances.SetValue(distanceToPlayer, 0);

                if (Mathf.Min(distances) <= meleeAttackRange && !movement.IsMeleeAttacking && lastAttackTime < 0)
                {
                    StartCoroutine(DoMeleeAttack(enemy));
                }
            }
        }
    }
    private IEnumerator DoMeleeAttack(Collider enemy)
    {
        animator.speed = UpgradeManager.Instance.MeleeAttackSpeed;
        movement.IsMeleeAttacking = true;
        animator.SetFloat("RandomMeleeAttack", Random.Range(0, 2));
        animator.SetTrigger("DoAttack");

        float attackInterval = 1 / UpgradeManager.Instance.MeleeAttackSpeed;
        yield return new WaitForSeconds(attackInterval);

        if (enemy != null)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(UpgradeManager.Instance.MeleeDamage);
        }

        movement.IsMeleeAttacking = false;
        lastAttackTime = attackInterval;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackRangeTransform.position + new Vector3(0, 1, 0), meleeAttackRange);
    }
}
