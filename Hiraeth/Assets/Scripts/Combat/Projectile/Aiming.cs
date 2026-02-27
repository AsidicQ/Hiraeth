using UnityEngine;

public class Aiming : MonoBehaviour
{
    [Header("Settings")]
    public float sensDecrease = 2f;
    public float aimMultiplier = 2f;
    public float ADS_Speed = 10f;
    public Camera mainCam;
    public Camera gunCam;
    public float defaultFOV;
    public float adsFOV;
    public float defaultGunFOV;
    public float FOV_speed;
    private float targetFOV;
    private float gunFOV;

    [Header("References")]
    public WeaponProfiles weaponStats;
    public ShootScript shootingFunc;
    public Movement movementFunc;
    public CameraMovement lookFunc;
    public HandSway weaponSway;

    [Header("ADS Targeting")]
    public Transform sightTarget;

    private Vector3 weaponStartPos;
    private Quaternion weaponStartRot;

    private float defaultSpread;
    private float defaultSensitivity;
    public GameObject crossHair;

    private bool wasAiming = false;

    void Start()
    {
        weaponStartPos = transform.localPosition;
        weaponStartRot = transform.localRotation;

        mainCam = GetComponentInParent<Camera>();
        shootingFunc = GetComponentInChildren<ShootScript>();
        weaponSway = GetComponentInParent<HandSway>();

        targetFOV = defaultFOV;
        gunFOV = defaultGunFOV;
    }

    void Update()
    {
        if (shootingFunc.isAiming)
        {
            AimDownSight();
            if (!wasAiming) EnterADS();
        }
        else
        {
            ReturnPosition();
            if (wasAiming) ExitADS();
        }

        mainCam.fieldOfView = Mathf.Lerp(
            mainCam.fieldOfView,
            targetFOV,
            FOV_speed * Time.deltaTime);

        gunCam.fieldOfView = Mathf.Lerp(
            gunCam.fieldOfView,
            gunFOV,
            FOV_speed * Time.deltaTime);
    }

    private void AimDownSight()
    {
        if (sightTarget == null)
        {
            Debug.LogWarning("SightTarget is not assigned!");
            return;
        }

        Vector3 offset = sightTarget.position - transform.position;

        Vector3 desiredPos = mainCam.transform.position + mainCam.transform.forward * offset.magnitude - offset;

        transform.position = Vector3.Lerp(transform.position, desiredPos, ADS_Speed * Time.deltaTime);

        Quaternion desiredRot = Quaternion.LookRotation(mainCam.transform.forward, mainCam.transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, ADS_Speed * Time.deltaTime);
    }

    public void ReturnPosition()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, weaponStartPos, ADS_Speed * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, weaponStartRot, ADS_Speed * Time.deltaTime);
    }

    private void EnterADS()
    {
        weaponSway.enabled = false;
        weaponStats.spreadIntensity = defaultSpread / aimMultiplier;
        lookFunc.sensitivityAmount = defaultSensitivity / sensDecrease;
        movementFunc.canSprint = false;
        wasAiming = true;
        targetFOV = adsFOV;
        gunFOV = defaultGunFOV;
        crossHair.SetActive(false);
    }

    private void ExitADS()
    {
        weaponSway.enabled = true;
        weaponStats.spreadIntensity = defaultSpread;
        lookFunc.sensitivityAmount = defaultSensitivity;
        movementFunc.canSprint = true;
        wasAiming = false;
        targetFOV = defaultFOV;
        gunFOV = defaultGunFOV;
        crossHair.SetActive(true);
    }

    public void Initialize(Movement move,
        CameraMovement lookScript,
        Camera playerCamera,
        Camera fpsCam,
        WeaponProfiles weaponStats)
    {
        this.movementFunc = move;
        this.lookFunc = lookScript;
        this.mainCam = playerCamera;
        this.gunCam = fpsCam;
        this.weaponStats = weaponStats;

        defaultSpread = weaponStats.spreadIntensity;
        defaultSensitivity = lookFunc.sensitivityAmount;
    }
}
