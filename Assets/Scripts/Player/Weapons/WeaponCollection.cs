using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponCollection : MonoBehaviour
{
    public static WeaponCollection Instance;

    #region LISTS
    public List<GameObject> Weapons;
    #endregion

    #region INPUT ACTIONS
    [SerializeField] private InputAction switchWeaponDownAction;
    [SerializeField] private InputAction switchWeaponUpAction;
    #endregion

    #region DELAY VARIABLES
    public float shotgunDelay = 0.62f;
    public float smgDelay = 0.1f;
    #endregion

    public string CurrentWeaponHeld { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentWeaponHeld = "Shotgun";

        switchWeaponDownAction.Enable();
        switchWeaponUpAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        #region INPUT HANDLER
        if (CanSwitch() && switchWeaponDownAction.WasPressedThisFrame())
        {
            SwitchWeaponDown();
        }
        if (CanSwitch() && switchWeaponUpAction.WasPressedThisFrame())
        {
            SwitchWeaponUp();
        }
        #endregion
    }

    #region SWITCH METHODS
    private void SwitchWeaponDown()
    {
        var actW = CheckActiveWeapon();

        if (actW >= 0 && !Weapons[Weapons.Count - 1].activeSelf)
        {
            Weapons[actW].SetActive(false);
            Weapons[actW + 1].SetActive(true);
        }
        else if (Weapons[Weapons.Count - 1].activeSelf)
        {
            Weapons[actW].SetActive(false);
            Weapons[0].SetActive(true);
        }

        CurrentWeaponHeld = Weapons[CheckActiveWeapon()].name;
       
    }
    private void SwitchWeaponUp()
    {
        var actW = CheckActiveWeapon();

        if (actW > 0)
        {
            Weapons[actW].SetActive(false);
            Weapons[actW - 1].SetActive(true);
        }
        else if (actW == 0)
        {
            Weapons[actW].SetActive(false);
            Weapons[Weapons.Count - 1].SetActive(true);
        }

        CurrentWeaponHeld = Weapons[CheckActiveWeapon()].name;
    }
    #endregion

    #region SWITCH STATE CHECK
    private bool CanSwitch()
    {
        return Weapons.Count > 1;
    }
    private int CheckActiveWeapon()
    {
        int activeWeapon = 0;
        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].activeSelf)
            {
                activeWeapon = i;
            }
        }
        return activeWeapon;
    }
    #endregion
}