using UnityEngine;
using System.Collections;

public class ShootScript : MonoBehaviour
{
    [Header("Stats")]
    public WeaponProfiles weaponData;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Camera plyrCamera;
    private int currentBurst;

    [Header("Input")]
    [SerializeField] private KeyCode aimingKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode shootingKey = KeyCode.Mouse0;

    [Header("Bools")]
    public bool isShooting, readyToShoot = true;
    [SerializeField] private bool allowReset = true;
    public bool isReloading;
    public bool isAiming;

    [Header("References")]
    //public WeaponRecoil Recoil;
    public HandSway handSway;
    public Movement movementScript;
    public Reloading reloading;

    [Header("Reloading")]
    public Animator animator;

    public void Initialize(Movement movementScript,
        ParticleSystem muzzleFlash,
        Camera playerCam)
        //WeaponRecoil recoil)
    {
        this.movementScript = movementScript;
        this.muzzleFlash = muzzleFlash;
        this.plyrCamera = playerCam;
        //Recoil = recoil;
    }

    public void ApplyWeaponData(WeaponProfiles data)
    {
        this.weaponData = data;
    }

    private void Start()
    {
        currentBurst = weaponData.bulletsPerBurst;
        reloading.UpdateAmmo();

        handSway = GetComponentInParent<HandSway>();
        reloading = GetComponent<Reloading>();
    }

    private void Update()
    {
        HandleInput();
        reloading.UpdateAmmo();
    }

    private void HandleInput()
    {
        if (weaponData.shootingMode == WeaponProfiles.ShootingMode.Auto)
            isShooting = Input.GetKey(shootingKey) && reloading.currentAmmo > 0;
        else
            isShooting = Input.GetKeyDown(shootingKey) && reloading.currentAmmo > 0;

        if (readyToShoot && isShooting && !isReloading && !movementScript.isSprinting)
        {
            currentBurst = weaponData.bulletsPerBurst;
            Shoot();
            //Recoil.Recoil(isAiming);
            //Recoil.HandleRecoilAnimation();
        }

        isAiming = Input.GetKey(aimingKey);
    }

    void Shoot()
    {
        readyToShoot = false;
        muzzleFlash.Play();
        reloading.currentAmmo--;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(weaponData.bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * weaponData.bulletVelocity, ForceMode.Force);
        StartCoroutine(DestroyBulletAfterTime(bullet, weaponData.bulletPrefabLifeTime));
        bullet.transform.forward = shootingDirection;

        if (allowReset)
        {
            Invoke("ResetShot", weaponData.shootingDelay);
            allowReset = false;
        }

        if (weaponData.shootingMode == WeaponProfiles.ShootingMode.Burst && currentBurst > 1)
        {
            currentBurst--;
            Invoke("Shoot", weaponData.shootingDelay);
        }
    }

    private IEnumerator spawnTrail(TrailRenderer Trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = hit.point;

        Destroy(Trail.gameObject, Trail.time);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = plyrCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = (targetPoint - bulletSpawnPoint.position).normalized;
        float x = Random.Range(-weaponData.spreadIntensity, weaponData.spreadIntensity);
        float y = Random.Range(-weaponData.spreadIntensity, weaponData.spreadIntensity);

        Vector3 spread = plyrCamera.transform.right * x + plyrCamera.transform.up * y;

        Vector3 finalDirection = (direction + spread).normalized;

        return finalDirection;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
}

