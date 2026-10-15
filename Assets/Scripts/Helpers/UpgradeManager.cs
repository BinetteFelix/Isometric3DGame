using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

public class UpgradeManager : SingletonBehaviour<UpgradeManager>
{
    [SerializeField] public GameObject upgradeScreen;
    [SerializeField] public GameObject levelUpScreen;
    private GameObject tearOrigin;

    #region Upgradeable Variables
    public float MeleeAttackSpeed = 1;
    public float MeleeDamage = 50;
    public float WeaponDamage;
    public float ShotgunReloadSpeed;
    public float SMGReloadSpeed;
    public float MovementSpeed;
    public float PoisonDamage;
    public float AoEDamage;
    public int PierceAmount;
    public int MaxTearAmount;
    public int MaxShotgunAmmo;
    public float tearLifeTime;
    #endregion

    private float nextUpgradeKillCount;

    [SerializeField] private InputAction[] shootActions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextUpgradeKillCount = 5;
    }

    // Update is called once per frame
    void Update()
    {
        ObjectiveHandler();
    }
    private void ObjectiveHandler()
    {
        if (EnemyController.Instance.enemyKilledLoopNumber == nextUpgradeKillCount)
        {
            EnemyController.Instance.enemyKilledLoopNumber = 0;
            if (!upgradeScreen.activeSelf)
            {
                OpenUpgradeScreen();
            }
        }
    }
    public void OpenUpgradeScreen()
    {
        upgradeScreen.SetActive(true);
        nextUpgradeKillCount += 2;
        Time.timeScale = 0;
        UIManager.Instance.SetCursorState(CursorLockMode.None, CursorMode.Auto, default);
    }
    public void OpenLevelUpScreen()
    {
        levelUpScreen.SetActive(true);
        Time.timeScale = 0;
        UIManager.Instance.SetCursorState(CursorLockMode.None, CursorMode.Auto, default);
    }
    public void UpgradePlayer(string name, float value)
    {
        switch (name)
        {
            case "Damage":
                {
                    MeleeDamage *= 1 + value;
                    WeaponDamage *= 1 + value;
                    AoEDamage *= 1 + value;

                    EyeBall eyeBall = WeaponCollection.Instance.Weapons[1].GetComponent<EyeBall>();
                    foreach (Bullet tear in eyeBall.tearPrefabs)
                    {
                        tear.SetDamage(WeaponDamage);
                    }
                    foreach (Bullet tear in eyeBall.TearParent.GetComponentsInChildren<Bullet>(true))
                    {
                        tear.SetDamage(WeaponDamage);
                    }
                    AutoAoE aoE = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AutoAoE>(true);
                    aoE.SetAoEDamage(AoEDamage);
                    break;
                }
            case "Speed":
                {
                    MeleeAttackSpeed *= 1 + value;
                    break;
                }
            case "Reload":
                {
                    ShotgunReloadSpeed *= value;
                    SMGReloadSpeed *= value;
                    break;
                }
            case "Move":
                {
                    MovementSpeed *= 1 + value;
                    break;
                }
            case "Poison":
                {
                    EyeBall eyeBall = WeaponCollection.Instance.Weapons[1].GetComponent<EyeBall>();
                    eyeBall.SwitchTearState(EyeBall.EyeballState.poisonous);
                    PoisonDamage = value;
                    break;
                }
            case "Piercing":
                {
                    PierceAmount++;
                    EyeBall eyeBall = WeaponCollection.Instance.Weapons[1].GetComponent<EyeBall>();
                    foreach(Bullet tear in eyeBall.tearPrefabs)
                    {
                        tear.state = Bullet.TearState.piercing;
                        tear.PiercingAmount = PierceAmount;
                    }
                    foreach (Bullet tear in eyeBall.TearParent.GetComponentsInChildren<Bullet>(true))
                    {
                        tear.state = Bullet.TearState.piercing;
                        tear.PiercingAmount = PierceAmount;
                    }
                    break;
                }
            case "Max Tears":
                {
                    MaxTearAmount += Mathf.RoundToInt(value);
                    EyeBall eyeBall = WeaponCollection.Instance.Weapons[1].GetComponent<EyeBall>();
                    eyeBall.UpgradeMaxTears();
                    break;
                }
            case "Max Shotgun Ammo":
                {
                    MaxShotgunAmmo += Mathf.RoundToInt(value);
                    break;
                }
            case "Range":
                {
                    tearLifeTime *= 1 + value;
                    EyeBall eyeBall = WeaponCollection.Instance.Weapons[1].GetComponent<EyeBall>();
                    foreach (Bullet tear in eyeBall.tearPrefabs)
                    {
                        tear.Range = tearLifeTime;
                    }
                    foreach (Bullet tear in eyeBall.TearParent.GetComponentsInChildren<Bullet>(true))
                    {
                        tear.Range = tearLifeTime;
                    }
                    break;
                }
            case "Reaper":
                {
                    EyeBall eyeBall = WeaponCollection.Instance.Weapons[1].GetComponent<EyeBall>();
                    eyeBall.SwitchTearState(EyeBall.EyeballState.reaper);
                    break;
                }
            case "AoE":
                {
                    AutoAoE aoE = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AutoAoE>(true);
                    aoE.ActivateAoE(AoEDamage);
                    break;
                }
        }
        if (!UIManager.Instance.IsPaused)
            Time.timeScale = 1;
        UIManager.Instance.SetCursorState(CursorLockMode.Confined, CursorMode.Auto, UIManager.Instance.cursor);
    }
    public override void Instantiate()
    {
    }
}
