using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeTime = 0.25f;
    public float Range = 0.25f;
    private Action<Bullet> _killAction;
    private float bulletDamage;
    [SerializeField] private string bulletState;

    private EyeBall eyeBall;
    [SerializeField] private ParticleSystem[] hitEffect;

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
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            _killAction.Invoke(this);

        if (eyeBall.state == EyeBall.EyeballState.poisonous && (this.name == "RegularTear(Clone)" || this.name == "ReaperTear(Clone)") || 
            eyeBall.state == EyeBall.EyeballState.reaper && (this.name == "RegularTear(Clone)" || this.name == "PoisonTear(Clone)") || 
            eyeBall.state == EyeBall.EyeballState.basic && (this.name == "PoisonTear(Clone)" || this.name == "ReaperTear(Clone)")
            )
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
            if (eyeBall.state == EyeBall.EyeballState.reaper)
                enemy.InitHealthSteal();
        }
    }
    private void PierceAndDamage(EnemyHealth enemy)
    {
        if (bulletState == "Poisonous")
        {
            enemy.DoLingeringDamage(bulletDamage);
            enemy.PlayEffect(1);
        }
        else if (bulletState == "Basic")
        {
            enemy.TakeDamage(bulletDamage);
            enemy.PlayEffect(2);
        }
        else if (bulletState == "Reaper")
        {
            enemy.TakeDamage(bulletDamage);
            enemy.PlayEffect(3);
        }
    }
    private void DirectDamage(EnemyHealth enemy)
    {
        if (bulletState == "Poisonous")
        {
            enemy.DoLingeringDamage(bulletDamage);
            _killAction.Invoke(this);
            enemy.PlayEffect(1);
        }
        else if (bulletState == "Basic")
        {
            enemy.TakeDamage(bulletDamage);
            _killAction.Invoke(this);
            enemy.PlayEffect(2);
        }
        else if(bulletState == "Reaper")
        {
            enemy.TakeDamage(bulletDamage);
            _killAction.Invoke(this);
            enemy.PlayEffect(3);
        }
    }
    private void OnEnable()
    {
        lifeTime = Range;
        enemiesPierced = 0;
    }
    public void SetDamage(float damage)
    {
        bulletDamage = damage;
    }
}