using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFireGun : MonoBehaviour
{
    #region INPUT ACTIONS
    [SerializeField] private InputAction shootAction;
    [SerializeField] private InputAction reloadAction;
    #endregion

    #region COMPONENTS
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform BulletOrigin;
    [SerializeField] private Transform[] shotgunBulletTransforms;
    [SerializeField] private TextMeshProUGUI bulletAmount;

    private WeaponCollection weaponCollection;
    #endregion

    #region Weapon Variables
    [Header("Weapon Variables")]
    [SerializeField] private float shotDelay;
    [SerializeField] private float reloadSpeed;
    #endregion

    #region EFFECTS
    [SerializeField] private ParticleSystem WeaponEffect;
    #endregion

    #region STATE PARAMETERS
    public float LastPressedShot { get; private set; }

    private Transform PlayerTransform;

    public int maxAmmo;
    private int ammo;
    [HideInInspector]
    public int Ammo
    {
        get
        {
            return Mathf.Clamp(ammo, 0, maxAmmo);
        }
        set
        {
            return;
        }
    }
    #endregion

    private void Awake()
    {
        shootAction.Enable();
        reloadAction.Enable();
    }
    private void Start()
    {
        ammo = maxAmmo;
        bulletAmount.text = $"{Ammo}";

        weaponCollection = GetComponentInParent<WeaponCollection>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        #region TIMERS
        LastPressedShot -= Time.deltaTime;
        #endregion

        #region INPUT HANDLER
        if (weaponCollection.CurrentWeaponHeld == "Shotgun")
        {
            if (CanShoot() && shootAction.WasPressedThisFrame())
            {
                ShootShotgun();
                WeaponEffect.Play();
                LastPressedShot = shotDelay;
            }
        }
        else if (weaponCollection.CurrentWeaponHeld == "SMG")
        {
            if (CanShoot() && shootAction.IsPressed())
            {
                ShootSMG();
                WeaponEffect.Play();
                LastPressedShot = shotDelay;
            }
        }

        if (Ammo <= 0 && shootAction.WasPressedThisFrame())
        {
            Invoke("ReloadGun", reloadSpeed);
        }
        if (reloadAction.WasPressedThisFrame())
        {
            Invoke("ReloadGun", reloadSpeed);
        }
        #endregion
    }

    #region ACTION METHODS
    private void ShootSMG()
    {
        CancelReload();
        GameObject bullet = Instantiate(BulletPrefab, BulletOrigin.position, Quaternion.identity);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();

        ammo--;
        bulletAmount.text = $"{Ammo}";

        bulletRB.AddForce(PlayerTransform.forward * 1500);

        float bulletDespawnTimer = 3;
        bulletDespawnTimer -= Time.deltaTime;

        if (bulletDespawnTimer < 0)
            Destroy(bullet);
    }
    private void ShootShotgun()
    {
        CancelReload();
        int bulletSpawned = 0;
        List<GameObject> bullets = new List<GameObject>();

        #region Spawn Bullets
        for (int i = 0; i < 9; i++)
        {
            bullets.Add(BulletPrefab);
            bullets[i] = Instantiate(BulletPrefab, BulletOrigin.position + new Vector3(Random.Range(0.1f, 0.4f), Random.Range(0.1f, 0.15f), 0), Quaternion.identity);
        }
        #endregion

        #region Calculate Bullet Direction
        foreach (GameObject bullet in bullets)
        {
            if (bulletSpawned >= 0 && bulletSpawned < 4)
            {
                Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
                bulletRB.AddForce(shotgunBulletTransforms[0].forward * 1750);
            }
            else if (bulletSpawned > 4 && bulletSpawned < 7)
            {
                Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
                bulletRB.AddForce(shotgunBulletTransforms[1].forward * 1750);
            }
            else
            {
                Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
                bulletRB.AddForce(shotgunBulletTransforms[2].forward * 1750);
            }
            bulletSpawned++;
        }
        #endregion

        #region Update UI
        ammo--;
        bulletAmount.text = $"{Ammo}";
        #endregion

        #region Despawning Bullets
        float bulletDespawnTimer = 3;
        bulletDespawnTimer -= Time.deltaTime;

        if (bulletDespawnTimer < 0)
        {
            bullets.Remove(bullets[0]);
            Destroy(bullets[0]);
        }
        #endregion
    }
    private void ReloadGun()
    {
        ammo = maxAmmo;
        bulletAmount.text = $"{Ammo}";
    }
    #endregion

    #region CHECK METHODS
    private bool CanShoot()
    {
        return LastPressedShot < 0 && Ammo > 0;
    }
    #endregion

    private void OnEnable()
    {
        bulletAmount.text = $"{Ammo}";
    }
    public void CancelReload()
    {
        CancelInvoke("ReloadGun");
    }
}