using UnityEngine;

public class WeaponEquipManager : MonoBehaviour
{
    public Transform weaponParent;
    [SerializeField] private WeaponProfiles startingWeapon;
    [SerializeField] private RecoilProfiles startingRecoil;
    public GunBase gunBase;
    private IWeaponFactory weaponFactory;
    private GameObject currentWeaponObject;
    public Initializer initializer;

    public WeaponProfiles currentWeapon { get; private set; }
    public Reloading currentReloading { get; private set; }

    public Vector3 spawnOffset;

    private void Awake()
    {
        weaponFactory = new WeaponFactory();
    }

    public void Start()
    {
        if (startingWeapon != null)
        {
            currentWeapon = startingWeapon;

            EquipWeapon(currentWeapon, spawnOffset);
        }
    }

    public void EquipWeapon(WeaponProfiles newWeapon, Vector3 weaponPosition)
    {
        if (newWeapon == null) return;

        if (initializer == null)
        {
            Debug.LogError("Initializer missing!");
            return;
        }

        if (initializer.mainCameraTransform == null)
        {
            Debug.LogError("mainCameraTransform not assigned in Initializer!");
        }

        if (currentWeaponObject != null)
            Destroy(currentWeaponObject);

        currentWeaponObject = weaponFactory.CreateWeapon(newWeapon, weaponParent, spawnOffset);

        gunBase = currentWeaponObject.GetComponent<GunBase>();
        currentWeapon = newWeapon;
        
        int projectileLayer = LayerMask.NameToLayer("Player/Combat/Projectile");

        if (projectileLayer == -1)
        {
            Debug.LogError("Layer 'Player/Combat/Projectile' not found!");
            return;
        }

        SetLayerRecursively(currentWeaponObject, LayerMask.NameToLayer("Player/Combat/Projectile"));

        if (currentWeaponObject == null)
        {
            Debug.LogError("WeaponFactory failed to create weapon!");
            return;
        }

        if (gunBase != null)
        {
            gunBase.Initialize(
                weaponContext: new WeaponContext
                {
                    movement = initializer.playerMovement,
                    handSway = initializer.handSway,
                    playerCamera = initializer.mainCamera
                }
            );
        }

        var reloading = currentWeaponObject.GetComponent<Reloading>();
        if (reloading != null)
        {
            reloading.Initialize(initializer.ammoCounter, initializer.reloadingText);
            currentReloading = reloading;
        }
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}