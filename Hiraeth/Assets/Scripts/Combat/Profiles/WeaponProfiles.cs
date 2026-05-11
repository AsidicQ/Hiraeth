using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Weapons/WeaponStats")]
public class WeaponProfiles : ScriptableObject
{
    [Header("Stats")]
    public float damage;
    public float bulletVelocity;
    public GameObject bulletPrefab;
    public GameObject weaponPrefab;
    public string weaponName;

    [Header("Shooting")]
    public float bulletPrefabLifeTime;
    public float shootingDelay;
    public float spreadIntensity;

    public enum ShootingMode { Single, Burst, Auto }
    public ShootingMode shootingMode;

    [Header("Burst Mode")]
    public int bulletsPerBurst;

    [Header("Reloading")]
    public int currentAmmo;
    public int maxAmmo;
    public int reserveAmmo;
    public float reloadTime;
    public float reloadTimeEmpty;

    [Header("Single Shot Delay")]
    public float singleShotDelayFloat;
    public float autoFireDelay = 0.08f;
    public float holdThreshold = 0.2f;

    [Header("Animations")]
    public RuntimeAnimatorController animatorController;
}
