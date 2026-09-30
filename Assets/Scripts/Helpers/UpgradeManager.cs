using UnityEngine;
using Utility;

public class UpgradeManager : SingletonBehaviour<UpgradeManager>
{
    [SerializeField] private GameObject UpgradeScreen;

    #region Upgradeable Variables
    public float MeleeAttackSpeed = 1;
    public float MeleeDamage = 50;
    public float WeaponDamage;
    public float ShotgunReloadSpeed;
    public float SMGReloadSpeed;
    public float MovementSpeed;
    #endregion

    private Objectives objState;
    private enum Objectives
    {
        obj1,
        obj2,
        obj3
    }
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
        if (EnemyController.Instance.EnemiesKilled == 5)
        {
            EnemyController.Instance.EnemiesKilled = 0;
            UpgradeScreen.SetActive(true);
        }
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
        }
    }
    public override void Instantiate()
    {
    }
}
