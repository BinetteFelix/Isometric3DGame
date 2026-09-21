using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float LifeTime = 1.5f;
    private Action<Bullet> _killAction;
    private float bulletDamage;

    // Update is called once per frame
    void Update()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
            _killAction.Invoke(this);
    }
    public void Init(Action<Bullet> killAction)
    {
        _killAction = killAction;
        Debug.Log(killAction);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(bulletDamage);
            _killAction.Invoke(this);
        }
    }
    private void OnEnable()
    {
        LifeTime = 1.5f;
    }
    public void SetDamage(float damage)
    {
        bulletDamage = damage;
    }
}