using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    #region COMPONENTS
    [SerializeField] private Renderer rend;
    #endregion

    #region TAKE DAMAGE DATA
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    private Color originalColor;
    #endregion

    [SerializeField] private EnemyData enemyData;
    [SerializeField] private GameObject HealthbarPrefab;
    private GameObject healthbarUI;
    private Transform worldSpaceCanvas;
    private Vector3 healthbarPos = new Vector3(0, 2f, 0);

    private float DamageToTake;
    private float currentHealth;
    public float CurrentHealth
    {
        get
        {
            return Mathf.Clamp(currentHealth, 0, enemyData.BaseHealth);
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
        currentHealth = enemyData.BaseHealth;
        originalColor = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        healthbarUI.transform.position = transform.position + healthbarPos;
    }
    public void TakeDamage(float damage)
    {
        DamageToTake = damage;
        currentHealth -= DamageToTake;
        healthbarUI.GetComponent<EnemyHealthBar>().HealthBar.fillAmount = ConvertToDecimal(currentHealth);
        Flash();
        if (currentHealth <= 0)
        {
            Invoke("EnemyDead", flashDuration);
        }
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
        currentHealth = enemyData.BaseHealth;
        rend.material.color = originalColor;

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
        if (EnemyController.HasInstance && currentHealth <= 0)
        {
            EnemyController.Instance.EnemiesKilled++;
            EnemyController.Instance.UpdateEnemyCountUI();
        }
        if (ExperienceHandler.HasInstance && currentHealth <= 0)
            ExperienceHandler.Instance.SpawnXP(transform.position);

    }
}