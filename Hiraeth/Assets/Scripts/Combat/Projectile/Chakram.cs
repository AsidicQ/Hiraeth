using UnityEngine;

public class Chakram : GunBase
{
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (PlayerHealth.isDead) return;
        if (PauseMenu.isPaused) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isHolding = true;
            autoFireActivate = false;
            holdTimer = 0;

            TryShoot(weaponData.shootingDelay + weaponData.singleShotDelayFloat);
        }

        if (Input.GetKey(KeyCode.Mouse0) && isHolding)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer > weaponData.holdThreshold)
            {
                autoFireActivate = true;
            }

            if (autoFireActivate && readyToShoot)
            {
                TryShoot(weaponData.shootingDelay);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isHolding = false;
            autoFireActivate = false;
            holdTimer = 0;
        }
    }

    public void TryShoot(float delay)
    {
        if (!readyToShoot || currentAmmo <= 0) return;

        readyToShoot = false;

        Shoot();

        Invoke("ResetShot", delay);
    }

    public override void Shoot()
    {
        readyToShoot = false;
        currentAmmo--;
        reloading.UpdateAmmo();

        Vector3 shootingDirection = CalculateSpread().normalized;

        GameObject bullet = Instantiate(weaponData.bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * weaponData.bulletVelocity * 2, ForceMode.Force);
        bullet.transform.forward = shootingDirection;

        if (allowReset)
        {
            
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
