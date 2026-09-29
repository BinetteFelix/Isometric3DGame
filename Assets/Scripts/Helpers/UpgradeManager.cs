using UnityEngine;
using Utility;

public class UpgradeManager : SingletonBehaviour<UpgradeManager>
{
    [SerializeField] private GameObject UpgradeScreen;

    [SerializeField] public GameObject[] UpgradeTypes;
    

    public float MeleeAttackSpeed = 1;
    public float MeleeDamage = 50;
    public float WeaponDamage;

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
    }
    private void ObjectiveHandler()
    {
        if (EnemyController.Instance.EnemiesKilled == 5)
        {
            EnemyController.Instance.EnemiesKilled = 0;
            UpgradeScreen.SetActive(true);
            

        }
    }

    private void UpgradeType(string name, float value)
    {
        
    }
    private void ObjectiveType1()
    {
        objState = Objectives.obj1;
        
    }
    private void ObjectiveType2()
    {
        objState = Objectives.obj2;
    }
    private void ObjectiveType3()
    {
        objState = Objectives.obj3;
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
        }
    }
    public override void Instantiate()
    {
    }
}
