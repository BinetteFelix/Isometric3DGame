using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AutoFiringGun : MonoBehaviour
{
    private float updateTargetTime;
    private float[] nearestEnemies =
    {
        0, 
        1, 
        2, 
        3, 
        4, 
        5, 
        6
    };
    [SerializeField] private GameObject[] Enemies;

    [SerializeField] private GameObject bulletPrefab;

    private Transform Target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Look();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(nearestEnemies.Min());

        updateTargetTime -= Time.deltaTime;

        if (updateTargetTime < 0)
        {
            updateTargetTime = 0.1f;
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < Enemies.Length; i++)
            {
                nearestEnemies.SetValue(Vector3.Distance(transform.position, Enemies[i].transform.position), i);
                if (Vector3.Distance(transform.position, Enemies[i].transform.position) == nearestEnemies.Min())
                {
                    Target = Enemies[i].transform;

                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                    Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
                    bulletRB.AddForce(transform.forward * 1750);
                }
            }
            
        }
    }
    private float CalculateLookDirection()
    {
        float angle = AngleBetweenTwoPoints(Target.position, transform.position);

        return angle;
    }
    void Look()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, CalculateLookDirection(), 0));
    }
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}