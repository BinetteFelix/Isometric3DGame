using UnityEngine;

public class WeaponBobbing : MonoBehaviour
{
    public GameObject player;

    [SerializeField] private Transform startPos;

    [SerializeField] private float frequenzy = 5f;

    [SerializeField] private float magnitude = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = startPos.position + transform.up * Mathf.Sin(Time.time * frequenzy) * magnitude;
        }
    }
}
