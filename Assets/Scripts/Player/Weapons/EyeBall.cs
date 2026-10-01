using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class EyeBall : MonoBehaviour
{
    #region INPUT ACTIONS
    [SerializeField] private InputAction shootAction;
    [SerializeField] private InputAction reloadAction;
    #endregion

    #region OBEJCT POOLING
    public ObjectPool<Bullet> tearPool;
    #endregion

    #region COMPONENTS
    [SerializeField] private Bullet[] tearPrefabs;
    [SerializeField] private Transform tearOrigin;
    [SerializeField] private TextMeshProUGUI tearAmount;

    CinemachineImpulseSource cameraShakeSource;
    private WeaponCollection weaponCollection;
    #endregion

    #region Weapon Variables
    [Header("Weapon Variables")]
    [SerializeField] private float shotDelay;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private float eyeDamage;
    #endregion

    [SerializeField] private Transform TearParent;

    public TearState state;
    public enum TearState
    {
        basic,
        poisonous
    }

    #region EFFECTS
    [SerializeField] private Renderer rend;
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
        #region TEAR POOL
        tearPool = new ObjectPool<Bullet>(
            () =>
            {
                if (state == TearState.basic)
                    return Instantiate(tearPrefabs[0], tearOrigin.position, Quaternion.identity);
                else if (state == TearState.poisonous)
                    return Instantiate(tearPrefabs[1], tearOrigin.position, Quaternion.identity);
                else
                    return null;
            },
            bullet =>
            {
                bullet.transform.SetParent(TearParent);
                bullet.transform.position = tearOrigin.position;
                bullet.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                bullet.gameObject.SetActive(true);
            },
            bullet =>
            {
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
        #endregion

        ammo = maxAmmo;
        tearAmount.text = $"{Ammo}";

        cameraShakeSource = GetComponent<CinemachineImpulseSource>();
        weaponCollection = GetComponentInParent<WeaponCollection>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        #region TIMERS
        LastPressedShot -= Time.deltaTime;
        #endregion

        #region INPUT HANDLER
        if (CanShoot() && shootAction.IsPressed())
        {
            Shoot();
            WeaponEffect.Play();
            LastPressedShot = shotDelay;
        }
        if (Ammo <= 0 && shootAction.WasPressedThisFrame())
        {
            Invoke("Reload", reloadSpeed);
        }
        if (reloadAction.WasPressedThisFrame())
        {
            Invoke("Reload", reloadSpeed);
        }
        #endregion
    }

    #region ACTION METHODS
    private void Shoot()
    {
        CancelReload();
        Bullet bullet = tearPool.Get();
        if (bullet.name == "RegularTear(Clone)" && state == TearState.poisonous)
        {
            tearPool.Release(bullet);
            tearPool.Clear();
        }

        bullet.SetDamage(eyeDamage);
        bullet.GetComponent<Rigidbody>().AddForce(tearOrigin.forward * 1500);

        ammo--;
        tearAmount.text = $"{Ammo}";

        bullet.Init(DestroyBullet);
    }
    private void Reload()
    {
        ammo = maxAmmo;
        tearAmount.text = $"{Ammo}";
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
        tearAmount.text = $"{Ammo}";
    }
    public void CancelReload()
    {
        CancelInvoke("ReloadGun");
    }
    public void DestroyBullet(Bullet bullet)
    {
        tearPool.Release(bullet);
    }
    public void SwitchTearState(TearState newState)
    {
        state = newState;
        tearPool.Clear();
        if (newState == TearState.basic)
        {
            rend.materials[1].color = Color.blue;
        }
        else if (newState == TearState.poisonous)
        {
            rend.materials[1].color = Color.greenYellow;
        }
    }
}
