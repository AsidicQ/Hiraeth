using UnityEngine;

public class GunShoot : GunBase
{
    private void HandleInput()
    {
        if (PlayerHealth.isDead) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (readyToShoot && currentAmmo > 0)
            {
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            isAiming = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            isAiming = false;
        }

        if (PauseMenu.isPaused) return;
    }

    public override void Shoot()
    {
        readyToShoot = false;
        muzzleFlash.Play();

        Vector3 shootingDirection = CalculateSpread().normalized;

        GameObject bullet = Instantiate(weaponData.bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * weaponData.bulletVelocity, ForceMode.Force);
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

    public override Vector3 CalculateSpread()
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
}
