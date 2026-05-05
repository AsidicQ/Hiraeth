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
    public RecoilProfiles currentRecoil { get; private set; }
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
            currentRecoil = startingRecoil;

            EquipWeapon(currentWeapon, currentRecoil, spawnOffset);
        }
    }

    public void EquipWeapon(WeaponProfiles newWeapon, RecoilProfiles weaponRecoilStats, Vector3 weaponPosition)
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

        currentWeaponObject = weaponFactory.CreateWeapon(newWeapon, weaponRecoilStats, weaponParent, spawnOffset);

        gunBase = currentWeaponObject.GetComponent<GunBase>();
        currentWeaponObject.transform.localPosition = weaponPosition;
        currentReloading = currentWeaponObject.GetComponent<Reloading>();
        currentWeapon = newWeapon;
        currentRecoil = weaponRecoilStats;
        currentWeaponObject.layer = LayerMask.NameToLayer("Hands/Weapon");

        if (currentWeaponObject == null)
        {
            Debug.LogError("WeaponFactory failed to create weapon!");
            return;
        }

        var weaponRecoil = currentWeaponObject.GetComponent<Recoil>();
        if (weaponRecoil != null)
        {
            weaponRecoil.Initialize(weaponRecoilStats, initializer.weaponCamera.transform);
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
        }

        var aimingScript = currentWeaponObject.GetComponent<Aiming>();
        if (aimingScript != null)
        {
            aimingScript.Initialize(
                initializer.playerMovement,
                initializer.lookScript,
                initializer.mainCamera,
                initializer.weaponCamera,
                newWeapon
            );
        }
    }
}