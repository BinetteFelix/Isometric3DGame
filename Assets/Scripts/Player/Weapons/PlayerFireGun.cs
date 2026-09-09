using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFireGun : MonoBehaviour
{
    #region INPUT ACTIONS
    [SerializeField] private InputAction shootAction;
    #endregion

    #region COMPONENTS
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform BulletOrigin;
    [SerializeField] private float shotDelay;

    private WeaponCollection weaponCollection;
    #endregion

    #region EFFECTS
    [SerializeField] private ParticleSystem WeaponEffect;
    #endregion

    #region STATE PARAMETERS
    public float LastPressedShot { get; private set; }

    private Transform PlayerTransform;
    #endregion

    private void Awake()
    {
        shootAction.Enable();
    }
    private void Start()
    {
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
        
        #endregion
    }

    #region ACTION METHODS
    private void ShootSMG()
    {
        GameObject bullet = Instantiate(BulletPrefab, BulletOrigin.position, Quaternion.identity);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();

        bulletRB.AddForce(PlayerTransform.forward * 1500);

        float bulletDespawnTimer = 3;
        bulletDespawnTimer -= Time.deltaTime;

        if (bulletDespawnTimer < 0)
            Destroy(bullet);
    }
    private void ShootShotgun()
    {
        List<GameObject> bullets = new List<GameObject>();

        for (int i = 0; i < 8; i++)
        {
            bullets.Add(BulletPrefab);
            bullets[i] = Instantiate(BulletPrefab, BulletOrigin.position + new Vector3(Random.Range(0.1f, 0.4f), Random.Range(0.1f, 0.15f), 0), Quaternion.identity);
        }
        foreach (GameObject bullet in bullets)
        {
            Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
            bulletRB.AddForce(PlayerTransform.forward * 1750);
        }

        float bulletDespawnTimer = 3;
        bulletDespawnTimer -= Time.deltaTime;

        if (bulletDespawnTimer < 0)
        {
            bullets.Remove(bullets[0]);
            Destroy(bullets[0]);
        }
    }
    #endregion

    #region CHECK METHODS
    private bool CanShoot()
    {
        return LastPressedShot < 0;
    }
    public float SetShotDelay(float delay)
    {
        shotDelay = delay;
        return shotDelay;
    }
    #endregion


}