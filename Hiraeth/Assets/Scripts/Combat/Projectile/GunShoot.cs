using UnityEngine;

public class GunShoot : GunBase, IGun
{
    private Camera plyrCamera;

    public override void Shoot()
    {
        //Gun Shoot Logic here
    }

    public Vector3 CalculateSpread()
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

    public void Initialize(Camera playerCam)
    {
        this.plyrCamera = playerCam;
    }
}
