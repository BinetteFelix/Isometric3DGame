using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PlayerFireGun : MonoBehaviour
{
    #region INPUT ACTIONS
    [SerializeField] private InputAction shootAction;
    [SerializeField] private InputAction reloadAction;
    #endregion

    #region OBEJCT POOLING
    private ObjectPool<Bullet> _bulletPool;
    [SerializeField] private bool _usePool;
    #endregion

    #region COMPONENTS
    [SerializeField] private Bullet BulletPrefab;
    [SerializeField] private Transform BulletOrigin;
    [SerializeField] private Transform[] shotgunBulletTransforms;
    [SerializeField] private TextMeshProUGUI bulletAmount;

    private WeaponCollection weaponCollection;
    #endregion

    #region Weapon Variables
    [Header("Weapon Variables")]
    [SerializeField] private float shotDelay;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private float weaponDamage;
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
        _usePool = true;
        _bulletPool = new ObjectPool<Bullet>(
            () => 
            {
                return Instantiate(BulletPrefab, BulletOrigin.position, Quaternion.identity); 

            },
            bullet => 
            {
                bullet.transform.position = BulletOrigin.position;
                bullet.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                bullet.gameObject.SetActive(true);
            },
            bullet => 
            {
                bullet.gameObject.transform.position = BulletOrigin.position;
                bullet.gameObject.SetActive(false); 
            },
            bullet => 
            { 
                Destroy(bullet.gameObject); 
            },
            false, 
            50, 
            100
            );

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
        Bullet bullet = _bulletPool.Get();
        bullet.SetDamage(weaponDamage);
        bullet.GetComponent<Rigidbody>().AddForce(BulletOrigin.forward * 1500);

        ammo--;
        bulletAmount.text = $"{Ammo}";

        bullet.Init(DestroyBullet);
    }
    private void ShootShotgun()
    {
        CancelReload();
        int bulletSpawned = 0;
        List<Bullet> bullets = new List<Bullet>();

        #region Spawn Bullets
        for (int i = 0; i < 9; i++)
        {
            bullets.Add(BulletPrefab);
            bullets[i] = _usePool ? _bulletPool.Get() : Instantiate(BulletPrefab, BulletOrigin.position + new Vector3(Random.Range(0.1f, 0.4f), Random.Range(0.1f, 0.15f), 0), Quaternion.identity);
        }
        #endregion

        #region Calculate Bullet Direction
        foreach (Bullet bullet in bullets)
        {
            bullet.SetDamage(weaponDamage);
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
        foreach (Bullet bullet in bullets)
        {
            bullet.Init(DestroyBullet);
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
    public void DestroyBullet(Bullet bullet)
    {
        if (_usePool) 
        {
            _bulletPool.Release(bullet);
        }
        else Destroy(bullet.gameObject);
    }
}