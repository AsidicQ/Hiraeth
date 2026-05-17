using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    [Header("Weapon Scripts")]
    public SwordAttack swordAttack;
    public ChakramMelee chakramMelee;
    //public RecoilProfiles recoilProfiles;
    public WeaponProfiles weaponStats;

    [Header("Player Scripts")]
    public Movement playerMovement;
    public CameraMovement lookScript;
    public HandSway handSway;

    [Header("References")]
    public GameObject UI_Manager;

    [Header("Transforms")]
    public Transform mainCameraTransform;

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
        chakramMelee.Initialize(UI_Manager.GetComponent<HitMarkers>());
    }
}
