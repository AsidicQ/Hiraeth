using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    public RecoilProfiles weaponRecoil;
    public float hipfireRecoilMultiplier;
    public float adsRecoilMultiplier;

    [Header("References")]
    [SerializeField] private Transform plyrCam;
    private Vector3 initialGunPosition;

    private Vector3 currentGunRotation;
    private Vector3 targetGunRotation;
    private Vector3 currentGunPosition;
    private Vector3 targetGunPosition;

    private Vector3 currentCamRotation;
    private Vector3 targetCamRotation;

    public void Initialize(RecoilProfiles weaponRecoil, Transform plyrCam)
    {
        this.weaponRecoil = weaponRecoil;
        this.plyrCam = plyrCam;
    }


    void Start()
    {
        if (plyrCam == null)
        {
            Debug.LogError("Player camera (plyrCam) is not assigned. Please make sure it is assigned.");
            enabled = false;
            return;
        }

        initialGunPosition = transform.localPosition;
    }

    void Update()
    {
        HandleRecoilAnimation();
    }

    public void RecoilFunction(bool isAiming)
    {
        float aimMultiplier = isAiming ? adsRecoilMultiplier : hipfireRecoilMultiplier;

        targetGunRotation += new Vector3(weaponRecoil.recoilX * aimMultiplier, Random.Range(-weaponRecoil.recoilY, weaponRecoil.recoilY) * aimMultiplier, 0);
        targetGunPosition -= new Vector3(0, 0, weaponRecoil.kickBack * aimMultiplier);

        targetCamRotation += new Vector3(weaponRecoil.recoilX * aimMultiplier, Random.Range(-weaponRecoil.recoilY, weaponRecoil.recoilY) * aimMultiplier, 0);
    }

    public void HandleRecoilAnimation()
    {
        targetGunRotation = Vector3.Lerp(targetGunRotation, Vector3.zero, Time.deltaTime * weaponRecoil.returnSpeed);
        currentGunRotation = Vector3.Slerp(currentGunRotation, targetGunRotation, Time.deltaTime * weaponRecoil.snapAmount);
        transform.localRotation = Quaternion.Euler(currentGunRotation);

        targetGunPosition = Vector3.Lerp(targetGunPosition, initialGunPosition, Time.deltaTime * weaponRecoil.returnSpeed);
        currentGunPosition = Vector3.Lerp(currentGunPosition, targetGunPosition, Time.deltaTime * weaponRecoil.snapAmount);
        transform.localPosition = currentGunPosition;

        targetCamRotation = Vector3.Lerp(targetCamRotation, Vector3.zero, Time.deltaTime * weaponRecoil.returnSpeed);
        currentCamRotation = Vector3.Slerp(currentCamRotation, targetCamRotation, Time.deltaTime * weaponRecoil.snapAmount);
        plyrCam.localRotation *= Quaternion.Euler(currentCamRotation);
    }
}
