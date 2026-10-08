using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent m_Agent;
    private Transform target;
    private float updateDestinationTime;
    EnemyHealth health;
    private Animator animator;
    Collider[] player;


    [Header("Attack Variables")]
    public float BaseDamage;
    public float attackRange;
    public bool IsInAttackRange { get; private set; }
    private bool isAttacking;
    private float lastAttackTime;
    public float AttackSpeed;
    Transform attackRangeTransform;
    private float walkAnimThreshold = 0.75f;
        
    #region GIZMOS
    [Range(1, 10)] public float ViewRadius;
    #endregion

    [SerializeField] private LayerMask whatIsPlayer;
    public EnemyState State;
    public enum EnemyState
    {
        idle,
        walking,
        attacking,
        tookDamage,
        dead
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        whatIsPlayer = 1 << LayerMask.NameToLayer("Player");
        animator = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        attackRangeTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        #region TIMERS
        updateDestinationTime -= Time.deltaTime;
        lastAttackTime -= Time.deltaTime;
        #endregion

        #region DESTINATION HANDLER
        if (updateDestinationTime < 0 && !health.EnemyIsDead)
        {
            SetDestination();
            player = Physics.OverlapSphere(attackRangeTransform.position, attackRange, whatIsPlayer);
            if (player.Length != 0)
            {
                IsInAttackRange = true;
            }
            else
            {
                IsInAttackRange = false;
            }
            animator.SetBool("InAttackRange", IsInAttackRange);
        }
        else if (health.EnemyIsDead)
        {
            m_Agent.velocity = Vector3.zero;
        }
        
        #endregion

        if ((m_Agent.velocity.x > walkAnimThreshold || m_Agent.velocity.x < -walkAnimThreshold) || (m_Agent.velocity.z > walkAnimThreshold || m_Agent.velocity.z < -walkAnimThreshold))
        {
            State = EnemyState.walking;
        }
        else if (CanAttack() && lastAttackTime < 0)
        {
            State = EnemyState.attacking;
        }
        else
        {
            State = EnemyState.idle;
        }
            

        StateHandler();
    }

    void StateHandler()
    {
        if (State == EnemyState.idle)
        {
            animator.SetFloat("Speed", 0);
        }
        else if (State == EnemyState.walking)
        {
            animator.SetFloat("Speed", 1);
        }
        else if (State == EnemyState.attacking)
        {
            if (health.EnemyIsDead)
                StopCoroutine(DoAttack());
            else
                StartCoroutine(DoAttack());
        }
        else if (State == EnemyState.tookDamage)
        {

        }
        else if (State == EnemyState.dead)
        {

        }
    }
    private IEnumerator DoAttack()
    {
        isAttacking = true;
        float randomAttack = Random.Range(0, 1.1f);
        animator.SetFloat("WhichAttack", randomAttack);
        animator.SetTrigger("DoAttack");

        float timeInRange = 0;              //initiate attack
        while (timeInRange < 0.75f)
        {
            timeInRange += Time.deltaTime;
            yield return null;
        }
        if (!health.EnemyIsDead && player.Length != 0)
            player[0].GetComponent<PlayerHealth>().TakeDamage(BaseDamage);      //deals damage if the player is still in range after the set time frame, otherwise don't do any damage

        float attackInterval = 1.5f;
        yield return new WaitForSeconds(attackInterval);

        isAttacking = false;
        lastAttackTime = attackInterval;
    }
    #region DESTINATON METHODS
    private void SetDestination()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, ViewRadius);
        foreach (Collider col in collider)
        {
            if (col.tag == "Player")
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
                updateDestinationTime = 0.5f;
                if (m_Agent != null)
                {
                    m_Agent.SetDestination(target.position);
                }
            }
        }
    }
    #endregion

    #region CHECK METHODS
    private bool CanAttack()
    {
        return IsInAttackRange & !isAttacking & !health.EnemyIsDead;
    }
    
    #endregion

    #region PAINTING
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ViewRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackRangeTransform.position + new Vector3(0, 1, 0), attackRange);
    }
    #endregion
}