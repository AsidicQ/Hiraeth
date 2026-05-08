using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Reloading))]
public abstract class GunBase : MonoBehaviour, IGun, IReload
{
    [SerializeField] protected WeaponProfiles weaponData;
    public WeaponProfiles WeaponData => weaponData;
    [SerializeField] protected Recoil recoil;
    public Recoil Recoil => recoil;

    public Movement movement;
    public HandSway handSway;
    public Reloading reloading;

    public int currentAmmo;
    public int reserveAmmo;
    public int maxAmmo;

    [Header("Injected Dependencies")]
    public Transform bulletSpawnPoint;
    public Camera playerCamera;

    public int currentBurst;
    public bool isShooting, readyToShoot = true;
    public bool allowReset = true;

    public bool isReloading;

    public void Start()
    {
        reloading = GetComponent<Reloading>();
        recoil = GetComponent<Recoil>();
        reloading.UpdateAmmo();
    }

    public void ApplyWeaponData(WeaponProfiles weaponData)
    {
        this.weaponData = weaponData;

        currentAmmo = weaponData.currentAmmo;
        reserveAmmo = weaponData.reserveAmmo;
        maxAmmo = weaponData.maxAmmo;
        reloading.UpdateAmmo();
    }

    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;

        reloading.UpdateAmmo();
    }

    public abstract void Shoot();

    public abstract Vector3 CalculateSpread();

    public void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public void Initialize(WeaponContext weaponContext)
    {
        this.movement = weaponContext.movement;
        this.handSway = weaponContext.handSway;
        this.playerCamera = weaponContext.playerCamera;

        Debug.Log("Initialize called");
    }

    public IEnumerator Reload()
    {
        Reloading reloadComponent = reloading;

        if (isReloading || currentAmmo == maxAmmo || reserveAmmo <= 0)
            yield break;

        isReloading = true;

        reloadComponent.reloadingText.enabled = true;

        if (handSway != null)
            handSway.enabled = false;

        reloadComponent.animator.SetTrigger("ReloadDown");
        StartCoroutine(reloadComponent.PlayAfterAnimation("ReloadAnim", "ReloadUpAnim"));

        float reloadDuration = currentAmmo > 0 ? weaponData.reloadTime : weaponData.reloadTimeEmpty;
        yield return new WaitForSeconds(reloadDuration);

        if (handSway != null)
            handSway.enabled = true;

        int ammoToReload = Mathf.Min(maxAmmo - currentAmmo, reserveAmmo);
        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        isReloading = false;

        reloadComponent.reloadingText.enabled = false;

        reloadComponent.UpdateAmmo();
    }
}

public class WeaponContext
{
    public Movement movement;
    public HandSway handSway;
    public Camera playerCamera;
}

public interface IGun
{
    void Shoot();
    Vector3 CalculateSpread();
    void ResetShot();
}

public interface IReload
{
    public IEnumerator Reload();
}