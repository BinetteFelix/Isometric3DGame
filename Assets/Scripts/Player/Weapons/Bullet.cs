using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float LifeTime = 0.5f;
    private Action<Bullet> _killAction;
    private float bulletDamage;
    [SerializeField] private string bulletState;

    private EyeBall eyeBall;
    private void Start()
    {
        eyeBall = WeaponCollection.Instance.Weapons[1].GetComponent<EyeBall>();
    }
    // Update is called once per frame
    void Update()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
            _killAction.Invoke(this);

        if (eyeBall.state == EyeBall.TearState.poisonous && this.name == "RegularTear(Clone)")
            Destroy(this.gameObject);
    }
    public void Init(Action<Bullet> killAction)
    {
        _killAction = killAction;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            if (bulletState == "Poisonous")
            {
                enemy.DoLingeringDamage(bulletDamage);
                _killAction.Invoke(this);
            }
            else if (bulletState == "Basic")
            {
                enemy.TakeDamage(bulletDamage);
                _killAction.Invoke(this);
            }
        }
    }
    private void OnEnable()
    {
        LifeTime = 0.5f;
    }
    public void SetDamage(float damage)
    {
        bulletDamage = damage;
    }
}