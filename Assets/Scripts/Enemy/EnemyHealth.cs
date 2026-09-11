using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    #region COMPONENTS

    #endregion

    #region TAKE DAMAGE DATA
    [SerializeField] private Renderer rend;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    private Color originalColor;
    #endregion

    [SerializeField] private EnemyData enemyData;
    [SerializeField] private GameObject HealthbarPrefab;
    private GameObject healthbarUI;
    private Transform worldSpaceCanvas;
    private Vector3 healthbarPos = new Vector3(0, 2f, 0);

    private float currentHealth;
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            Mathf.Clamp(currentHealth, 0, enemyData.BaseHealth);
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
        currentHealth -= damage;
        healthbarUI.GetComponent<HealthUI>().HealthBar.fillAmount = ConvertToDecimal(currentHealth);
        Flash();
        if (currentHealth <= 0)
        {
            Invoke("EnemyDead", flashDuration);
        }
    }
    private void EnemyDead()
    {
        Destroy(gameObject);
    }
    private IEnumerator DamageEffect()
    {
        rend.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = originalColor;
    }
    private void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(DamageEffect());
    }

    private float ConvertToDecimal(float n)
    {
        return n / 100;
    }
    public void UpdateMaxHealth(float damage)
    {

    }
    public void Heal(float damage)
    {

    }
}