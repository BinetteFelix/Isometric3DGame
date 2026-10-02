using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    #region COMPONENTS
    [SerializeField] private Renderer rend;
    [SerializeField] private GameObject healthbarPrefab;
    private GameObject healthbarUI;
    private Animator animator;
    private Player_Movement movement;
    [SerializeField] InputAction damageAction;
    #endregion

    #region TAKE DAMAGE DATA
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    private Color originalColor;
    #endregion

    public bool PlayerIsDead {  get; private set; }
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
        healthbarUI = Instantiate(healthbarPrefab, GameObject.FindGameObjectWithTag("WorldSpaceCanvas").transform);
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

        if (currentHealth <= 0 || damage >= currentHealth)
        {
            CancelInvoke(nameof(ReEnableInput));
            Invoke(nameof(PlayerDead), flashDuration);
        }
        else
        {
            animator.SetTrigger("TookDamage");
            Invoke(nameof(ReEnableInput), 1f);
        }
        DoDamage(damage);
        Flash();
    }
    private void DoDamage(float damage)
    {
        movement.movementAction.Disable();
        DamageToTake = damage;
        currentHealth -= DamageToTake;
        healthbarUI.GetComponent<PlayerHealthBar>().HealthBar.fillAmount = ConvertToDecimal(currentHealth);
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
        PlayerIsDead = true;

        animator.SetTrigger("TookDamage");
        animator.SetBool("DeadState", PlayerIsDead);
    }
    private IEnumerator DamageEffect()
    {
        rend.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        rend.material.color = originalColor;
    }
    private void ReEnableInput()
    {
        movement.movementAction.Enable();
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
