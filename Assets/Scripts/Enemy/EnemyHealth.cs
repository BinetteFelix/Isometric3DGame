using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    #region COMPONENTS
    [SerializeField] public Renderer rend;
    BoxCollider collider;
    Animator animator;
    #endregion

    #region TAKE DAMAGE DATA
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    private Color originalColor;
    #endregion

    public float BaseHealth;
    public GameObject HealthbarPrefab;
    private GameObject healthbarUI;
    private Transform worldSpaceCanvas;
    public Vector3 healthbarPos = new Vector3(0, 2f, 0);
    public float deathAnimationLength;
    public bool EnemyIsDead {  get; private set; }
    bool tookHit;
    private float DamageToTake;
    private float currentHealth;
    public float CurrentHealth
    {
        get
        {
            return Mathf.Clamp(currentHealth, 0, BaseHealth);
        }
        set
        {
            return;
        }
    }

    private void Awake()
    {
        worldSpaceCanvas = GameObject.FindGameObjectWithTag("WorldSpaceCanvas").transform;
        healthbarUI = Instantiate(HealthbarPrefab, worldSpaceCanvas);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider>();
        currentHealth = BaseHealth;
        originalColor = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        healthbarUI.transform.position = transform.position + healthbarPos;
    }
    public void TakeDamage(float damage)
    {
        if (EnemyIsDead)
            return;

        DamageToTake = damage;
        currentHealth -= DamageToTake;
        healthbarUI.GetComponent<EnemyHealthBar>().HealthBar.fillAmount = ConvertToDecimal(currentHealth);
        Flash();

        if (currentHealth <= 0 && !EnemyIsDead)
        {
            if (EnemyController.HasInstance && currentHealth <= 0)
            {
                EnemyController.Instance.EnemiesKilled++;
                EnemyController.Instance.UpdateEnemyCountUI();
            }
            animator.SetBool("Dead", true);
            animator.SetTrigger("TakeHit");
            Invoke("EnemyDead", deathAnimationLength);
            collider.enabled = false;
            EnemyIsDead = true;
            return;
        }

        if (!tookHit && currentHealth > 0 || damage < currentHealth && !EnemyIsDead)
            animator.SetTrigger("TakeHit");

        tookHit = true;
        return;
    }

    #region LingeringDamage
    public void DoLingeringDamage(float damage)
    {
        TakeDamage(damage);
        CancelInvoke(nameof(TakeLingeringDamage));
        if (currentHealth > 0)
            InvokeRepeating(nameof(TakeLingeringDamage), 1f, 1f);
    }
    private void TakeLingeringDamage()
    {
        DamageToTake = UpgradeManager.Instance.PoisonDamage;
        TakeDamage(DamageToTake);
    }
    #endregion

    public void ResetAttributes()
    {
        currentHealth = BaseHealth;
        rend.material.color = originalColor;
        EnemyIsDead = false;

        healthbarUI.GetComponent<EnemyHealthBar>().HealthBar.fillAmount = ConvertToDecimal(currentHealth);
        healthbarUI.GetComponent<EnemyHealthBar>().ResetAlpha();
        healthbarUI.SetActive(true);
    }
    private void EnemyDead()
    {
        if (EnemyController.HasInstance)
            EnemyController.Instance.EnemyPool.Release(this.gameObject);
    }
    private IEnumerator DamageEffect()
    {
        rend.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = originalColor;
        tookHit = false;
    }
    private void Flash()
    {
        if (gameObject.activeSelf)
        {
            StopAllCoroutines();
            StartCoroutine(DamageEffect());
        }   
    }

    private float ConvertToDecimal(float n)
    {
        return n / 100;
    }
    private void OnDisable()
    {
        if (ExperienceHandler.HasInstance && currentHealth <= 0)
            ExperienceHandler.Instance.SpawnXP(transform.position);

    }
}