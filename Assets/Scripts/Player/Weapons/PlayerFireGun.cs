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
                Shoot();
                WeaponEffect.Play();
                LastPressedShot = shotDelay;
            }
        }
        else if (weaponCollection.CurrentWeaponHeld == "SMG")
        {
            if (CanShoot() && shootAction.IsPressed())
            {
                Shoot();
                WeaponEffect.Play();
                LastPressedShot = shotDelay;
            }
        }
        
        #endregion
    }

    #region ACTION METHODS
    private void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, BulletOrigin.position, Quaternion.identity);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();

        bulletRB.AddForce(PlayerTransform.forward * 1500);

        float bulletDespawnTimer = 3;
        bulletDespawnTimer -= Time.deltaTime;

        if (bulletDespawnTimer < 0)
            Destroy(bullet);
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