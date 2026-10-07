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
    [SerializeField] public Bullet[] tearPrefabs;
    [SerializeField] private Transform tearOrigin;
    [SerializeField] public TextMeshProUGUI tearAmountText;

    CinemachineImpulseSource cameraShakeSource;
    private WeaponCollection weaponCollection;
    #endregion

    #region Weapon Variables
    [Header("Weapon Variables")]
    [SerializeField] private float shotDelay;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private float eyeDamage;
    #endregion

    [HideInInspector] public GameObject TearParent;

    public EyeballState state;
    public enum EyeballState
    {
        basic,
        poisonous,
    }

    #region EFFECTS
    [SerializeField] private Renderer rend;
    [SerializeField] private ParticleSystem WeaponEffect;
    #endregion

    #region STATE PARAMETERS
    public float LastPressedShot { get; private set; }

    private Transform PlayerTransform;

    public int MaxAmmo;
    private int ammo;
    [HideInInspector]
    public int Ammo
    {
        get
        {
            return Mathf.Clamp(ammo, 0, MaxAmmo);
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
        TearParent = new GameObject();
        TearParent.name = "TearPool";

        #region TEAR POOL
        tearPool = new ObjectPool<Bullet>(
            () =>
            {
                if (state == EyeballState.basic)
                    return Instantiate(tearPrefabs[0], tearOrigin.position, Quaternion.identity);
                else if (state == EyeballState.poisonous)
                    return Instantiate(tearPrefabs[1], tearOrigin.position, Quaternion.identity);
                else
                    return null;
            },
            bullet =>
            {
                bullet.transform.SetParent(TearParent.transform);
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

        ammo = MaxAmmo;
        tearAmountText.text = $"{Ammo}";

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
        if (bullet.name == "RegularTear(Clone)" && state == EyeballState.poisonous)
        {
            tearPool.Release(bullet);
            tearPool.Clear();
        }

        bullet.SetDamage(eyeDamage);
        bullet.GetComponent<Rigidbody>().AddForce(tearOrigin.forward * 1500);

        ammo--;
        tearAmountText.text = $"{Ammo}";

        bullet.Init(DestroyBullet);
    }
    private void Reload()
    {
        ammo = MaxAmmo;
        tearAmountText.text = $"{Ammo}";
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
        tearAmountText.text = $"{Ammo}";
    }
    public void CancelReload()
    {
        CancelInvoke("Reload");
    }
    public void DestroyBullet(Bullet bullet)
    {
        tearPool.Release(bullet);
    }
    public void SwitchTearState(EyeballState newState)
    {
        state = newState;
        tearPool.Clear();
        if (newState == EyeballState.basic)
        {
            rend.materials[1].color = Color.blue;
        }
        else if (newState == EyeballState.poisonous)
        {
            rend.materials[1].color = Color.greenYellow;
        }
    }
}
