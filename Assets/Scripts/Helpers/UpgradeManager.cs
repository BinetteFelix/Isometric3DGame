using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

public class UpgradeManager : SingletonBehaviour<UpgradeManager>
{
    [SerializeField] private GameObject upgradeScreen;
    [SerializeField] private GameObject levelUpScreen;
    [SerializeField] private Transform tearOrigin;

    #region Upgradeable Variables
    public float MeleeAttackSpeed = 1;
    public float MeleeDamage = 50;
    public float WeaponDamage;
    public float ShotgunReloadSpeed;
    public float SMGReloadSpeed;
    public float MovementSpeed;
    public float PoisonDamage;
    public int PierceAmount;
    public float MaxTearAmount;
    public float MaxShotgunAmmo;
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
            case "Melee Damage":
                {
                    MeleeDamage += value;
                    break;
                }
            case "Melee Speed":
                {
                    MeleeAttackSpeed += value;
                    break;
                }
            case "Weapon Damage":
                {
                    WeaponDamage += value;
                    break;
                }
            case "Shotgun Reload":
                {
                    ShotgunReloadSpeed += value;
                    break;
                }
            case "SMG Reload":
                {
                    SMGReloadSpeed += value;
                    break;
                }
            case "Move Speed":
                {
                    MovementSpeed += value;
                    break;
                }
            case "Poison Tears":
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
                    foreach (Bullet tear in tearOrigin.GetComponentsInChildren<Bullet>(true))
                    {
                        tear.state = Bullet.TearState.piercing;
                        tear.PiercingAmount = PierceAmount;
                    }
                    break;
                }
            case "Max Tears":
                {
                    MaxTearAmount += value;
                    break;
                }
            case "Max Shotgun Ammo":
                {
                    MaxShotgunAmmo += value;
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
