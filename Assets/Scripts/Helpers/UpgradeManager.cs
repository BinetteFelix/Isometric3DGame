using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

public class UpgradeManager : SingletonBehaviour<UpgradeManager>
{
    [SerializeField] private GameObject upgradeScreen;
    [SerializeField] private GameObject levelUpScreen;

    #region Upgradeable Variables
    public float MeleeAttackSpeed = 1;
    public float MeleeDamage = 50;
    public float WeaponDamage;
    public float ShotgunReloadSpeed;
    public float SMGReloadSpeed;
    public float MovementSpeed;
    public float PoisonDamage;
    public float MaxTearAmount;
    public float MaxShotgunAmmo;
    #endregion


    [SerializeField] private InputAction[] shootActions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ObjectiveHandler();
    }
    private void ObjectiveHandler()
    {
        if (EnemyController.Instance.enemyKilledLoopNumber == 5)
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
        if (!levelUpScreen.activeSelf)
            upgradeScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void OpenLevelUpScreen()
    {
        if (!upgradeScreen.activeSelf)
            levelUpScreen.SetActive(true);
        Time.timeScale = 0;
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
                    eyeBall.SwitchTearState(EyeBall.TearState.poisonous);
                    PoisonDamage = value;
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
        Time.timeScale = 1;
    }
    public override void Instantiate()
    {
    }
}
