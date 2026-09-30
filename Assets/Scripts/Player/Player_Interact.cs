using UnityEngine;

public class Player_Interact : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Experience xpObj = other.GetComponent<Experience>();

        if (xpObj != null)
        {
            xpObj.ReleaseToPool();
        }
    }
}
