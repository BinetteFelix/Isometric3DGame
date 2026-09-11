using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowPlayer : MonoBehaviour
{
    private NavMeshAgent m_Agent;
    private Transform target;
    private float updateDestinationTime;


    #region GIZMOS
    [Range(1, 10)] public float ViewRadius;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        #region TIMERS
        updateDestinationTime -= Time.deltaTime;
        #endregion

        #region DESTINATION HANDLER
        if (updateDestinationTime < 0)
            SetDestination();
        #endregion
    }

    #region DESTINATON METHODS
    private void SetDestination()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, ViewRadius);
        foreach (Collider c in collider)
        {
            if (c.tag == "Player")
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

    #region PAINTING
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ViewRadius);
        
    }
    #endregion
}