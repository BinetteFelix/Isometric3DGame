using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponCollection : MonoBehaviour
{
    public static WeaponCollection Instance;
    public GameObject[] Weapons;

    [SerializeField] private InputAction switchWeaponDownAction;
    [SerializeField] private InputAction switchWeaponUpAction;
    bool LastWeaponInCollection;
    bool FirstWeaponInCollection;

    private void Awake()
    {
        switchWeaponDownAction.Enable();
       // switchWeaponUpAction.Enable();
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSwitch() && switchWeaponDownAction.WasPressedThisFrame())
        {
            SwitchWeaponDown();
        }
        if (CanSwitch() && switchWeaponUpAction.WasPressedThisFrame())
        {
            //SwitchWeaponUp();
        }

        LastWeaponInCollection = CheckActiveWeapon() == (Weapons.Length - 1);
        FirstWeaponInCollection = (CheckActiveWeapon() == 0);
        Debug.Log(CheckActiveWeapon());

    }
    private void SwitchWeaponDown()
    {
        if (CheckActiveWeapon() > -1 && !LastWeaponInCollection)
        {
            Weapons[CheckActiveWeapon()].SetActive(false);
            Weapons[CheckActiveWeapon() + 1].SetActive(true);
        }
        else if (LastWeaponInCollection)
        {
            Weapons[CheckActiveWeapon()].SetActive(false);
            Weapons[0].SetActive(true);
            Debug.Log("reached the last weapon");
        }
    }
    private bool CanSwitch()
    {
        return Weapons.Length > 1;
    }
    private int CheckActiveWeapon()
    {
        int activeWeapon = 0;
        for (int i = 0; i < Weapons.Length; i++)
        {
            if (Weapons[i].activeSelf)
            {
                activeWeapon = i;
            }
        }
        return activeWeapon;
    }
}