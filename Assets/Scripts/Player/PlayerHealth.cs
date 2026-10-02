using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    #region COMPONENTS
    [SerializeField] private Renderer rend;
    [SerializeField] private GameObject healthbarUI;
    private Animator animator;
    private Player_Movement movement;
    [SerializeField] InputAction damageAction;
    #endregion

    #region TAKE DAMAGE DATA
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    private Color originalColor;
    #endregion

    public float BaseHealth;
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
        animator = GetComponent<Animator>();
        movement = GetComponent<Player_Movement>();
        damageAction.Enable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = BaseHealth;
        originalColor = rend.material.color;
    }
    private void Update()
    {
        if (damageAction.WasPressedThisFrame())
            TakeDamage(10);
    }

    public void TakeDamage(float damage)
    {
        DamageToTake = damage;
        currentHealth -= DamageToTake;
        healthbarUI.GetComponent<PlayerHealthBar>().HealthBar.fillAmount = ConvertToDecimal(currentHealth);
        Flash();
        if (currentHealth <= 0)
        {
            Invoke("PlayerDead", flashDuration);
        }
    }
    private float ConvertToDecimal(float n)
    {
        return n / BaseHealth;
    }
    public void ResetAttributes()
    {
        currentHealth = BaseHealth;
        rend.material.color = originalColor;

        healthbarUI.GetComponent<PlayerHealthBar>().HealthBar.fillAmount = ConvertToDecimal(currentHealth);
    }
    private void PlayerDead()
    {
        movement.movementAction.Disable();
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
}
