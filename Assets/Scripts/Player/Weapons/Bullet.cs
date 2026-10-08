using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float LifeTime = 0.25f;
    private Action<Bullet> _killAction;
    private float bulletDamage;
    [SerializeField] private string bulletState;

    private EyeBall eyeBall;

    public int PiercingAmount;
    private int enemiesPierced;

    public TearState state;
    public enum TearState
    {
        piercing,
        directHitting
    }
    private void Start()
    {
        eyeBall = WeaponCollection.Instance.Weapons[1].GetComponent<EyeBall>();
        state = TearState.directHitting;
        PiercingAmount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
            _killAction.Invoke(this);

        if (eyeBall.state == EyeBall.EyeballState.poisonous && this.name == "RegularTear(Clone)")
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
            if (state == TearState.piercing)
            {
                if (enemiesPierced < PiercingAmount)
                {
                    enemiesPierced++;
                    PierceAndDamage(enemy);
                }
                else
                    DirectDamage(enemy);
            }
            else if (state == TearState.directHitting)
            {
                DirectDamage(enemy);
            }
        }
    }
    private void PierceAndDamage(EnemyHealth enemy)
    {
        if (bulletState == "Poisonous")
            enemy.DoLingeringDamage(bulletDamage);
        else if (bulletState == "Basic")
            enemy.TakeDamage(bulletDamage);
    }
    private void DirectDamage(EnemyHealth enemy)
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
    private void OnEnable()
    {
        LifeTime = 0.25f;
        enemiesPierced = 0;
    }
    public void SetDamage(float damage)
    {
        bulletDamage = damage;
    }
}