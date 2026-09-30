using UnityEngine;

public class Player_Interact : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Experience xpObj = other.GetComponent<Experience>();

        if (xpObj != null)
        {
            xpObj.ReleaseToPool();
        }
    }
}
