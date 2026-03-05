using UnityEngine;

public abstract class GunBase : IGun
{
    [SerializeField] protected WeaponProfiles weaponData;
    public WeaponProfiles WeaponData => weaponData;

    [SerializeField] protected Recoil recoil;
    public Recoil Recoil => recoil;

    public Movement movement;
    public HandSway handSway;

    public Transform bulletSpawnPoint;
    public Camera playerCamera;

    private int currentBurst;
    public bool isShooting, readyToShoot = true;
    private bool allowReset = true;

    public abstract void Shoot();

    public Vector3 CalculateSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
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

        Vector3 spread = Camera.main.transform.right * x + Camera.main.transform.up * y;

        Vector3 finalDirection = (direction + spread).normalized;

        return finalDirection;
    }

    public void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public void Initialize(Movement movementScript,
        ParticleSystem muzzleFlash,
        Camera playerCam,
        Recoil recoil)
    {
        this.movement = movementScript;
        this.playerCamera = playerCam;
        this.recoil = recoil;
    }
}

public interface IGun
{
    void Shoot();
    Vector3 CalculateSpread();
    void ResetShot();
}

public interface IAim
{
    public void AimDownSight();
    public void ExitAimDownSight();
}

public interface IReload
{
    public void Reload();
}