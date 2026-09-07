using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFireGun : MonoBehaviour
{
    [SerializeField] private InputAction shootAction;
    [SerializeField] private GameObject BulletPrefab;

    [SerializeField] private Transform BulletOrigin;

    Transform PlayerTransform;

    [SerializeField] private ParticleSystem ShotgunEffect;

    #region STATE PARAMETERS
    public float LastPressedShotgunShot { get; private set; }
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
            ShotgunEffect.Play();
            LastPressedShotgunShot = 0.75f;
        }
        #endregion
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, BulletOrigin.position, Quaternion.identity);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();

        bulletRB.AddForce(PlayerTransform.forward * 1000);

        float bulletDespawnTimer = 3;
        bulletDespawnTimer -= Time.deltaTime;

        if (bulletDespawnTimer < 0)
            Destroy(bullet);
    }

    #region CHECK METHODS
    private bool CanShoot()
    {
        return LastPressedShotgunShot < 0;
    }
    #endregion

}