using UnityEngine;

public class AutoPickup : MonoBehaviour
{
    Collider[] xpColliders;
    public float xpGatherArea;
    public float gatherTime;
    private float drawXPUpdateTime;
    [SerializeField] private LayerMask whatIsXP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        drawXPUpdateTime -= Time.deltaTime;

        if (drawXPUpdateTime < 0)
        {
            xpColliders = Physics.OverlapSphere(transform.position, xpGatherArea, whatIsXP);
            if (xpColliders.Length > 0)
            {
                DrawInXP(xpColliders[0].gameObject);
            }
            
        }
    }
    private void DrawInXP(GameObject xpObj)
    {
        //float posX = Mathf.Lerp(xpObj.transform.position.x, transform.position.x, gatherTime);
        //float posY = Mathf.Lerp(xpObj.transform.position.y, transform.position.y, gatherTime);
        //float posZ = Mathf.Lerp(xpObj.transform.position.z, transform.position.z, gatherTime);

        Rigidbody xpRB = xpObj.GetComponent<Rigidbody>();
        xpRB.MovePosition(xpObj.transform.position * 5);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(transform.position, xpGatherArea);
    }
}
