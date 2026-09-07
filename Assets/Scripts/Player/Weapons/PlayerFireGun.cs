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
    #endregion

    #region EFFECTS
    [SerializeField] private ParticleSystem WeaponEffect;
    #endregion

    #region STATE PARAMETERS
    public float LastPressedShotgunShot { get; private set; }
    private float shotgunShotDelay = 0.62f;

    private Transform PlayerTransform;
    #endregion

    private void Awake()
    {
        shootAction.Enable();
    }
    private void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {

        #region TIMERS
        LastPressedShotgunShot -= Time.deltaTime;
        #endregion

        #region INPUT HANDLER
        if (CanShoot() && shootAction.WasPressedThisFrame())
        {
            Shoot();
            WeaponEffect.Play();
            LastPressedShotgunShot = shotgunShotDelay;
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
        return LastPressedShotgunShot < 0;
    }
    #endregion

}