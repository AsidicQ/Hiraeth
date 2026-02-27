using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    [Header("Scripts")]
    public SwordAttack swordAttack;
    public ShootScript shootScript;
    public Recoil recoil;
    public RecoilProfiles recoilProfiles;
    public Aiming aiming;
    public Movement playerMovement;
    public CameraMovement lookScript;
    public WeaponProfiles weaponStats;
    public Reloading reloading;

    [Header("References")]
    public GameObject UI_Manager;

    [Header("Transforms")]

    [Header("Cameras")]
    public Camera mainCamera;
    public Camera weaponCamera;

    [Header("UI Elements")]
    public TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI reloadingText;

    private void Awake()
    {
        BindAll();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindAll();
    }

    void BindAll()
    {
        swordAttack.Initialize(UI_Manager.GetComponent<HitMarkers>());
        recoil.Initialize(recoilProfiles, weaponCamera.transform);
        aiming.Initialize(playerMovement, lookScript, mainCamera, weaponCamera, weaponStats);
        reloading.Initialize(ammoCounter, reloadingText);
    }
}
