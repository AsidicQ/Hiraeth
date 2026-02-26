using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    [Header("Scripts")]
    public SwordAttack swordAttack;

    [Header("References")]
    public GameObject UI_Manager;

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
    }
}
